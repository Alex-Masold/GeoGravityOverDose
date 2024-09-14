using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI.Fody.Helpers;

namespace GeoGravityOverDose.ViewModels
{
    public class CustomerCardViewModel : BaseViewModel
    {
        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Phone { get; set; }


    }
}
