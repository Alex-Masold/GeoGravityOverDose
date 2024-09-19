using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace GeoGravityOverDose.ViewModels
{
    public class CustomerCardViewModel : BaseViewModel
    {
        [Reactive]   
        public Customer Customer { get; set; }

        public CustomerCardViewModel()
        {
   
        }
    }
}
