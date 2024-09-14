using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Widget
{
    /// <summary>
    /// Логика взаимодействия для CustomerPresentation.xaml
    /// </summary>
    public partial class CustomerPresentation : UserControl, IViewFor<CustomerPresentationViewModel>
    {
        public CustomerPresentation()
        {
            InitializeComponent();
            ViewModel = new CustomerPresentationViewModel();

            this.WhenActivated(disposable =>
            {

                this.WhenAnyValue(
                    vm => vm.ViewModel.Customers)
                    .BindTo(this, vm => vm.ViewModel.Customers);

                this.Bind(this.ViewModel,
                    vm => vm.SelectedCustomer,
                    v => v.CustomersList.SelectedEntity)
                .DisposeWith(disposable);
            });

        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(CustomerPresentationViewModel), typeof(CustomerPresentation), null);

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
