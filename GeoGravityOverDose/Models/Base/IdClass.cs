using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GeoGravityOverDose.Models.Base
{
    public class IdClass : ReactiveObject
    {
        [ObservableAsProperty]
        public string FullName { get; }
        public Guid Id { get; set; }

    }
}
