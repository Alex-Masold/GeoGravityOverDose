using GeoGravityOverDose.Models.Base;

namespace GeoGravityOverDose.Models
{
    public class Account : IdClass
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
