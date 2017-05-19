using Common.Components;
using Microsoft.IoT.Lightning.Providers;
using System.Threading.Tasks;
using Windows.Devices;
using System;
using System.Collections.Generic;

namespace Pir.ViewModels
{
    public class Main :
        CompositeComponent
    {
        public Main() :
            base("PIR")
        { }

        public override string Description => "Plasmatic Implosion Reactor";
        public override IEnumerable<IComponent> Components
        {
            get
            {
                yield return Pwm;
            }
        }

        public Pwm Pwm { get; } = new Pwm();

        public override async Task Initialize()
        {
            if (LightningProvider.IsLightningEnabled)
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            await Pwm.Initialize();
        }
    }
}
