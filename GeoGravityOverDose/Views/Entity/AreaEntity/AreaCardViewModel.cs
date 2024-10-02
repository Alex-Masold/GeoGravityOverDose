using GeoGravityOverDose.Models;
using GeoGravityOverDose.Models.Base;
using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace GeoGravityOverDose.Views.Entity.AreaEntuty
{
    public class AreaCardViewModel : BaseViewModel
    {
        [Reactive]
        public Area Area { get; set; }

        [Reactive]
        public DrawingImage Image { get; set; }

        [Reactive]
        public AreaPoint SelectedPoint { get; set; }

        [Reactive]
        public Profile SelectedProfile { get; set; }

        public IReactiveCommand<Unit, Unit> RedrawCommand { get; }

        public IReactiveCommand<Unit, Unit> AddPointCommand { get; }
        public IReactiveCommand<Unit, Unit> GeneratePointCommand { get; }
        public IReactiveCommand<AreaPoint, Unit> DeletePointCommand { get; }

        public IReactiveCommand<Unit, Unit> AddProfileCommand { get; }
        public IReactiveCommand<Profile, Unit> DeleteProfileCommand { get; }

        public IReactiveCommand<object, Unit> ZoomCommand { get; }

        public int MaxLengthName { get; set; } = 50;

        public void Redraw()
        {
            var vd = new VisDraw();

            // Рисуем область
            Area.Draw(vd, Area.IsCorrect() ? Brushes.Green : Brushes.Red);

            // Рисуем точки
            foreach (var p in Area.Points ?? new())
                vd.DrawCircle(p.X, p.Y, 0.6, SelectedPoint == p ? Brushes.Yellow : Brushes.Green);

            // Рисуем профили
            foreach (var p in Area.Profiles ?? new())
                p.Draw(vd, p == SelectedProfile ? (p.IsCorrect() ? Brushes.Yellow : Brushes.Orange)
                                                : (p.IsCorrect() ? Brushes.Green : Brushes.Red));

            // Обновляем изображение
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
        private void AddPoint()
        {
            var newPoint = new AreaPoint() { X=0, Y=0, Area=Area };
            Area.Points.Add(newPoint);
            SelectedPoint = newPoint;
            Redraw();
        }
        private void GeneratePoint()
        {

            AreaPoint previosPoint = Area.Points?.Count > 0 ? Area.Points.Last() : new() { X = 0, Y = 0 }, point;
            int off = 25;
            Random r = new();
            while (true)
            {
                point = new AreaPoint() { X = previosPoint.X + r.Next(-off, off), Y = previosPoint.Y + r.Next(-off, off), Area = Area };
                Area.Points.Add(point);
                if (Area.IsCorrect()) break;
                else Area.Points.Remove(point);
            }
            SelectedPoint = point;
            Redraw();


        }
        private void DeletePoint(AreaPoint point)
        {
            var _deletedPointIndex = Area.Points.IndexOf(point);
            var _SelectedPointIndex = Area.Points.IndexOf(SelectedPoint);
            Area.Points.Remove(point);
            Redraw();

            if (_deletedPointIndex == _SelectedPointIndex)
            {
                if (_deletedPointIndex == 0)
                { SelectedPoint = Area.Points.FirstOrDefault(); }
                else if (_deletedPointIndex == Area.Points.Count)
                { SelectedPoint = Area.Points.LastOrDefault(); }
                else if (_deletedPointIndex > 0 && _deletedPointIndex < Area.Points.Count)
                { SelectedPoint = Area.Points[_deletedPointIndex]; }
            }
        }

        private void AddProfile()
        {
            var newProfile = new Profile(Area);
            Area.Profiles.Add(newProfile);
            SelectedProfile = newProfile;
            Redraw();
        }
        private void DeleteProfile(Profile profile)
        {
            var _deletedProfileIndex = Area.Profiles.IndexOf(profile);
            var _SelectedProfileIndex = Area.Profiles.IndexOf(SelectedProfile);
            Area.Profiles.Remove(profile);
            Redraw();

            if (_deletedProfileIndex == _SelectedProfileIndex)
            {
                if (_deletedProfileIndex == 0)
                { SelectedProfile = Area.Profiles.FirstOrDefault(); }
                else if (_deletedProfileIndex == Area.Points.Count)
                { SelectedProfile = Area.Profiles.LastOrDefault(); }
                else if (_deletedProfileIndex > 0 && _deletedProfileIndex < Area.Profiles.Count)
                { SelectedProfile = Area.Profiles[_deletedProfileIndex]; }
            }
        }
        public AreaCardViewModel(Area area)
        {
            Area = area;

            ZoomCommand = ReactiveCommand.Create<object>(Zoom);

            RedrawCommand = ReactiveCommand.Create(Redraw);

            AddPointCommand = ReactiveCommand.Create(AddPoint);
            GeneratePointCommand = ReactiveCommand.Create(GeneratePoint);
            DeletePointCommand = ReactiveCommand.Create<AreaPoint>(DeletePoint);

            AddProfileCommand = ReactiveCommand.Create(AddProfile);
            DeleteProfileCommand = ReactiveCommand.Create<Profile>(DeleteProfile);

            this.WhenAnyValue(x => x.SelectedPoint)
                .Subscribe(_ => Redraw());

            this.WhenAnyValue(x => x.SelectedProfile)
                .Subscribe(_ => Redraw());

            this.WhenAnyValue(x => x.Area)
                .Where(x => x != null)
                .Subscribe(_ => Redraw());

            Redraw();
        }
    }
}
