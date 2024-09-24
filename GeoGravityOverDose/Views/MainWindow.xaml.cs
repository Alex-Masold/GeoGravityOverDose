using MaterialDesignThemes.Wpf;
using System.Windows;

namespace GeoGravityOverDose.Views.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            GlobalSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(500));
        }
    }
}