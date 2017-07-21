using Common.Components;
using Microsoft.IoT.Lightning.Providers;
using Pir.ViewModels.Amplifier;
using Pir.ViewModels.Pwm;
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

        public SoftwarePwm Pwm { get; } = new SoftwarePwm()
        {
            PinNumber = 4
        };
        public Ibt2 Amplifier { get; } = new Ibt2()
        {
            ForwardPinNumber = 27,
            BackwardPinNumber = 17
        };
    }
}
