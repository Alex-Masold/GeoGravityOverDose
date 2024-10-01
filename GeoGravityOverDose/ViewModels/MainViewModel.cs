using GeoGravityOverDose.ViewModels.Base;
using GeoGravityOverDose.Views.Pages;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace GeoGravityOverDose.ViewModels
{
    public class MainViewModel : BaseViewModel, IScreen
    {
        public RoutingState Router { get; }

        public IReactiveCommand<Unit, IRoutableViewModel> GoNext { get; }

        public IReactiveCommand<Unit, Unit> GoBack { get; }

        public MainViewModel()
        {
            Router= new RoutingState();

            Locator.CurrentMutable.Register(() => new AdminPage(), typeof(IViewFor<AdminViewModel>));

            Router.Navigate.Execute(new AdminViewModel(this));
        }
    }
}
