using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGis.Viewer
{
    class NavigateException : ApplicationException 
    {
        public NavigateException(string msg) : base(msg)
        {
           
        }
    }
    interface INavigateTarget
    {
        void Activate(Form parent, IDictionary<string, object> parameters);
        void Deactivate();
    }
    enum NavigatePath
    {
        Welcome,
        GisImmediate,
        PathEdit,
        CarEdit,
    }
    class NavigateNode
    {
        private NavigatePath path;
        private Form parentForm;
        private IDictionary<string, object> parameters;
        public NavigatePath Path
        {
            get { return path; }
        }
        public Form ParentForm
        {
            get { return parentForm; }
        }
        public IDictionary<string, object> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        public NavigateNode(NavigatePath _path, Form _parentForm, IDictionary<string, object> _parameters)
        {
            path = _path;
            parentForm = _parentForm;
            parameters = _parameters;
        }
    }
    public delegate void OnNavigate();
    class NavigateMgr
    {
        static NavigateMgr instance = null;
        const int MaxHistroyNum = 10;
        public static NavigateMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new NavigateMgr();
                return instance;
            }
        }
        private IDictionary<NavigatePath, INavigateTarget> naviDict;
        private IList<NavigateNode> naviHistoryPre;
        private IList<NavigateNode> naviHistoryAft;
        private INavigateTarget currentTarget;

        internal INavigateTarget CurrentTarget
        {
            get { return currentTarget; }
        }
        private NavigateNode currentNode;
        public event OnNavigate OnNaviChanged;
        private NavigateMgr()
        {
            naviDict = new Dictionary<NavigatePath, INavigateTarget>();
            naviHistoryPre = new List<NavigateNode>();
            naviHistoryAft = new List<NavigateNode>();
            currentTarget = null;
            
            naviDict[NavigatePath.Welcome] = new NtWelcome();
            naviDict[NavigatePath.GisImmediate] = new NGis();
            naviDict[NavigatePath.PathEdit] = new NDrawPath();
            naviDict[NavigatePath.CarEdit] = new NCarEdit();
        }

        public void NavigateTo(NavigatePath np, Form parentForm, IDictionary<string, object> parameters = null, bool bSetHistory = true)
        {
            INavigateTarget target;
            if(!naviDict.TryGetValue(np, out target))
                throw new NavigateException("error navigate path");
            if (currentTarget != null)
            {
                currentTarget.Deactivate();
                currentTarget = null;
            }
            if (bSetHistory && (currentNode != null))
            {
                naviHistoryPre.Add(currentNode);
                while (naviHistoryPre.Count > MaxHistroyNum)
                    naviHistoryPre.RemoveAt(0);
            }
            currentNode = null;

            target.Activate(parentForm, parameters);
            currentTarget = target;
            currentNode = new NavigateNode(np, parentForm, parameters);
            if (bSetHistory)
            {
                naviHistoryAft.Clear();
            }
            if(OnNaviChanged != null)
                OnNaviChanged();
        }
        public void NavigatePrevious()
        {
            if(naviHistoryPre.Count == 0)
                throw new NavigateException("no pre navigate path");
            NavigateNode targetNode = naviHistoryPre.ElementAt(naviHistoryPre.Count - 1);
            naviHistoryAft.Add(currentNode);
            naviHistoryPre.RemoveAt(naviHistoryPre.Count - 1);
            NavigateTo(targetNode.Path, targetNode.ParentForm, targetNode.Parameters, false);
        }
        public void NavigateNext()
        {
            if (naviHistoryAft.Count == 0)
                throw new NavigateException("no next navigate path");
            NavigateNode targetNode = naviHistoryAft.ElementAt(0);
            naviHistoryPre.Add(currentNode);
            naviHistoryAft.RemoveAt(0);
            NavigateTo(targetNode.Path, targetNode.ParentForm, targetNode.Parameters, false);
        }
        public bool CanNaviPre
        {
            get { return naviHistoryPre.Count > 0; }
        }
        public bool CanNaviNext
        {
            get { return naviHistoryAft.Count > 0; }
        }
    }
}

