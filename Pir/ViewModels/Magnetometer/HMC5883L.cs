using Common.Components;
using Common.Validation;
using Microsoft.IoT.Lightning.Providers;
using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;
using Windows.Devices.Pwm;

namespace Pir.ViewModels.Magnetometer
{
    public class HMC5883L :
        I2c.I2cDevice
    {
        public HMC5883L() :
            base("HMC5883L")
        { }

        public override int Address => 0x1E;

        public override string Name => "Magnetometer";
        public override string Description => "Measures magnetic field induction";
    }
}
