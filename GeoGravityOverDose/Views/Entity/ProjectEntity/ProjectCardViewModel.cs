using DynamicData;
using GeoGravityOverDose.Models;
using GeoGravityOverDose.Models.Base;
using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GeoGravityOverDose.Views.Entity.ProjectEntity
{
    public class ProjectCardViewModel : BaseViewModel
    {
        [Reactive]
        public Project Project { get; set; }

        [Reactive]
        public DrawingImage Image { get; set; }

        [Reactive]
        public Area SelectedArea { get; set; }

        public IReactiveCommand<Unit, Unit> RedrawCommand { get; }
        public IReactiveCommand<object, Unit> ZoomCommand { get; }

        public IReactiveCommand<Unit, Unit> AddAreaCommand { get; }
        public IReactiveCommand<Area, Unit> DeleteAreaCommand { get; }

        public int MaxLengthName { get; set; } = 50;
        public int MaxLengtAddress { get; set; } = 50;


        public void Redraw()
        {
            var vd = new VisDraw();

            foreach (var area in Project?.Areas ?? new ObservableCollection<Area>())
            {
                area.Draw(vd, area == SelectedArea ? Brushes.Yellow : (area.IsCorrect() ? Brushes.Green : Brushes.Red));
                foreach (var profile in area.Profiles ?? new())
                    profile.Draw(vd, area == SelectedArea ? Brushes.Yellow : (profile.IsCorrect() ? Brushes.Green : Brushes.Red));
            }
            Image = vd.Render();
        }
        private void Zoom(object obj)
        {
            var e = (MouseWheelEventArgs)obj;
            var image = (Image)e.Source;

            double delta = e.Delta > 0 ? 0.1 : -0.1;
            double scaleX = image.RenderTransform.Value.M11 + delta;
            double scaleY = image.RenderTransform.Value.M22 + delta;

            if (scaleX < 1 || scaleY < 1) return;

            image.RenderTransform = new ScaleTransform(scaleX, scaleY);
            var vp = e.MouseDevice.GetPosition(image);
            image.RenderTransformOrigin = new Point(vp.X/image.ActualWidth, vp.Y/image.ActualHeight);
        }

        public void AddArea()
        {
            var Area = new Area() { Name="New Area", Points = new(), Profiles = new() };
            Project.Areas.Add(Area);
            SelectedArea = Area;
        }
        private void DeleteArea(Area Area)
        {
            var _deletedAreaIndex = Project.Areas.IndexOf(Area);
            var _selectedAreaIndex = Project.Areas.IndexOf(SelectedArea);
            Project.Areas.Remove(Area);

            if (_deletedAreaIndex == _selectedAreaIndex)
            {
                if (_deletedAreaIndex == 0)
                { SelectedArea =  Project.Areas.FirstOrDefault(); }
                else if (_deletedAreaIndex ==  Project.Areas.Count)
                { SelectedArea =  Project.Areas.LastOrDefault(); }
                else if (_deletedAreaIndex > 0 && _deletedAreaIndex < Project.Areas.Count)
                { SelectedArea = Project.Areas[_deletedAreaIndex]; }
            }
        }

        public ProjectCardViewModel(Project project)
        {
            Project = project;

            ZoomCommand = ReactiveCommand.Create<object>(Zoom);

            RedrawCommand = ReactiveCommand.Create(Redraw);

            AddAreaCommand = ReactiveCommand.Create(AddArea);
            DeleteAreaCommand = ReactiveCommand.Create<Area>(DeleteArea);

            this.WhenAnyValue(x => x.SelectedArea)
                .Subscribe(_ => Redraw());

            this.WhenAnyValue(x => x.Project)
                .Where(x => x != null)
                .Subscribe(_ => Redraw());

            Redraw();
        }
    }
}
