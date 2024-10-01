using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using GeoGravityOverDose.Views.Entity.CustomerEntity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace GeoGravityOverDose.Views.Widget.CustomerWidget
{
    public class CustomerPresentationViewModel : BaseViewModel
    {
        private readonly SourceList<Customer> _customersSource = new SourceList<Customer>();
        private ReadOnlyObservableCollection<Customer> _filteredCustomers;
        public ReadOnlyObservableCollection<Customer> Customers => _filteredCustomers;

        private readonly ICollection<Customer> _dataSource;

        private CancellationTokenSource _cancellationToken;

        [Reactive]
        public Customer SelectedCustomer { get; set; }

        [Reactive]
        public string SearchQuery { get; set; }

        [Reactive]
        public bool AutoSearch { get; set; } = true;

        public CustomerCardViewModel CustomerCardViewModel { get; set; }

        //Управление Customer
        public IReactiveCommand<Unit, Unit> AddCustomerCommand { get; }
        public IReactiveCommand<Customer, Unit> DeleteCustomerCommand { get; }

        //Поиск
        public IReactiveCommand<Unit, Unit> CancelSearch { get; }
        public IReactiveCommand<string, ICollection<Customer>> SearchCommand { get; }

        public CustomerPresentationViewModel()
        {
            CustomerCardViewModel = new CustomerCardViewModel();
            _cancellationToken = new CancellationTokenSource();

            _dataSource = new List<Customer>();

            for (int i = 0; i < 10; i++)
            {
                var customer = new Customer { FirstName = "new", LastName = "Customer", Family = $"{i}", Phone = "00000000" };

                for (int j = 0; j < 10; j++)
                {
                    var project = new Project { Name = $"project{i}" };
                    customer.Projects.Add(project);
                }
                _dataSource.Add(customer);
            }

            _customersSource.AddRange(_dataSource);

            _customersSource.Connect()
                    .Bind(out _filteredCustomers)
                    .Subscribe();

            this.WhenAnyValue(vm => vm.SelectedCustomer)
            .Subscribe(customer =>
            {
                CustomerCardViewModel.Customer = customer;
            });

            SelectedCustomer = Customers.FirstOrDefault();

            AddCustomerCommand = ReactiveCommand.Create(AddCustomer);
            DeleteCustomerCommand = ReactiveCommand.Create<Customer>(DeleteCustomer);

            CancelSearch = ReactiveCommand.Create(() =>
                       {
                           _cancellationToken.Cancel();
                           _cancellationToken = new CancellationTokenSource();
                       });

            SearchCommand = ReactiveCommand.CreateFromObservable<string, ICollection<Customer>>(query =>
                Observable
                .StartAsync(ct => SearchAsync(SearchQuery, ct)) // Запускаем асинхронную задачу
                    .TakeUntil(CancelSearch) // Прерываем выполнение, если вызывается отмена
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(result =>
                    {
                        _customersSource.Clear(); // Очищаем старые результаты
                        _customersSource.AddRange(result); // Добавляем новые результаты

                        SelectedCustomer = Customers.FirstOrDefault();
                    }),
                this.WhenAnyValue(vm => vm.SearchQuery)
                    .Select(q => !string.IsNullOrWhiteSpace(q)) // Активируется только если есть текст в строке поиска
            );

            this.WhenAnyValue(vm => vm.SearchQuery)
                .Where(_ => AutoSearch && !string.IsNullOrWhiteSpace(_))  // Автопоиск включен
                .Throttle(TimeSpan.FromMilliseconds(300), RxApp.MainThreadScheduler)  // Ждем 300 мс
                .Select(query => SearchCommand.Execute(query))  // Вызываем команду поиска
                .Switch()  // Используем Switch() для автоматической отмены предыдущего поиска
                .Subscribe();

            this.WhenAnyValue(vm => vm.SearchQuery)
                .Where(query => string.IsNullOrEmpty(query))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ =>
                {
                    _customersSource.Clear();
                    _customersSource.AddRange(_dataSource);

                    SelectedCustomer = Customers.FirstOrDefault();
                });
        }
        public void AddCustomer()
        {
            var customer = new Customer { FirstName = "new", LastName = "Customer", Family = "!", Phone = "00000000" };
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
        private async Task<ICollection<Customer>> SearchAsync(string query, CancellationToken token)
        {
            query = query.Trim();

            await Task.Delay(1000, token);

            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                return _dataSource;

            // Реальная логика поиска по данным
            var result = _dataSource
                .Where(item => item.FullName.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return result;
        }
    }
}
