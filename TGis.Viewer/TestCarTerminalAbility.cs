using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TGis.Common;

namespace TGis.Viewer
{
    class TestCarTerminalAbility : ICarTerminalAbility
    {
        private Thread thread = null;
        private bool bExit = false;
        private double xs = 116.01;
        private double ys = 38.42;
        private double xe = 116.14;
        private double ye = 38.56;
        private int times = 100;
        private int cur_times = 0;
        private int interval = 1000;
        private int carnum = 20;
        public bool CanInteract
        {
            get { return false; }
        }
        public event CarTerminalStateChangeHandler OnCarStateChanged;
        public void Run()
        {
            bExit = false;
            thread = new Thread(Product);
            thread.Start();
        }
        public void Stop()
        {
            bExit = true;
            thread.Join();
        }
        private void Product()
        {
            while (!bExit)
            {
                for (int i = 1; i <= carnum; i++)
                {
                    double x = xs + (xe - xs) / times * cur_times + i * 0.01;
                    double y = ys + (ye - ys) / times * cur_times;
                    if (OnCarStateChanged != null)
                    {
                        CarStateArg arg = new CarStateArg();
                        arg.PhoneNum = i.ToString();
                        arg.RollDirection = CarRollDirection.Forward;
                        arg.Time = DateTime.Now;
                        arg.X = x;
                        arg.Y = y;
                        OnCarStateChanged(this, arg);
                    }
                }
                cur_times++;
                if (cur_times > times)
                    cur_times = 0;
                Thread.Sleep(interval);
            }
        }
    }
}
