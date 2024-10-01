using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;
using System.Reactive.Linq;

namespace GeoGravityOverDose.Models
{
    public class AreaPoint : IdClass
    {
        [Reactive]
        public double X { get; set; }

        [Reactive]
        public double Y { get; set; }

        [Reactive]
        public Area Area { get; set; }
        
        [NotMapped]
        public Point P => new Point(X, Y);

        public AreaPoint()
        {
            this.WhenAnyValue(
                fullNameData => fullNameData.X,
                fullNameData => fullNameData.Y)
                .Select(t => $"X:{t.Item1} Y:{t.Item2}")
                .ToPropertyEx(this, x => x.FullName);
        }
    }
}
