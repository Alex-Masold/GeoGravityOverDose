using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Entity.CustomerModel
{
    /// <summary>
    /// Логика взаимодействия для CustomerCard.xaml
    /// </summary>
    public partial class CustomerCard : UserControl, IViewFor<CustomerCardViewModel>
    {
        public CustomerCard()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel), 
                typeof(CustomerCardViewModel), 
                typeof(CustomerCard), 
                null);

        public CustomerCardViewModel ViewModel
        {
            get => (CustomerCardViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);  
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CustomerCardViewModel)value;
        }
    }
}
