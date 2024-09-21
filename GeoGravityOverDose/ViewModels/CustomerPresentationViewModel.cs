using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace GeoGravityOverDose.ViewModels
{
    public class CustomerPresentationViewModel : BaseViewModel
    {
        private readonly SourceList<Customer> _customersSource = new SourceList<Customer>();
        private readonly Subject<Customer> _customerUpdatedSubject = new Subject<Customer>();

        private string _previousFirstName;
        private string _previousLastName;
        private string _previousFamily;

        public CustomerPresentationViewModel()
        {
            _customersSource.Connect()
                    .Bind(out ReadOnlyObservableCollection<Customer> customers)
                    .Subscribe();


            Customers = customers;

            for (int i = 0; i < 10; i++)
            {
                var customer = new Customer { FirstName = "new", LastName = "Customer", Family = $"{i}", Phone="00000000" };

                for (int j = 0; j < 10; j++)
                {
                    var project = new Project { Name = $"project{i}" };
                    customer.Projects.Add(project);
                }

                _customersSource.Add(customer);

            }

            SelectedCustomer = customers.FirstOrDefault();

            _previousFirstName = SelectedCustomer.FirstName;
            _previousLastName = SelectedCustomer.LastName;
            _previousFamily = SelectedCustomer.Family;

            AddCustomerCommand = ReactiveCommand.Create(AddCustomer);
            DeleteCustomerCommand = ReactiveCommand.Create<Customer>(DeleteCustomer);

        //    this.WhenAnyValue(x => x.SelectedCustomer)
        //.Subscribe(customer =>
        //{
        //    if (customer != null)
        //    {
        //        _previousFirstName = customer.FirstName;
        //        _previousLastName = customer.LastName;
        //        _previousFamily = customer.Family;
        //    }
        //});

        //    this.WhenAnyValue(x => x.SelectedCustomer.FirstName)
        //        .Throttle(TimeSpan.FromMilliseconds(500))
        //        .DistinctUntilChanged()
        //        .Subscribe(newFirstName =>
        //        {
        //            if (SelectedCustomer != null)
        //            {
        //                ShowSnackbar(_previousFirstName, newFirstName, "Имя");
        //                _previousFirstName = newFirstName;
        //            }
        //        });
        }

        public ReadOnlyObservableCollection<Customer> Customers { get; }

        [Reactive]
        public Customer SelectedCustomer { get; set; }
        [Reactive]
        //public CustomerCardViewModel CustomerCardViewModel { get; set; }

        public ReactiveCommand<Unit, Unit> AddCustomerCommand { get; }
        public ReactiveCommand<Customer, Unit> DeleteCustomerCommand { get; }

        public void AddCustomer()
        {
            var customer = new Customer { FirstName = "new", LastName = "Customer", Family = "!", Phone="00000000" }; ;
            _customersSource.Add(customer);
        }

        private void DeleteCustomer(Customer customer)
        {
            _customersSource.Remove(customer);
        }

        public void NotifyCustomerUpdated(Customer customer)
        {
            _customerUpdatedSubject.OnNext(customer);
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
