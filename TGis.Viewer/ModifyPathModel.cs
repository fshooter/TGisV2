using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGis.Viewer.TGisRemote;

namespace TGis.Viewer
{
    class ModifyPathModel
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public ModifyPathModel(int pid)
        {
            GisPathInfo path;
            if (!GisGlobal.GPathMgr.TryGetPath(pid, out path))
                throw new ApplicationException("PathId Error");
            id = pid;
        }

    }
}
