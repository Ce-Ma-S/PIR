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
        public virtual I2cBusSpeed BusSpeed => I2cBusSpeed.StandardMode;
        public virtual I2cSharingMode SharingMode => I2cSharingMode.Exclusive;

        protected Windows.Devices.I2c.I2cDevice Device { get; private set; }

        protected override async Task DoSwitchOn()
        {
            controller = await I2cController.GetDefaultAsync();
            var connectionSettings = new I2cConnectionSettings(Address)
            {
                BusSpeed = BusSpeed,
                SharingMode = SharingMode
            };
            Device = controller.GetDevice(connectionSettings);
            AddDisposables(Device);
        }

        protected override Task DoSwitchOff()
        {
            RemoveDisposables(Device);
            Device = null;
            controller = null;
            return Task.CompletedTask;
        }

        private I2cController controller;
    }
}
