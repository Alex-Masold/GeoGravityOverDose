using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using GeoGravityOverDose.Views.Entity.CustomerModel;
using MaterialDesignColors;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace GeoGravityOverDose.ViewModels
{
    public class CustomerPresentationViewModel : BaseViewModel
    {
        private readonly Subject<Customer> _customerUpdatedSubject = new Subject<Customer>();
        private readonly SourceList<Customer> _customersSource = new SourceList<Customer>();
        private ReadOnlyObservableCollection<Customer> _filteredCustomers;

        private CancellationTokenSource _cancellationToken;
        public ReadOnlyObservableCollection<Customer> Customers => _filteredCustomers;

        [Reactive]
        public Customer SelectedCustomer { get; set; }

        [Reactive]
        public string SearchQuery { get; set; }

        [Reactive]
        public bool AutoSearch { get; set; } = true;

        public CustomerCardViewModel CustomerCardViewModel { get; set; }

        public IReactiveCommand<Unit, Unit> AddCustomerCommand { get; }
        public IReactiveCommand<Customer, Unit> DeleteCustomerCommand { get; }
        public IReactiveCommand<Unit, Unit> CancelSearch { get; }
        public IReactiveCommand<string, Unit> SearchCommand { get; }
        public CustomerPresentationViewModel()
        {
            CustomerCardViewModel = new CustomerCardViewModel();
            _cancellationToken = new CancellationTokenSource();

            _customersSource.Connect()
                    .Bind(out _filteredCustomers)
                    .Subscribe();

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

            this.WhenAnyValue(vm => vm.SelectedCustomer)
            .Subscribe(customer =>
            {
                CustomerCardViewModel.Customer = customer;
            });

            SelectedCustomer = Customers.FirstOrDefault();

            AddCustomerCommand = ReactiveCommand.Create(AddCustomer);
            DeleteCustomerCommand = ReactiveCommand.Create<Customer>(DeleteCustomer);

            SearchCommand = ReactiveCommand.CreateFromObservable<string, Unit>(
                query => Observable.StartAsync(cancelToken => SearchAsync(query, cancelToken)).TakeUntil(CancelSearch),
                this.WhenAnyValue(vm => vm.SearchQuery)
                    .Select(query => !string.IsNullOrWhiteSpace(query))
            );

            CancelSearch = ReactiveCommand.Create(
                () => 
                {
                    _cancellationToken.Cancel(); // Отменяем поиск
                    _cancellationToken = new CancellationTokenSource(); // Обновляем токен для следующего поиска
                }, 
                SearchCommand.IsExecuting);


            this.WhenAnyValue(vm => vm.SearchQuery)
            .Where(_ => AutoSearch)
            .Throttle(TimeSpan.FromSeconds(0.3), RxApp.MainThreadScheduler)
            .InvokeCommand((ReactiveCommandBase<string, Unit>?)SearchCommand);
        }
        public void AddCustomer()
        {
            var customer = new Customer { FirstName = "new", LastName = "Customer", Family = "!", Phone="00000000" }; ;
            _customersSource.Add(customer);
            SelectedCustomer = customer;
        }

        private void DeleteCustomer(Customer customer)
        {
            var _deletedCustomerIndex = Customers.IndexOf(customer);
            var _selectedCustomerIndex = Customers.IndexOf(SelectedCustomer);
            _customersSource.Remove(customer);

            if (_deletedCustomerIndex == _selectedCustomerIndex)
            {
                if (_deletedCustomerIndex == 0)
                { SelectedCustomer = Customers.FirstOrDefault(); }
                else if (_deletedCustomerIndex == Customers.Count)
                { SelectedCustomer = Customers.LastOrDefault(); }
                else if (_deletedCustomerIndex > 0 && _deletedCustomerIndex < Customers.Count)
                { SelectedCustomer = Customers[_deletedCustomerIndex]; }
            }
        }
        private async Task SearchAsync(string query, CancellationToken token)
        {
            await Task.Delay(1500, token);

            query = query.ToLower();

            _customersSource
                 .Connect()
                 .Filter(_customer => string.IsNullOrWhiteSpace(query) || _customer.FullName.ToLower().Contains(query))
                 .Bind(out _filteredCustomers)
                 .Subscribe();

        }
        public void NotifyCustomerUpdated(Customer customer)
        {
            _customerUpdatedSubject.OnNext(customer);
        }
    }
}
