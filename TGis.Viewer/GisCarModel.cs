using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGis.Viewer
{
    class GisCarModel
    {
        private CarSessionMgr csm;

        public GisCarModel()
        {

        }

        public CarSessionMgr CsMgr
        {
            get { return csm; }
            set { csm = value; }
        }
    }
}
