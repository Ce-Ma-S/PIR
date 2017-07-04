using Common.Components;
using Microsoft.IoT.Lightning.Providers;
using System.Threading.Tasks;
using Windows.Devices;
using System.Collections.Generic;

namespace Pir.ViewModels
{
    public class Main :
        CompositeComponent
    {
        public Main() :
            base("PIR")
        {
            IsOn = true;
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

        protected override async Task DoInitialize()
        {
            if (LightningProvider.IsLightningEnabled)
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            await base.DoInitialize();
        }
    }
}
