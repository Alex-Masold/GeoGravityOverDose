using ReactiveUI;
using System.Reactive.Linq;

namespace GeoGravityOverDose.ViewModels
{
    public class AuthorizationViewModel : ReactiveObject
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set { this.RaiseAndSetIfChanged(ref _email, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        private readonly ObservableAsPropertyHelper<string> _fullTest;
        public string FullTest => _fullTest.Value;
        public AuthorizationViewModel()
        {
            _fullTest = this.WhenAnyValue(
                authData => authData.Email,
                authData => authData.Password)
                .Select(t => $"{t.Item1} {t.Item2}")
                .ToProperty(this, authData => authData.FullTest);
        }
    }
}
