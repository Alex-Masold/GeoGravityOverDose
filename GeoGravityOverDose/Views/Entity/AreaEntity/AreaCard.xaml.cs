using GeoGravityOverDose.Views.Entity.AreaEntuty;
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

namespace GeoGravityOverDose.Views.Entity.AreaEntity
{
    /// <summary>
    /// Логика взаимодействия для AreaCard.xaml
    /// </summary>
    public partial class AreaCard : UserControl, IViewFor<AreaCardViewModel>
    {
        public AreaCard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ViewModelProperty =
           DependencyProperty.Register(
               nameof(ViewModel),
               typeof(AreaCardViewModel),
               typeof(AreaCard),
               new PropertyMetadata(null));

        public AreaCardViewModel ViewModel
        {
            get => (AreaCardViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AreaCardViewModel)value;
        }
    }
}
