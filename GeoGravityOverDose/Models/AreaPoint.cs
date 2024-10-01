using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;
using System.Windows;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoGravityOverDose.Models
{
    public class AreaPoint : IdClass
    {
        public override string ToString()
        {
            return $"{X},{Y}";
        }

        [Reactive]
        public double X { get; set; }

        [Reactive]
        public double Y { get; set; }

        [Reactive]
        public Area Area { get; set; }
        
        [NotMapped]
        public Point P => new Point(X, Y);
    }
}
