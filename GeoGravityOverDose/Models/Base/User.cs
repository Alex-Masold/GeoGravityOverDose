namespace GeoGravityOverDose.Models.Base
{
    public class User : IdClass
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Family { get; set; }
        public string Phone { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Account Account { get; set; }

        public User()
        {
        }
    }
}
