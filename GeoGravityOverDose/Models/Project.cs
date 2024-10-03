using GeoGravityOverDose.Models.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Area> Areas { get; set; } = new ObservableCollection<Area>();

        public Project()
        {
            this.WhenAnyValue(
               fullNameData => fullNameData.Name,
               fullNameData => fullNameData.Address)
               .Select(t => $"{t.Item1}, {t.Item2}")
               .ToPropertyEx(this, x => x.FullName);
        }
    }
}
