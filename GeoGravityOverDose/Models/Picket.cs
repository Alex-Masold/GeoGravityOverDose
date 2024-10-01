using GeoGravityOverDose.Models.Base;
using System.Windows;

namespace GeoGravityOverDose.Models
{
    public class Picket : IdClass
    {
        int id;
        Profile profile;
        double x, y, ra, th, k;
        public Point Projection(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double t = ((x - p1.X) * dx + (y - p1.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));
            return new Point(p1.X + t * dx, p1.Y + t * dy);
        }

        public double DistanceToLine(Point p1, Point p2)
        {
            //double dx = p2.X - p1.X;
            //double dy = p2.Y - p1.Y;
            //var v = ((x - p1.X) * dx + (y - p1.Y) * dy) / (dx * dx + dy * dy);
            //v = Math.Max(0, Math.Min(1, Math.Abs(v)));
            //return Math.Sqrt(Math.Pow(p1.X - x + dx * v, 2) + Math.Pow(p1.Y - y + dx*v, 2));
            var p = Projection(p1, p2);
            return Math.Sqrt(Math.Pow(p.X-x, 2) + Math.Pow(p.Y - y, 2));
        }

        public Profile Profile { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public double Ra { get; set; }
        public double Th { get; set; }
        public double K { get; set; }

        public Picket(Profile profile)
        {
            Profile=profile;
        }
    }
}
