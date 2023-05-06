using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;

namespace RGrafikaPZ1.Model
{
    public class Circle : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                double radius = Math.Min(ActualWidth, ActualHeight) / 2;
                Point center = new Point();
                center.X = (int)ActualWidth / 2;
                center.Y = (int)ActualHeight / 2;
                return new EllipseGeometry(center, radius, radius);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            System.Windows.Media.Pen pen = new System.Windows.Media.Pen(Stroke, StrokeThickness);
            drawingContext.DrawEllipse(Fill, pen, new Point(ActualWidth / 2, ActualHeight / 2), ActualWidth / 2, ActualHeight / 2);
        }
    }
}
