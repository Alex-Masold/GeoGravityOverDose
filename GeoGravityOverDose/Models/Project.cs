using GeoGravityOverDose.Models.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace GeoGravityOverDose.Models
{
    public class Project : IdClass
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string? Address { get; set; }

        [Reactive]
        public Customer Customer { get; set; }

        [Reactive]
        public ICollection<Area> Areas { get; set; }

        public Project()
        {
            this.WhenAnyValue(
               fullNameData => fullNameData.Name)
               .Select(t => $"{t}")
               .ToPropertyEx(this, x => x.FullName);
        }
    }
}
