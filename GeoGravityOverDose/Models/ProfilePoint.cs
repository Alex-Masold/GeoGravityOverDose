﻿using GeoGravityOverDose.Models.Base;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;

namespace GeoGravityOverDose.Models
{
    public class ProfilePoint : IdClass
    {
        [Reactive]
        public double X { get; set; }

        [Reactive]
        public double Y { get; set; }

        [Reactive]
        public Profile Profile { get; set; }


        [NotMapped]
        public Point P => new Point(X, Y);
    }
}
