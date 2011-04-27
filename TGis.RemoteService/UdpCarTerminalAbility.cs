using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.Common;
using System.Net.Sockets;
using System.Threading;

namespace TGis.RemoteService
{
    public class UdpCarTerminalAbility : ICarTerminalAbility
    {
        int port;
        UdpClient server;
        public static TcpServer tcp_server;
        bool bStop;
        public bool CanInteract
        {
            get { return false; }
        }
        public event CarTerminalStateChangeHandler OnCarStateChanged;
        public void Run()
        {
            bStop = false;
            this.port = 4999;
            server = new UdpClient(this.port);
            server.BeginReceive(OnRecv, server);
            tcp_server = new TcpServer();
            tcp_server.Run(this.port);
        }
        public void Stop()
        {
            bStop = true;
            server.Close();
            tcp_server.Stop();
        }
        private void OnRecv(IAsyncResult ar)
        {
            if (bStop) return;
            var server = ar.AsyncState as UdpClient;
            try
            {
                System.Net.IPEndPoint endpoint = new System.Net.IPEndPoint(0, 0);
                byte[] data = server.EndReceive(ar, ref endpoint);
                DecodedProtocolStatus status = new DecodedProtocolStatus();
                if (ProtocolDecoder.Decode(data, ref status))
                {
                    CarTernimalStateArg arg = new CarTernimalStateArg();
                    arg.Time = status.Time;
                    arg.RollDirection = status.RollForward ? CarRollDirection.Forward : CarRollDirection.Backward;
                    arg.X = status.Longitude;
                    arg.Y = status.Latitude;
                    arg.PhoneNum = status.SerialNum;
                    if (OnCarStateChanged != null)
                        OnCarStateChanged(this, arg);
                }
            }
            catch (System.Exception e)
            {
            	
            }
            server.BeginReceive(OnRecv, server);
        }
    }

    public class TerminalState
    {
        TcpClient client = null;     
        DateTime tmConnect = DateTime.MinValue;
        string serialNum = null;
        NetworkStream stream = null;

        public TcpClient ClientSocket
        {
            get { return client; }
            set { client = value; }
        }
        public DateTime TmConnect
        {
            get { return tmConnect; }
            set { tmConnect = value; }
        }
        public string SerialNum
        {
            get { return serialNum; }
            set { serialNum = value; }
        }
        public NetworkStream Stream
        {
            get { return stream; }
            set { stream = value; }
        }
    }
    public delegate void MsgShowHandler(string msg);
    public class TcpServer
    {
        TcpListener listener;
        int port;
        Thread loopThread;
        IDictionary<string, TerminalState> dictTerminals = new Dictionary<string, TerminalState>();
        bool bStop;
        public event MsgShowHandler OnMsg;
        public void Run(int port)
        {
            dictTerminals.Clear();
            this.port = port;
            bStop = false;
            loopThread = new Thread(RunThread);
            loopThread.Start();
            Dispatch("tcpserver:begin");
        }
        private void RunThread()
        {
            listener = new TcpListener(port);
            listener.Start();
            while (!bStop)
            {
                var ar = listener.BeginAcceptTcpClient(OnAcceptClient, listener);
                ar.AsyncWaitHandle.WaitOne();
            }
            
        }
        public void Stop()
        {
            bStop = true;
            loopThread.Join();
        }

        private void OnAcceptClient(IAsyncResult ar)
        {
            Dispatch("tcpserver:accept");
            TcpListener listener = ar.AsyncState as TcpListener;
            if (listener == null) return;
            if (bStop) return;
            TerminalState ns = new TerminalState();
            try
            {

                ns.ClientSocket = listener.EndAcceptTcpClient(ar);
                ns.Stream = ns.ClientSocket.GetStream();
                ns.TmConnect = DateTime.Now;
                BeginAsynRead(ns, listener);
            }
            catch (System.Exception ex)
            {
                CloseTerminalState(ns);
            }

        }
        private void BeginAsynRead(TerminalState state, TcpListener listener)
        {
            try
            {
                if (bStop) return;
                byte[] buffer = new byte[1024];
                Tuple<TerminalState, TcpListener, byte[]> context = new Tuple<TerminalState, TcpListener, byte[]>(state, listener, buffer);
                state.Stream.BeginRead(buffer, 0, buffer.Length, OnDataArrived, context);
            }
            catch (System.Exception ex)
            {
                CloseTerminalState(state);
            }

        }

        private void OnDataArrived(IAsyncResult ar)
        {
            Dispatch("tcpserver:dataarrive");
            var context = ar.AsyncState as Tuple<TerminalState, TcpListener, byte[]>;
            if (context == null) return;
            if (bStop) return;
            try
            {
                if(context.Item1.Stream.EndRead(ar) == 0)
                    throw new ApplicationException();
                if (context.Item1.SerialNum == null)
                {
                    if (context.Item3[0] != 0x2a)
                        throw new ApplicationException();
                    string str = System.Text.Encoding.ASCII.GetString(context.Item3, 0, context.Item3.Length);
                    string[] elements = str.Split(',');
                    //if ((elements.Length != 19))
                    //    throw new ApplicationException();
                    context.Item1.SerialNum = elements[1];
                    lock (this)
                    {
                        TerminalState old;
                        if (dictTerminals.TryGetValue(context.Item1.SerialNum, out old))
                        {
                            old.Stream.Close();
                            old.ClientSocket.Close();
                            dictTerminals.Remove(context.Item1.SerialNum);
                        }
                        dictTerminals[context.Item1.SerialNum] = context.Item1;
                    }
                    string strConf1 = string.Format("*HQ,000,D1,{0:D2}{1:D2}{2:D2},600,1#",
                        DateTime.UtcNow.Hour,  DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
                    byte[] config1 = System.Text.Encoding.ASCII.GetBytes(strConf1);
                    string strConf2 = string.Format("*HQ,000,S17,{0:D2}{1:D2}{2:D2},10,1#",
                        DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
                    byte[] config2 = System.Text.Encoding.ASCII.GetBytes(strConf2);
                    context.Item1.Stream.Write(config1, 0, config1.Length);
                    context.Item1.Stream.Write(config2, 0, config2.Length);
                }
                context.Item1.Stream.BeginRead(context.Item3, 0, context.Item3.Length, OnDataArrived, context);
            }
            catch (System.Exception)
            {
                CloseTerminalState(context.Item1);
            }
        }
        private void CloseTerminalState(TerminalState ts)
        {
            Dispatch("tcpserver:close");
            lock (this)
            {
                TerminalState old;

                if ((ts.SerialNum != null) && dictTerminals.TryGetValue(ts.SerialNum, out old))
                {
                    dictTerminals.Remove(ts.SerialNum);
                }
            }
            if (ts.Stream != null)
                ts.Stream.Close();
            if (ts.ClientSocket != null)
                ts.ClientSocket.Close();
        }
        private void Dispatch(string msg)
        {
            if (OnMsg != null)
                OnMsg(msg);
        }
    }
}
