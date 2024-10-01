using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoGravityOverDose.Models
{
    public class Operator : User
    {
        [Reactive]
        public ObservableCollection<Profile> Profiles { get; set; }
    }
}
