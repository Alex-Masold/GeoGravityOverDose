using GeoGravityOverDose.Models;
using GeoGravityOverDose.Models.Base;
using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

        private void Redraw()
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

        public AreaCardViewModel()
        {
            this.WhenAnyValue(x => x.SelectedPoint)
            .Subscribe(_ => Redraw());

            this.WhenAnyValue(x => x.SelectedProfile)
                .Subscribe(_ => Redraw());
        }
    }
}
