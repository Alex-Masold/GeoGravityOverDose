using GeoGravityOverDose.Models;
using GeoGravityOverDose.Views.Widget.CustomerWidget;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Widget.CustomerWidget
{
    /// <summary>
    /// Логика взаимодействия для CustomerPresentation.xaml
    /// </summary>  
    public partial class CustomerPresentation : UserControl, IViewFor<CustomerPresentationViewModel>
    {
        public CustomerPresentation()
        {
            InitializeComponent();
            DataContext = ViewModel;

            this.WhenAnyObservable(view =>
                 view.ViewModel.SearchCommand.IsExecuting)
                    .BindTo(SearchExecutingProgressBar, pb => pb.IsIndeterminate);

            this.WhenAnyObservable(view =>
                 view.ViewModel.SearchCommand.IsExecuting)
                    .BindTo(SearchExecutingProgressBar, pb => pb.Visibility);

            this.WhenAnyObservable(view =>
                  view.ViewModel.SearchCommand.IsExecuting)
                 .Select(isExecuting => isExecuting ? Visibility.Collapsed : Visibility.Visible)
                 .BindTo(this, view => view.IconSearch.Visibility);

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SearchQuery, v => v.SearchBox.Text);

                this.OneWayBind(ViewModel, vm => vm.Customers, v => v.CustomersList.Entities).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedCustomer, v => v.CustomersList.SelectedEntity).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.AddCustomerCommand, v => v.CustomersList.AddEntityCommand).DisposeWith(disposables); ;
                this.OneWayBind(ViewModel, vm => vm.DeleteCustomerCommand, v => v.CustomersList.DeleteEntityCommand).DisposeWith(disposables); ;

                this.OneWayBind(ViewModel, vm => vm.CustomerCardViewModel, v => v.CustomerCard.DataContext).DisposeWith(disposables);

            });
        }   

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(CustomerPresentationViewModel),
                typeof(CustomerPresentation),
                new PropertyMetadata(null));

        public CustomerPresentationViewModel ViewModel
        {
            get => (CustomerPresentationViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CustomerPresentationViewModel)value;
        }
    }
}
