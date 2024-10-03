using GeoGravityOverDose.Models.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Media;

namespace GeoGravityOverDose.Models
{
    public class Area : IdClass
    {
        [Reactive]
        public string? Name { get; set; }

        [Reactive]
        public Project Project { get; set; }

        [Reactive]
        public ObservableCollection<AreaPoint> Points { get; set; } = new();

        [Reactive]
        public ObservableCollection<Profile> Profiles { get; set; } = new();

        public Area()
        {

            this.WhenAnyValue(
                fullNameData => fullNameData.Name)
                .Select(t => $"{t}")
                .ToPropertyEx(this, x => x.FullName);
        }

        public void Draw(VisDraw gd, Brush br)
        {
            if (Points is null) return;
            gd.DrawPoly(Points.Select(p => p.P).ToArray(), br, 0.5, true);
            foreach (var p in Points)
                gd.DrawText($"{p.X},{p.Y}", p.X, p.Y, Brushes.Black, 1.5);
        }

        public bool IsCorrect()
        {
            for (int i = 0; i < Points?.Count; i++)
                for (int j = i + 1; j < Points.Count; j++)
                    if (AreCrossing(Points[i], Points[(i + 1) % Points.Count], Points[j], Points[(j + 1) % Points.Count]))
                        return false;
            return true;
        }

        public bool AreCrossing(AreaPoint p1, AreaPoint p2, AreaPoint p3, AreaPoint p4)
        {
            double mult(double ax, double ay, double bx, double by) => 
                ax * by - bx * ay;
            return ((mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y) * mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y)) < 0 &&
                    (mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y) * mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y)) < 0);
        }
    }
}
