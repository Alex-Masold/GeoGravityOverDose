using GeoGravityOverDose.Models.Base;

namespace GeoGravityOverDose.Models
{
    public class Role : IdClass
    {
        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
