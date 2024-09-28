using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace Test
{
    public class SearchViewModel : ReactiveObject
    {
        // Строка поиска
        [Reactive] public string SearchQuery { get; set; }

        // Автопоиск
        [Reactive] public bool AutoSearch { get; set; } = true;

        // Коллекция для результатов поиска
        private readonly SourceList<string> _searchSource;
        private ReadOnlyObservableCollection<string> _searchResult;
        public ReadOnlyObservableCollection<string> SearchResult => _searchResult;

        private readonly List<string> _dataSource;

        // Команда поиска
        public ReactiveCommand<string, ICollection<string>> Search { get; }

        // Команда отмены поиска
        public ReactiveCommand<Unit, Unit> CancelSearch { get; }

        // Источник для отмены операций
        private CancellationTokenSource _cancellationToken;

        public SearchViewModel()
        {
            _cancellationToken = new CancellationTokenSource();

            _dataSource = new();

            for (int i = 0; i < 10; i++)
            {
                _dataSource.Add($"{i}{i+i}");
            }

            // Инициализация SourceList
            _searchSource = new SourceList<string>();

            _searchSource.AddRange(_dataSource);

            // Подключение и фильтрация через Dynamic Data
            _searchSource.Connect()
                .Bind(out _searchResult)   // Привязываем резу  льтат к коллекции
                .Subscribe();

            // Команда для отмены поиска
            CancelSearch = ReactiveCommand.Create(() =>
            {
                _cancellationToken.Cancel();
                _cancellationToken = new CancellationTokenSource();
            });

            // Команда поиска через CreateFromObservable
            Search = ReactiveCommand.CreateFromObservable<string, ICollection<string>>(query =>
                Observable.StartAsync(ct => SearchAsync(SearchQuery, ct)) // Запускаем асинхронную задачу
                    .TakeUntil(CancelSearch) // Прерываем выполнение, если вызывается отмена
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(result =>
                    {
                        _searchSource.Clear(); // Очищаем старые результаты
                        _searchSource.AddRange(result); // Добавляем новые результаты
                    }),
                this.WhenAnyValue(vm => vm.SearchQuery)
                    .Select(q => !string.IsNullOrWhiteSpace(q)) // Активируется только если есть текст в строке поиска
            );

            // Автопоиск
            this.WhenAnyValue(vm => vm.SearchQuery)
    .Where(_ => AutoSearch && !string.IsNullOrWhiteSpace(_))  // Автопоиск включен
    .Throttle(TimeSpan.FromMilliseconds(300), RxApp.MainThreadScheduler)  // Ждем 300 мс
    .Select(query => Search.Execute(query))  // Вызываем команду поиска
    .Switch()  // Используем Switch() для автоматической отмены предыдущего поиска
    .Subscribe();

            this.WhenAnyValue(vm => vm.SearchQuery)
                .Where(query => string.IsNullOrEmpty(query))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ =>
                {
                        _searchSource.Clear();
                    _searchSource.AddRange(_dataSource); // Возвращаем исходный список данных
                });
        }

        // Метод для создания фильтра для коллекции
        private Func<string, bool> BuildFilter(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return _ => true;

            return item => item.Contains(searchQuery, StringComparison.InvariantCultureIgnoreCase);
        }

        // Асинхронный метод поиска
        private async Task<ICollection<string>> SearchAsync(string query, CancellationToken token)
        {
            query = query.Trim();

            await Task.Delay(1000, token);

            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
                return _dataSource;

            // Реальная логика поиска по данным
            var result = _dataSource
                .Where(item => item.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return result;
        }
    }
}
