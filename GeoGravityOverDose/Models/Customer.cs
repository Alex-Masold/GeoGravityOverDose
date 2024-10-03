using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace GeoGravityOverDose.Models
{
    public class Customer : User
    {
        [Reactive]
        public ObservableCollection<Project> Projects { get; set; } = new();
    }
}
