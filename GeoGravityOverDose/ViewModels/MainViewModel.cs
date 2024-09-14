using ReactiveUI;

namespace GeoGravityOverDose.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private object _currentVM;
        public object CurrentVM
        {
            get { return _currentVM; }
            set { this.RaiseAndSetIfChanged(ref _currentVM, value); }
        }
    }
}
