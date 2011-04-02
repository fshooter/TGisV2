using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TGis.Common;
using TGis.MapControl;

namespace TGis.Viewer
{
    partial class ViewGisCar : Form
    {
        private GisCarModel model;
        private GisCarController controller;
        private bool bInitComplete = false;
        public ViewGisCar(GisCarModel model, GisCarController controller)
        {
            this.model = model;
            this.controller = controller;

            InitializeComponent();
        }

        private void ViewGisCar_Load(object sender, EventArgs e)
        {
            mapControl1.Navigate(Ultility.GetAppDir() + @"\map\map.html");
            mapControl1.OnMapLoadCompleted += new MapLoadCompleteHandler(InitMapFirstTime);
            this.model.CsMgr.OnCarSessionStateChanged += new CarSessionStateChangeHandler(CarSessionState_Change);
        }

        private void CarSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            if (!bInitComplete) return;
            MapControlSessionState_Change(sender, arg);
        }
        private void MapControlSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            switch (arg.ReasonArg)
            {
                case CarSessionStateChangeArgs.Reason.Connect:
                    mapControl1.AsynAddCar(arg.CarSessionArg.CarInstance.Id, arg.CarSessionArg.CarInstance.Name,
                        arg.CarSessionArg.X, arg.CarSessionArg.Y,
                        arg.CarSessionArg.ExceptionState);
                    break;
                case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                    mapControl1.AsynUpdateCar(arg.CarSessionArg.CarInstance.Id, arg.CarSessionArg.CarInstance.Name,
                        arg.CarSessionArg.X, arg.CarSessionArg.Y,
                        arg.CarSessionArg.ExceptionState);
                    break;
                case CarSessionStateChangeArgs.Reason.Disconnect:
                    mapControl1.AsynRemoveCar(arg.CarSessionArg.CarInstance.Id);
                    break;
            }
        }
        private void TableControlSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            switch (arg.ReasonArg)
            {
                case CarSessionStateChangeArgs.Reason.Add:
                case CarSessionStateChangeArgs.Reason.Remove:
                case CarSessionStateChangeArgs.Reason.Update:
                    break;
                case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                case CarSessionStateChangeArgs.Reason.Connect:
                case CarSessionStateChangeArgs.Reason.Disconnect:
                    break;
            }
        }
        private void EnumCar_Handler(CarSession cs)
        {
            mapControl1.AsynAddCar(cs.CarInstance.Id, cs.CarInstance.Name, cs.X, cs.Y,
                cs.RollDirection == CarRollDirection.Forward ? true : false);
        }
        private void InitMapFirstTime(object map)
        {
            model.CsMgr.EnumCarSession(new CarSessionMgr.EnumCarSessionHandler(EnumCar_Handler));
            bInitComplete = true;
        }

        private void ViewGisCar_FormClosing(object sender, FormClosingEventArgs e)
        {
            mapControl1.OnMapLoadCompleted -= new MapLoadCompleteHandler(InitMapFirstTime);
            this.model.CsMgr.OnCarSessionStateChanged -= new CarSessionStateChangeHandler(CarSessionState_Change);
        }
    }
}
