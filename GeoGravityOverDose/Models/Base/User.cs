using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace GeoGravityOverDose.Models.Base
{
    public class User : IdClass
    {

        [Reactive]
        public string FirstName { get; set; }

        [Reactive]
        public string? LastName { get; set; }

        [Reactive]
        public string Family { get; set; }

        [Reactive]
        public string Phone { get; set; }

        [ObservableAsProperty]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Account Account { get; set; }

        public User()
        {
            this.WhenAnyValue(
                fullNameData => fullNameData.FirstName,
                fullNameData => fullNameData.LastName,
                fullNameData => fullNameData.Family)
                .Select(t => $"{t.Item1} {t.Item2} {t.Item3}")
                .ToPropertyEx(this, x => x.FullName);
        }
    }
}
