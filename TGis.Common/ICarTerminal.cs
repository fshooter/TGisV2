﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGis.Common
{
    public enum CarProcResult
    {
        Ok,
        Expired,
        TimeNotReach,// 未达到当前时刻
        Miss, 
    }
    public enum CarRollDirection
    {
        Forward,
        Backward,
    }
    public class CarTernimalStateArg
    {
        private string phoneNum;
        private double x;
        private double y;
        private CarRollDirection rollDirection;
        private DateTime tm;

        public DateTime Time
        {
            get { return tm; }
            set { tm = value; }
        }

        public string PhoneNum
        {
          get { return phoneNum; }
          set { phoneNum = value; }
        }

        public double X
        {
          get { return x; }
          set { x = value; }
        }
       
        public double Y
        {
          get { return y; }
          set { y = value; }
        }

        public CarRollDirection RollDirection
        {
          get { return rollDirection; }
          set { rollDirection = value; }
        }
    }
    public delegate CarProcResult CarTerminalStateChangeHandler(object sender, CarTernimalStateArg arg);

    public interface ICarTerminalAbility
    {
        bool CanInteract { get; }
        event CarTerminalStateChangeHandler OnCarStateChanged;
        void Run();
        void Stop();
    }
}
