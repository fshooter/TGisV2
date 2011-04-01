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

            this.model.CsMgr.OnCarSessionStateChanged += new CarSessionStateChangeHandler(CarSessionState_Change);
            InitializeComponent();
        }

        private void ViewGisCar_Load(object sender, EventArgs e)
        {
            mapControl1.Navigate(Ultility.GetAppDir() + @"\map\map.html");
            mapControl1.OnMapLoadCompleted += new MapLoadCompleteHandler(InitMapFirstTime);
        }

        private void CarSessionState_Change(object sender, CarSessionStateChangeArgs arg)
        {
            if (!bInitComplete) return;
            switch (arg.ReasonArg)
            {
                case CarSessionStateChangeArgs.Reason.Connect:
                    mapControl1.AsynAddCar(arg.CarSessionArg.CarInstance.Id, arg.CarSessionArg.CarInstance.Name,
                        arg.CarSessionArg.X, arg.CarSessionArg.Y,
                        arg.CarSessionArg.RollDirection == CarRollDirection.Forward ? true : false);
                    break;
                case CarSessionStateChangeArgs.Reason.UpdateTemprary:
                    mapControl1.AsynUpdateCar(arg.CarSessionArg.CarInstance.Id, arg.CarSessionArg.CarInstance.Name,
                        arg.CarSessionArg.X, arg.CarSessionArg.Y,
                        arg.CarSessionArg.RollDirection == CarRollDirection.Forward ? true : false);
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
    }
}
