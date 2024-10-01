using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Widget.AreaWidget
{
    /// <summary>
    /// Логика взаимодействия для AreaPresentation.xaml
    /// </summary>
    public partial class AreaPresentation : UserControl, IViewFor<AreaPresentationViewModel>
    {
        public AreaPresentation()
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

                this.OneWayBind(ViewModel, vm => vm.Areas, v => v.AreaList.Entities).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedArea, v => v.AreaList.SelectedEntity).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.AddAreaCommand, v => v.AreaList.AddEntityCommand).DisposeWith(disposables); ;
                this.OneWayBind(ViewModel, vm => vm.DeleteAreaCommand, v => v.AreaList.DeleteEntityCommand).DisposeWith(disposables); ;

                this.OneWayBind(ViewModel, vm => vm.AreaCardViewModel, v => v.AreaCard.DataContext).DisposeWith(disposables);

            });

        }

        public static readonly DependencyProperty ViewModelProperty =
           DependencyProperty.Register(
               nameof(ViewModel),
               typeof(AreaPresentationViewModel),
               typeof(AreaPresentation),
               new PropertyMetadata(null));

        public AreaPresentationViewModel ViewModel
        {
            get => (AreaPresentationViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AreaPresentationViewModel)value;
        }
    }
}
