using GeoGravityOverDose.ViewModels;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;

namespace GeoGravityOverDose.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {

        public static readonly DependencyProperty ViewModelProperty =
          DependencyProperty.Register(
              nameof(ViewModel),
              typeof(MainViewModel),
              typeof(MainWindow),
              new PropertyMetadata(null));

        public MainViewModel ViewModel
        {
            get => (MainViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;

            GlobalSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(500));

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                .DisposeWith(disposables);
            });
        }
    }
}