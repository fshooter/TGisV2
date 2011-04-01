using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TGis.Viewer
{

    class Polygon
    {
        const int POS_FACTOR = 100000000;
        GraphicsPath myGraphicsPath = new GraphicsPath();
        Region myRegion = new Region(); 
        double[] pointArray;     
        public Polygon(double[] points)
        {
            if ((points.Length < 6)
                || (points.Length % 2 != 0))
                throw new ArgumentOutOfRangeException();
            pointArray = points;
            PointF[] newPoints = new PointF[points.Length / 2];
            for (int i = 0; i < points.Length / 2; ++i)
            {
                newPoints[i] = new PointF((float)points[i * 2], (float)points[i * 2 + 1]);
            }
            myGraphicsPath.AddPolygon(newPoints);
            myRegion.MakeEmpty();
            myRegion.Union(myGraphicsPath);  
        }
        bool IsPointInRegion(double[] point)
        {
            if(point.Length != 2)
                throw new ArgumentOutOfRangeException();
            PointF ip = new PointF((float)point[0], (float)point[1]);
            return myRegion.IsVisible(ip);
        }
        public double[] PointArray
        {
          get { return pointArray; }
        }  
    }
}
