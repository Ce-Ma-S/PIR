using Common.Components;
using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace Pir.ViewModels.I2c
{
    public abstract class I2cDevice :
        Component
    {
        public I2cDevice(string id) :
            base(id)
        { }

        public abstract int Address { get; }

        protected Windows.Devices.I2c.I2cDevice Device { get; private set; }

        protected override async Task DoSwitchOn()
        {
            controller = await I2cController.GetDefaultAsync();
            Device = controller.GetDevice(new I2cConnectionSettings(Address));
        }
        protected override Task DoSwitchOff()
        {
            Device.Dispose();
            Device = null;
            controller = null;
            return Task.CompletedTask;
        }

        private I2cController controller;
    }
}
