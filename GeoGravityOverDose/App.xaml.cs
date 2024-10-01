using GeoGravityOverDose.ViewModels;
using GeoGravityOverDose.Views.Pages;
using GeoGravityOverDose.Views.Widget.CustomerWidget;
using System.Windows;

namespace GeoGravityOverDose
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow().Show();
        }
    }

}
