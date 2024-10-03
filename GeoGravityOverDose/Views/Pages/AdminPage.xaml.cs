using GeoGravityOverDose.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : UserControl, IViewFor<AdminViewModel>
    {
        public AdminPage()
        {
            InitializeComponent();

            DataContext = ViewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Router, v => v.RoutedViewHost.Router)
                .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoCustomerPresentation, v => v.CustomersNav)
                .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoProjectPresentation, v => v.ProgectsNav)
                    .DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.GoAreaPresentation, v => v.AreasNav)
                    .DisposeWith(disposables);
            });
        }

        public static readonly DependencyProperty ViewModelProperty =
           DependencyProperty.Register(
               nameof(ViewModel),
               typeof(AdminViewModel),
               typeof(AdminPage),
               new PropertyMetadata(null));

        public AdminViewModel ViewModel
        {
            get => (AdminViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AdminViewModel)value;
        }
    }
}
