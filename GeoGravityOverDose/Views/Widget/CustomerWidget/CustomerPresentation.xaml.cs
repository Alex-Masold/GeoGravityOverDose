using GeoGravityOverDose.Views.Widget.CustomerWidget;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Widget.CustomerWidget.Model
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
