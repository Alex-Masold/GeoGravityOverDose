using DynamicData.Binding;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System.Windows;

namespace GeoGravityOverDose.ViewModels
{
    public class CustomerCardViewModel : BaseViewModel
    {
        [Reactive]   
        public Customer Customer { get; set; }

        private string _previousFirstName;
        private string _previousLastName;
        private string _previousFamily;

        public CustomerCardViewModel()
        {
            //_previousFirstName = Customer.FirstName;
            //_previousLastName = Customer.LastName;
            //_previousFamily = Customer.Family;

            this.WhenAnyValue(x => x.Customer.FirstName)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .DistinctUntilChanged()
                .Subscribe(newFirstName =>
                {
                    ShowSnackbar(_previousFirstName, newFirstName, "Имя");
                    _previousFirstName = newFirstName;  
                });
        }
        private void ShowSnackbar(string oldValue, string newValue, string fieldName)
        {
            // Пример использования Snackbar
            if (oldValue != null)
            {
                MessageBox.Show($"{fieldName} изменено: {oldValue} => {newValue}");
            }

            // Здесь вы можете вызвать Snackbar или другую систему уведомлений
            // Например, вызов Snackbar для WPF:
            // SnackbarMessageQueue.Enqueue($"{fieldName} изменено: {oldValue} => {newValue}");
        }
    }
}
