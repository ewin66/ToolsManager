using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Atlante.Presentation.Shapes
{
    public class ArrowLine : Shape
    {
        private double arrowLength = 8;
        private double arrowWidth = 4;

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometryGroup = new GeometryGroup();

                geometryGroup.Children.Add(this.GetArrowGeometry());
                geometryGroup.Children.Add(new LineGeometry(new Point(X1, Y1), new Point(X2, Y2)));

                return geometryGroup;
            }
        }

        private PathGeometry GetArrowGeometry()
        {
            // Add arrow head
            var arrowPoint = new Point(X2, Y2);

            double dX = (X2 - X1);
            double dY = (Y2 - Y1);

            double length = Math.Sqrt(dX * dX + dY * dY);

            dX /= length;
            dY /= length;

            var myPathSegmentCollection = new PathSegmentCollection 
                    {
                        new LineSegment {Point = new Point(arrowPoint.X - dX * arrowLength + dY * arrowWidth/2.0, arrowPoint.Y - dY * arrowLength - dX * arrowWidth/2.0)},
                        new LineSegment {Point = new Point(arrowPoint.X - dX * arrowLength - dY * arrowWidth/2.0, arrowPoint.Y - dY * arrowLength + dX * arrowWidth/2.0)},
                        new LineSegment {Point = arrowPoint},
                    };

            var myPathFigure = new PathFigure { StartPoint = arrowPoint, Segments = myPathSegmentCollection };

            var myPathFigureCollection = new PathFigureCollection { myPathFigure };

            return new PathGeometry { Figures = myPathFigureCollection };
        }
    }
}
