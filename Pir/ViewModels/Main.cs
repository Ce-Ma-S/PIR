using Common.Components;
using Microsoft.IoT.Lightning.Providers;
using System.Collections.Generic;
using Windows.Devices;

namespace Pir.ViewModels
{
    public class Main :
        CompositeComponent
    {
        public Main() :
            base("PIR")
        {
            if (LightningProvider.IsLightningEnabled)
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
        }

        public override string Description => "Plasmatic Implosion Reactor";
        public override IEnumerable<IComponent> Components
        {
            get
            {
                yield return Pwm;
                yield return Amplifier;
            }
        }

        public Pwm Pwm { get; } = new Pwm()
        {
            PinNumber = 4
        };
        public Amplifier Amplifier { get; } = new Amplifier()
        {
            ForwardPinNumber = 27,
            BackwardPinNumber = 17
        };
    }
}
