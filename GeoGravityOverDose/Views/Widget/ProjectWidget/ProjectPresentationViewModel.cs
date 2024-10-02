using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.ViewModels.Base;
using GeoGravityOverDose.Views.Entity.AreaEntuty;
using GeoGravityOverDose.Views.Entity.ProfileEntity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace GeoGravityOverDose.Views.Widget.ProjectWidget
{
    public class ProjectPresentationViewModel: BaseViewModel, IRoutableViewModel
    {
        public string UrlPathSegment { get; } = "ProjectPresentation";

        public IScreen HostScreen { get; }

        private readonly SourceList<Project> _projectSource = new SourceList<Project>();

        private ReadOnlyObservableCollection<Project> _filteredProject;

        public ReadOnlyObservableCollection<Project> Projects => _filteredProject;
            
        private readonly ICollection<Project> _dataSource;

        private CancellationTokenSource _cancellationTokenSource;

        [Reactive]
        public Profile SelectedProfile { get; set; }

        [Reactive]
        public string SearchQuery { get; set; }

        [Reactive]
        public bool AutoSearch { get; set; } = true;

        public ProfileCardViewModel ProjectCardViewModel { get; set; }

        public IReactiveCommand<Unit, Unit> AddProjectCommand { get; }
        public IReactiveCommand<Project, Unit> DeleteProjectCommand { get; }

        public IReactiveCommand<Unit, Unit> CancelSearch { get; }
        public IReactiveCommand<string, ICollection<Project>> SearchCommand { get; }

        public ProjectPresentationViewModel(IScreen screen)
        {
            HostScreen = screen;

            _cancellationTokenSource = new CancellationTokenSource();

            _dataSource = new List<Project>();

            for (int i = 0; i < 3 ; i++)
            {

            }
        }
    }
}
