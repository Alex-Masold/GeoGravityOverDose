using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using GeoGravityOverDose.Views.Entity.ProjectEntity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace GeoGravityOverDose.Views.Widget.ProjectWidget
{
    public class ProjectPresentationViewModel : BaseViewModel, IRoutableViewModel
    {
        public string UrlPathSegment { get; } = "ProjectPresentation";

        public IScreen HostScreen { get; }

        private readonly SourceList<Project> _projectSource = new SourceList<Project>();

        private ReadOnlyObservableCollection<Project> _filteredProject;

        public ReadOnlyObservableCollection<Project> Projects => _filteredProject;

        private readonly ICollection<Project> _dataSource;

        private CancellationTokenSource _cancellationToken;

        [Reactive]
        public Project SelectedProject { get; set; }

        [Reactive]
        public string SearchQuery { get; set; }

        [Reactive]
        public bool AutoSearch { get; set; } = true;

        public ProjectCardViewModel ProjectCardViewModel { get; set; }

        public IReactiveCommand<Unit, Unit> AddProjectCommand { get; }
        public IReactiveCommand<Project, Unit> DeleteProjectCommand { get; }

        public IReactiveCommand<Unit, Unit> CancelSearch { get; }
        public IReactiveCommand<string, ICollection<Project>> SearchCommand { get; }

        public ProjectPresentationViewModel(IScreen screen)
        {
            HostScreen = screen;

            _cancellationToken = new CancellationTokenSource();

            _dataSource = new List<Project>();

            for (int i = 0; i < 3; i++)
            {
                var newProject = new Project()
                {
                    Name = $"Project {i}",
                    Address = $"Address {i}",
                    Areas = new ObservableCollection<Area>()
                };

                for (int j = 0; i< 3; i++)
                {
                    var Area = new Area()
                    {
                        Name="Поле кукурузы",
                        Points = new()
                        {
                            new() { X=0, Y=i },
                            new() { X=2*j, Y=2 },
                            new() { X=3*i, Y=3*i },
                            new() { X=3, Y=3*j },
                        },
                        Project = newProject,
                        Profiles = new()

                    };

                    var Profile = new Profile(Area)
                    {
                        Operator=new() { FirstName="Илья", LastName="Буров" },
                        Points= new()
                        {
                            new () {X=i, Y=i*2 },
                            new () {X=i*2, Y=i * 2 },
                        }
                    };

                    Area.Profiles.Add(Profile);

                    newProject.Areas.Add(Area);
                }

                _dataSource.Add(newProject);
            }

            _projectSource.AddRange(_dataSource);

            _projectSource.Connect()
                .Bind(out _filteredProject)
                .Subscribe();

            SelectedProject = Projects.FirstOrDefault();

            ProjectCardViewModel = new(SelectedProject);

            this.WhenAnyValue(vm => vm.SelectedProject)
                .Subscribe(Project =>
                {
                    ProjectCardViewModel.Project = Project;
                });

            AddProjectCommand = ReactiveCommand.Create(AddProject);
            DeleteProjectCommand = ReactiveCommand.Create<Project>(DeleteProject);

            CancelSearch = ReactiveCommand.Create(() =>
            {
                _cancellationToken.Cancel();
                _cancellationToken = new CancellationTokenSource();
            });
            SearchCommand = ReactiveCommand.CreateFromObservable<string, ICollection<Project>>(query =>
                Observable
                .StartAsync(ct => SearchAsync(SearchQuery, ct)) // Запускаем асинхронную задачу
                    .TakeUntil(CancelSearch) // Прерываем выполнение, если вызывается отмена
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Do(result =>
                    {
                        _projectSource.Clear(); // Очищаем старые результаты
                        _projectSource.AddRange(result); // Добавляем новые результаты

                        SelectedProject = Projects.FirstOrDefault();
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
                    _projectSource.Clear();
                    _projectSource.AddRange(_dataSource);

                    SelectedProject = Projects.FirstOrDefault();
                });
        }

        public void AddProject()
        {
            var Project = new Project() { Name="New Project" };
            _projectSource.Add(Project);
            SelectedProject = Project;
        }
        private void DeleteProject(Project project)
        {
            var _deletedProjectIndex = Projects.IndexOf(project);
            var _selectedProjectIndex = Projects.IndexOf(SelectedProject);
            _projectSource.Remove(project);

            if (_deletedProjectIndex == _selectedProjectIndex)
            {
                if (_deletedProjectIndex == 0)
                { SelectedProject = Projects.FirstOrDefault(); }
                else if (_deletedProjectIndex == Projects.Count)
                { SelectedProject = Projects.LastOrDefault(); }
                else if (_deletedProjectIndex > 0 && _deletedProjectIndex < Projects.Count)
                { SelectedProject = Projects[_deletedProjectIndex]; }
            }
        }
        private async Task<ICollection<Project>> SearchAsync(string query, CancellationToken token)
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
