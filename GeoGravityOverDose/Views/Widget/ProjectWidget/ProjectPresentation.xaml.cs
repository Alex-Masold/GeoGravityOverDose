using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Widget.ProjectWidget
{
    /// <summary>
    /// Логика взаимодействия для ProjectPresentation.xaml
    /// </summary>
    public partial class ProjectPresentation : UserControl, IViewFor<ProjectPresentationViewModel>
    {
        public ProjectPresentation()
        {
            InitializeComponent();
            DataContext = ViewModel;


            this.WhenAnyObservable(view =>
                 view.ViewModel.SearchCommand.IsExecuting)
                    .BindTo(SearchExecutingProgressBar, pb => pb.IsIndeterminate);

            this.WhenAnyObservable(view =>
                 view.ViewModel.SearchCommand.IsExecuting)
                    .BindTo(SearchExecutingProgressBar, pb => pb.Visibility);

            this.WhenAnyObservable(view =>
                  view.ViewModel.SearchCommand.IsExecuting)
                 .Select(isExecuting => isExecuting ? Visibility.Collapsed : Visibility.Visible)
                 .BindTo(this, view => view.IconSearch.Visibility);

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SearchQuery, v => v.SearchBox.Text);

                this.OneWayBind(ViewModel, vm => vm.Projects, v => v.ProjectList.Entities).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedProject, v => v.ProjectList.SelectedEntity).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.AddProjectCommand, v => v.ProjectList.AddEntityCommand).DisposeWith(disposables); ;
                this.OneWayBind(ViewModel, vm => vm.DeleteProjectCommand, v => v.ProjectList.DeleteEntityCommand).DisposeWith(disposables); ;

                this.OneWayBind(ViewModel, vm => vm.ProjectCardViewModel, v => v.ProjectCard.DataContext).DisposeWith(disposables);

            });

        }

        public static readonly DependencyProperty ViewModelProperty =
           DependencyProperty.Register(
               nameof(ViewModel),
               typeof(ProjectPresentationViewModel),
               typeof(ProjectPresentation),
               new PropertyMetadata(null));

        public ProjectPresentationViewModel ViewModel
        {
            get => (ProjectPresentationViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ProjectPresentationViewModel)value;
        }
    }
}
