using GeoGravityOverDose.Models.Base;

namespace GeoGravityOverDose.Models
{
    public class Area : IdClass
    {
        public string Name { get; set; }

        public Project Project { get; set; }
    }
}
