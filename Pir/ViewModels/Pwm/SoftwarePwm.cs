using Microsoft.IoT.Lightning.Providers;
using System;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace Pir.ViewModels.Pwm
{
    public class SoftwarePwm :
        Pwm
    {
        public SoftwarePwm() :
            base("Software PWM")
        { }

        protected override async Task<PwmController> GetController()
        {
            //return await Pwm​Controller.GetDefaultAsync();
            // TODO: how to get soft PWM without guessing or studying source code?
            // https://github.com/ms-iot/lightning/blob/develop/Providers/PwmDeviceProvider.cpp
            var controllers = await Pwm​Controller.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
            return controllers[1];    // software PWM
        }
    }
}
