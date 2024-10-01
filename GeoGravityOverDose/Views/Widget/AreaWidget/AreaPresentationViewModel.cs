using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.Views.Entity.AreaEntity;
using ReactiveUI.Fody.Helpers;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using GeoGravityOverDose.ViewModels.Base;
using System.Reactive.Linq;
using GeoGravityOverDose.Views.Entity.AreaEntuty;

namespace GeoGravityOverDose.Views.Widget.AreaWidget
{
    public class AreaPresentationViewModel : BaseViewModel
    {
        private readonly SourceList<Area> _AreasSource = new SourceList<Area>();
        private ReadOnlyObservableCollection<Area> _filteredAreas;
        public ReadOnlyObservableCollection<Area> Areas => _filteredAreas;

        private readonly ICollection<Area> _dataSource;

        private CancellationTokenSource _cancellationToken;

        [Reactive]
        public Area SelectedArea { get; set; }

        [Reactive]
        public string SearchQuery { get; set; }

        [Reactive]
        public bool AutoSearch { get; set; } = true;

        public AreaCardViewModel AreaCardViewModel { get; set; }

        //Управление Area
        public IReactiveCommand<Unit, Unit> AddAreaCommand { get; }
        public IReactiveCommand<Area, Unit> DeleteAreaCommand { get; }

        //Поиск
        public IReactiveCommand<Unit, Unit> CancelSearch { get; }
        public IReactiveCommand<string, ICollection<Area>> SearchCommand { get; }

        public AreaPresentationViewModel()
        {
            AreaCardViewModel = new AreaCardViewModel();
            _cancellationToken = new CancellationTokenSource();

            _dataSource = new List<Area>();

            for (int i = 0; i < 10; i++)
            {
                var Area = new Area()
                {
                    Name="Поле кукурузы",
                    Points = new()
                        {
                            new() { X=0, Y=0 },
                            new() { X=i/2, Y=2 },
                            new() { X=i/2 * 3, Y= i/2 * 5 },
                            new() { X=i/3, Y=i/3 * 3 },
                        },
                    Profiles=new() {
                                new()
                                {
                                    Operator=new() { FirstName="Илья", LastName="Буров" },
                                    Points= new()
                                    {
                                        new () {X=2, Y=2 },
                                        new () {X=8, Y=4 },
                                        new () {X=17, Y=4 },
                                        new () {X=25, Y=6 },
                                        new () {X=32, Y=7 },
                                    }
                                },
                                new()
                                {

                                    Operator=new() { FirstName="Мамаро", LastName="Воруб" },
                                    Points= new()
                                    {
                                        new () {X=4, Y=20 },
                                        new () {X=7, Y=15 },
                                        new () {X=11, Y=17 },
                                        new () {X=17, Y=22 },
                                        new () {X=23, Y=18 },
                                    }
                                }
                    }  
                };

                for (int j = 0; j < 5; j++)
                {
                }
                _dataSource.Add(Area);
            }

            _AreasSource.AddRange(_dataSource);

            _AreasSource.Connect()
                    .Bind(out _filteredAreas)
                    .Subscribe();

            this.WhenAnyValue(vm => vm.SelectedArea)
            .Subscribe(Area =>
            {
                AreaCardViewModel.Area = Area;
            });

            SelectedArea = Areas.FirstOrDefault();

            AddAreaCommand = ReactiveCommand.Create(AddArea);
            DeleteAreaCommand = ReactiveCommand.Create<Area>(DeleteArea);

            CancelSearch = ReactiveCommand.Create(() =>
            {
                _cancellationToken.Cancel();
                _cancellationToken = new CancellationTokenSource();
            });

            SearchCommand = ReactiveCommand.CreateFromObservable<string, ICollection<Area>>(query =>
                Observable
                .StartAsync(ct => SearchAsync(SearchQuery, ct)) // Запускаем асинхронную задачу
                    .TakeUntil(CancelSearch) // Прерываем выполнение, если вызывается отмена
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(result =>
                    {
                        _AreasSource.Clear(); // Очищаем старые результаты
                        _AreasSource.AddRange(result); // Добавляем новые результаты

                        SelectedArea = Areas.FirstOrDefault();
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
                    _AreasSource.Clear();
                    _AreasSource.AddRange(_dataSource);

                    SelectedArea = Areas.FirstOrDefault();
                });
        }
        public void AddArea()
        {
            var Area = new Area() { Name="New Area"};
            _AreasSource.Add(Area);
            SelectedArea = Area;
        }

        private void DeleteArea(Area Area)
        {
            var _deletedAreaIndex = Areas.IndexOf(Area);
            var _selectedAreaIndex = Areas.IndexOf(SelectedArea);
            _AreasSource.Remove(Area);

            if (_deletedAreaIndex == _selectedAreaIndex)
            {
                if (_deletedAreaIndex == 0)
                { SelectedArea = Areas.FirstOrDefault(); }
                else if (_deletedAreaIndex == Areas.Count)
                { SelectedArea = Areas.LastOrDefault(); }
                else if (_deletedAreaIndex > 0 && _deletedAreaIndex < Areas.Count)
                { SelectedArea = Areas[_deletedAreaIndex]; }
            }
        }
        private async Task<ICollection<Area>> SearchAsync(string query, CancellationToken token)
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
