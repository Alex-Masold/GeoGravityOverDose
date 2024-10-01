using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeoGravityOverDose.Views.Widget.AreaWidget
{
    /// <summary>
    /// Логика взаимодействия для AreaPresentation.xaml
    /// </summary>
    public partial class AreaPresentation : ReactiveUserControl<AreaPresentationViewModel>
    {
        public AreaPresentation()
        {
            InitializeComponent();
        }
    }
}
