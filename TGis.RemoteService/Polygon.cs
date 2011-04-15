using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TGis.RemoteService
{

    class Polygon
    {
        double[] pointArray;
        PointF[] pointArrayConvert;
        System.Drawing.Region myRegion;
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
            pointArrayConvert = new PointF[points.Length / 2];
            for (int i = 0; i < pointArrayConvert.Length; i++)
            {
                pointArrayConvert[i] = new PointF((float)pointArray[i * 2] * 1000,
                    (float)pointArray[i * 2 + 1] * 1000);
            }
            GraphicsPath myGraphicsPath = new GraphicsPath();
            myRegion = new System.Drawing.Region();
            myGraphicsPath.Reset();
            myGraphicsPath.AddPolygon(pointArrayConvert);//points);  

            myRegion.MakeEmpty();

            myRegion.Union(myGraphicsPath);     
        }
        public bool IsPointInRegion(double[] point)
        {
            if(point.Length != 2)
                throw new ArgumentOutOfRangeException();
            if (pointArray[0] == 0)
                return true;
            return myRegion.IsVisible(new PointF((float)point[0] * 1000,
                (float)point[1] * 1000));
        }
       
    }
}
