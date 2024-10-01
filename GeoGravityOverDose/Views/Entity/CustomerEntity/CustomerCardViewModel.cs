using GeoGravityOverDose.ViewModels.Base;
using ReactiveUI.Fody.Helpers;
using GeoGravityOverDose.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using System.Windows;

namespace GeoGravityOverDose.Views.Entity.CustomerEntity
{
    public class CustomerCardViewModel : BaseViewModel
    {
        [Reactive]
        public Customer Customer { get; set; }
        public IReactiveCommand<Unit, Unit> TestMessageCommand { get; }

        public int MaxLengthName { get; set; } = 20;
        public int MaxLengthPassword { get; set; } = 10;
        public int MaxLengthPhone { get; set; } = 15;

        public void TestMessage()
        {
            MessageBox.Show("1");
        }

        public CustomerCardViewModel()
        {
            TestMessageCommand = ReactiveCommand.Create(TestMessage);
        }
    }
}
