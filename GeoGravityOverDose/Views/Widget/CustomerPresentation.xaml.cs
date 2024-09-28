using GeoGravityOverDose.ViewModels;
using ReactiveUI;
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
            DataContext = ViewModel;
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

        private void PackIconKind_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
