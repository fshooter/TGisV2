using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGis.RemoteService
{

    class Polygon
    {
        double[] pointArray;

        public double[] Points
        {
            get { return pointArray; }
        }     
        public Polygon(double[] points)
        {
            if ((points.Length < 6)
                || (points.Length % 2 != 0))
                throw new ArgumentOutOfRangeException();
            pointArray = points;
           
        }
        public bool IsPointInRegion(double[] point)
        {
            if(point.Length != 2)
                throw new ArgumentOutOfRangeException();
            if (pointArray[0] == 0)
                return true;
            return true;
        }
       
    }
}
