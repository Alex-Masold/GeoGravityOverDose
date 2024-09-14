using GeoGravityOverDose.Models.Base;

namespace GeoGravityOverDose.Models
{
    public class Customer : User
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
