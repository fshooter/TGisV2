using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.Common;
using System.Net.Sockets;
using System.Threading;

namespace TGis.RemoteService
{
    class UdpCarTerminalAbility : ICarTerminalAbility
    {
        int port;
        UdpClient server;
        Thread thread;
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
        }
        public void Stop()
        {
            server.Close();
        }
        private void OnRecv(IAsyncResult ar)
        {
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
            catch (System.Exception)
            {
            	
            }
            server.BeginReceive(OnRecv, server);
        }
    }
}
