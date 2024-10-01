using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace GeoGravityOverDose.Models.Base
{
    public class VisDraw
    {
        DrawingGroup drawingGroup = new();
        public void DrawLine(double x1, double y1, double x2, double y2, Brush brush, double width) =>
             drawingGroup.Children.Add(new GeometryDrawing(null, new Pen(brush, width) { DashCap = PenLineCap.Round }, new LineGeometry(new(x1, y1), new(x2, y2))));

        public void DrawText(string text, double x, double y, Brush brush, double size = 1) =>
            drawingGroup.Children.Add(new GeometryDrawing(Brushes.Black, null, new FormattedText(text, CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, new Typeface("Arial"), size, Brushes.Black, 1).BuildGeometry(new(x, y))));

        public void DrawCircle(double x, double y, double r, Brush brush) =>
            drawingGroup.Children.Add(new GeometryDrawing(brush, null, new EllipseGeometry(new(x, y), r, r)));

        public void DrawPoly(ICollection<Point> points, Brush brush, double width, bool isClosed)
        {
            if (points is null || points.Count < 1) return;
            drawingGroup.Children.Add(new GeometryDrawing(null, new Pen(brush, width) { LineJoin = PenLineJoin.Round, DashCap = PenLineCap.Round }, new PathGeometry(new PathFigure[] {
                new PathFigure(
                    points.First(),
                    points.Skip(1).Select(p => new LineSegment(p, true)), isClosed)
            })));
        }

        public DrawingImage Render(int offset = 3, int grid = 10, bool drawAxies = false)
        {
            double minx = (int)drawingGroup.Bounds.Left - offset, miny = (int)drawingGroup.Bounds.Top - offset, maxx = (int)drawingGroup.Bounds.Right - offset, maxy = (int)drawingGroup.Bounds.Bottom + offset;
            var chilndren = drawingGroup.Children.ToArray();
            drawingGroup.Children.Clear();

            for (double i = minx > 0 ? minx + grid - minx % grid : minx - minx % grid; i < maxx; i += grid)
            {
                DrawLine(i, miny, i, maxy, Brushes.Gray, 0.1);
                for (double j = miny > 0 ? miny + grid - miny % grid : miny - miny % grid; j < maxy; j += grid)
                {
                    DrawLine(minx, j, maxx, j, Brushes.Gray, 0.1);
                    DrawText($"{i},{j}", i, j, Brushes.Black, 1);
                }

            }
            if ((miny < 0 && maxy > 0) || drawAxies) DrawLine(minx, 0, maxx, 0, Brushes.DarkGray, 0.2);
            if ((minx < 0 && maxx > 0) || drawAxies) DrawLine(0, miny, 0, maxy, Brushes.DarkGray, 0.2);
            foreach (var p in chilndren) drawingGroup.Children.Add(p);
            return new DrawingImage(drawingGroup);
        }

    }
}
