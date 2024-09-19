using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;

namespace GeoGravityOverDose.Models
{
    public class Customer : User
    {
        [Reactive]
        public ICollection<Project> Projects { get; set; } = [];
    }
}
