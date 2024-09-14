using GeoGravityOverDose.ViewModels;
using GeoGravityOverDose.Views.Pages;
using System.Windows;

namespace GeoGravityOverDose
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainViewModel mainVM = new MainViewModel() { CurrentVM = new CustomerPresentationViewModel() };

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow() { DataContext = mainVM }.Show();
        }
    }

}
