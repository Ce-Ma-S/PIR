using Common;
using Common.Events;
using Microsoft.IoT.Lightning.Providers;
using System.Threading.Tasks;
using Windows.Devices;

namespace Pir.ViewModels
{
    public class Main :
        NotifyPropertyChange,
        IInitializable
    {
        public Pwm Pwm { get; } = new Pwm();

        public async Task Initialize()
        {
            if (LightningProvider.IsLightningEnabled)
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
            await Pwm.Initialize();
        }
    }
}
