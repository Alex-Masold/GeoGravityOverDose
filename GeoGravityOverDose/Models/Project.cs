using GeoGravityOverDose.Models.Base;

namespace GeoGravityOverDose.Models
{
    public class Project : IdClass
    {
        public string Name { get; set; }
        public string? Address { get; set; }
    }
}
