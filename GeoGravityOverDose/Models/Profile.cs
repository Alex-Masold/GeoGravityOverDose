using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace GeoGravityOverDose.Models
{
    public class Profile : IdClass
    {
        public List<(Picket pic, Point proj)> OrderPickets()
        {
            if (Points is null || Pickets is null || Points.Count < 2) return new();
            var temp = new List<(int idx, double dis, Point pr, Picket pi)>();
            foreach (var pik in Pickets)
            {
                int min = 0;
                double minVal = double.MaxValue;
                for (int i = 0; i < Points.Count-1; i++)
                {
                    double d = pik.DistanceToLine(Points[i].P, Points[i+1].P);
                    if (d < minVal) { min = i; minVal = d; }
                }
                var proj = pik.Projection(Points[min].P, Points[min + 1].P);
                temp.Add((min, Distance(Points[min].P, proj), proj, pik));
            }
            return temp.OrderBy(o => o.idx).OrderBy(o => o.dis).Select(t => (t.pi, t.pr)).ToList();
        }

        public bool IsCorrect()
        {
            for (int i = 0; i < Points?.Count - 1; i++)
                for (int j = 0; j < Area.Points.Count; j++)
                    if (AreCrossing(Points[i].P, Points[i + 1].P, Area.Points[j].P, Area.Points[(j + 1) % Area.Points.Count].P))
                        return false;
            foreach (var pr in Area.Profiles)
                for (int i = 0; i < pr.Points?.Count - 1; i++)
                    for (int j = 0; j < Points?.Count - 1; j++)
                        if (AreCrossing(pr.Points[i].P, pr.Points[i + 1].P, Points[j].P, Points[j + 1].P, colideSegments: pr==this ? Math.Abs(i-j)>1 : true))
                            return false;
            return true;
        }

        public bool AreCrossing(Point a1, Point a2, Point b1, Point b2, bool colideSegments = true)
        {
            double mult(double ax, double ay, double bx, double by) => ax * by - bx * ay;
            if ((mult(b2.X - b1.X, b2.Y - b1.Y, a1.X - b1.X, a1.Y - b1.Y) * mult(b2.X - b1.X, b2.Y - b1.Y, a2.X - b1.X, a2.Y - b1.Y)) < 0 &&
                (mult(a2.X - a1.X, a2.Y - a1.Y, b1.X - a1.X, b1.Y - a1.Y) * mult(a2.X - a1.X, a2.Y - a1.Y, b2.X - a1.X, b2.Y - a1.Y)) < 0) return true;
            if ((IsPointOnSegment(a1, b1, b2, colideSegments) || IsPointOnSegment(a2, b1, b2, colideSegments) ||
                 IsPointOnSegment(b1, a1, a2, colideSegments) || IsPointOnSegment(b2, a1, a2, colideSegments))) return true;
            return false;
        }

        static double Distance(Point point1, Point point2)
        {
            double dx = point2.X - point1.X;
            double dy = point2.Y - point1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        static bool IsPointOnSegment(Point p, Point l1, Point l2, bool colideSegments)
        {
            if (p.X == 5)
                p = p;
            if (!colideSegments && (p==l1 || p == l2)) return false;
            if (p.X >= Math.Min(l1.X, l2.X) && p.X <= Math.Max(l1.X, l2.X) &&
                p.Y >= Math.Min(l1.Y, l2.Y) && p.Y <= Math.Max(l1.Y, l2.Y))
            {
                //var v = Math.Abs((p.X - l1.X) * (l2.X - l1.X) - (l2.Y - l1.Y) * (p.Y - l1.Y));
                var v = Math.Abs((l1.X - p.X) * (l2.Y - p.Y) - (l1.Y - p.Y) * (l2.X - p.X));
                return !(v > 0.001);
            }
            return false;
        }
        public void Draw(VisDraw vd, Brush br)
        {
            if (Points is null) return;
            vd.DrawPoly(Points.Select(p => p.P).ToArray(), br, 0.4, false);
            foreach (var p in Points)
                vd.DrawText($"{p.X},{p.Y}", p.X, p.Y, Brushes.Black, 1.3);
        }

        [Reactive]
        public Area Area { get; set; }

        [Reactive]
        public Operator? Operator { get; set; }

        [Reactive]
        public ObservableCollection<Picket> Pickets { get; set; }

        [Reactive]
        public ObservableCollection<ProfilePoint> Points { get; set; }
    }

}
