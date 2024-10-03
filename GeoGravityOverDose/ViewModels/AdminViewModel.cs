using GeoGravityOverDose.ViewModels.Base;
using GeoGravityOverDose.Views.Pages;
using GeoGravityOverDose.Views.Widget.AreaWidget;
using GeoGravityOverDose.Views.Widget.CustomerWidget;
using GeoGravityOverDose.Views.Widget.ProjectWidget;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;

namespace GeoGravityOverDose.ViewModels
{
    public class AdminViewModel : BaseViewModel, IRoutableViewModel, IScreen
    {
        public string UrlPathSegment => "admin";
        public IScreen HostScreen { get; }

        public RoutingState Router { get; } = new RoutingState();

        public CustomerPresentationViewModel CustomerPresentationViewModel { get; }
        public AreaPresentationViewModel AreaPresentationViewModel { get; }
        public ProjectPresentationViewModel ProjectPresentationViewModel { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoCustomerPresentation { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoAreaPresentation { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> GoProjectPresentation { get; }
        

        // Вложенный Router для управления внутренними представлениями (например, Presentation)
        public AdminViewModel(IScreen screen)
        {

            HostScreen = screen;

            Router = new RoutingState();

            CustomerPresentationViewModel = new CustomerPresentationViewModel(this);
            AreaPresentationViewModel = new AreaPresentationViewModel(this);
            ProjectPresentationViewModel = new ProjectPresentationViewModel(this);

            Locator.CurrentMutable.Register(() => new CustomerPresentation(), typeof(IViewFor<CustomerPresentationViewModel>));
            Locator.CurrentMutable.Register(() => new AreaPresentation(), typeof(IViewFor<AreaPresentationViewModel>));
            Locator.CurrentMutable.Register(() => new ProjectPresentation(), typeof(IViewFor<ProjectPresentationViewModel>));

            GoCustomerPresentation = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(CustomerPresentationViewModel));
            GoAreaPresentation = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(AreaPresentationViewModel));
            GoProjectPresentation = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(ProjectPresentationViewModel));

        }
    }
}
