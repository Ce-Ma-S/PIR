using Common.Math;
using Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace Pir.ViewModels.Magnetometer
{
    public class HMC5883L :
        I2c.I2cDevice
    {
        public HMC5883L() :
            base("HMC5883L")
        { }

        public override int Address => 0x1E;
        public override I2cBusSpeed BusSpeed => I2cBusSpeed.FastMode;

        public override string Name => "Magnetometer";
        public override string Description => "Measures magnetic field induction";

        protected override async Task DoSwitchOn()
        {
            await base.DoSwitchOn();
            ApplyConfigurationB();
            await ApplyFrequency();
        }
        protected override Task DoSwitchOff()
        {
            DisposeTimer();
            GoIdle();
            return base.DoSwitchOff();
        }

        #region Registers

        /// <summary>
        /// Configuration Register A (Read/Write)
        /// </summary>
        private const byte RegisterOfConfigurationA = 0x00;
        /// <summary>
        /// Configuration Register B (Read/Write)
        /// </summary>
        private const byte RegisterOfConfigurationB = 0x01;

        /// <summary>
        /// Mode Register (Read/Write)
        /// </summary>
        private const byte RegisterOfMode = 0x02;

        /// <summary>
        /// Data Output X MSB Register (Read only)
        /// </summary>
        private const byte RegisterOfDataOutputXMsb = 0x03;

        private void WriteRegister(byte register, byte value) => Device.Write(new[] { register, value });
        private void MoveToRegister(byte register) => Device.Write(new[] { register });
        private void ReadRegisters(byte register, byte[] values) => Device.WriteRead(new[] { register }, values);

        #endregion

        #region Configuration

        #region A

        private void OnConfigurationAChanged()
        {
            if (IsOn == true)
                ApplyConfigurationA();
        }

        private void ApplyConfigurationA()
        {
            byte value = (byte)(
                (byte)BiasMode |
                frequencyToConfigA[Frequency] ?? 00 |
                averagingSampleCountToConfigA[AveragingSampleCount]
                );
            WriteRegister(RegisterOfConfigurationA, value);
        }

        #endregion

        #region B

        private void OnConfigurationBChanged()
        {
            if (IsOn == true)
                ApplyConfigurationB();
        }

        private void ApplyConfigurationB()
        {
            byte value = Range.ConfigB;
            WriteRegister(RegisterOfConfigurationB, value);
        }

        #endregion

        #endregion

        #region AveragingSampleCount

        /// <summary>
        /// Number of samples to be averaged during one measurement.
        /// </summary>
        public byte AveragingSampleCount
        {
            get => averagingSampleCount;
            set
            {
                if (!averagingSampleCountToConfigA.ContainsKey(value))
                    throw new ArgumentOutOfRangeException(nameof(AveragingSampleCount));
                SetPropertyValue(ref averagingSampleCount, value, OnConfigurationAChanged);
            }
        }
        public IEnumerable<byte> AveragingSampleCounts => averagingSampleCountToConfigA.Keys;

        private static readonly Dictionary<byte, byte> averagingSampleCountToConfigA = new Dictionary<byte, byte>
        {
            [1] = 0,
            [2] = 0b01 << 5,
            [4] = 0b10 << 5,
            [8] = 0b11 << 5
        };

        private byte averagingSampleCount = 1;

        #endregion

        #region Frequency

        /// <summary>
        /// Measurement frequency for continuos measurements [Hz].
        /// </summary>
        /// <value>0 means single measurement mode.</value>
        public double Frequency
        {
            get => frequency;
            set
            {
                ArgumentValidation.In(value, frequencyToConfigA.Keys);
                SetPropertyValue(ref frequency, value, OnFrequencyChanged);
            }
        }
        public IEnumerable<double> Frequencies => frequencyToConfigA.Keys;

        private async void OnFrequencyChanged()
        {
            if (IsOn == true)
                await ApplyFrequency();
        }
        private async Task ApplyFrequency()
        {
            ApplyConfigurationA();
            // single mode
            if (Frequency == 0)
            {
                DisposeTimer();
                const byte singleMode = 0x01;
                WriteRegister(RegisterOfMode, singleMode);
                // Wait 6 ms (or monitor status register or DRDY hardware interrupt pin - not used)
                await Task.Delay(6);
                ReadValue();
            }
            // continuous mode
            else
            {
                const byte continuousMode = 0x00;
                WriteRegister(RegisterOfMode, continuousMode);
                // wait 2 periods, then take readings at given frequency
                var period = TimeSpan.FromSeconds(1 / Frequency);
                var doublePeriod = TimeSpan.FromTicks(period.Ticks * 2);
                timer = new Timer(
                    i => ReadValue(), null,
                    dueTime: doublePeriod,
                    period: period
                    );
                AddDisposables(timer);
            }
        }

        private static readonly Dictionary<double, byte?> frequencyToConfigA = new Dictionary<double, byte?>
        {
            [0] = null,
            [0.75] = 0,
            [1.5] = 0b001 << 2,
            [3] = 0b010 << 2,
            [7.5] = 0b011 << 2,
            [15] = 0b100 << 2,
            [30] = 0b101 << 2,
            [75] = 0b110 << 2
        };

        private void DisposeTimer()
        {
            if (timer != null)
                RemoveDisposables(timer);
            timer = null;
        }

        private double frequency = 15;
        private Timer timer;

        #endregion

        #region BiasMode

        public enum BiasModes :
            byte
        {
            None = 0,
            Positive = 0b01,
            Negative = 0b10
        }

        public BiasModes BiasMode
        {
            get => biasMode;
            set => SetPropertyValue(ref biasMode, value, OnConfigurationAChanged);
        }
        public IEnumerable<KeyValuePair<BiasModes, string>> BiasModeItems => biasModeItems;

        private static readonly Dictionary<BiasModes, string> biasModeItems = new Dictionary<BiasModes, string>
        {
            [BiasModes.None] = "None",
            [BiasModes.Positive] = "Positive",
            [BiasModes.Negative] = "Negative"
        };

        private BiasModes biasMode = BiasModes.None;

        #endregion

        #region Range

        public class RangeItem
        {
            internal RangeItem(
                double range /*[G]*/,
                double resolution /*[mG]/LSb*/,
                int gain /*LSb/[G]*/,
                byte configB
                )
            {
                Maximum = range * GaussToTesla;
                Minimum = -Maximum;
                Resolution = resolution * 0.001 * GaussToTesla;
                Gain = gain / GaussToTesla;
                ConfigB = (byte)(configB << 5);
            }

            public double Minimum { get; } /*[T]*/
            public double Maximum { get; } /*[T]*/
            public double Resolution { get; } /*[T]/LSb*/
            internal double Gain { get; } /*LSb/[T]*/
            internal byte ConfigB { get; }
        }

        /// <summary>
        /// Measurement range.
        /// </summary>
        public RangeItem Range
        {
            get => range;
            set
            {
                ArgumentValidation.In(value, Ranges);
                SetPropertyValue(ref range, value, OnConfigurationBChanged);
            }
        }
        public IReadOnlyList<RangeItem> Ranges => ranges;

        private static readonly RangeItem[] ranges = new[]
        {
            new RangeItem(0.88, 0.73, 1370, 0),
            new RangeItem(1.3, 0.92, 1090, 0b001),
            new RangeItem(1.9, 1.22, 820, 0b010),
            new RangeItem(2.5, 1.52, 660, 0b011),
            new RangeItem(4.0, 2.27, 440, 0b100),
            new RangeItem(4.7, 2.56, 390, 0b101),
            new RangeItem(5.6, 3.03, 330, 0b110),
            new RangeItem(8.1, 4.35, 230, 0b111)
        };

        private const double GaussToTesla = 0.0001;
        private RangeItem range = ranges[1];

        #endregion

        #region Value

        public Vector3d Value
        {
            get => value;
            set => SetPropertyValue(ref this.value, value);
        }

        public void ReadValue()
        {
            var values = new byte[6];
            // If gain is changed then this data set is using previous gain.
            ReadRegisters(0x06 /*read all 6 bytes*/, values);
            // point to first data X
            MoveToRegister(RegisterOfDataOutputXMsb);
            // Check the endianness of the system and flip the bytes if necessary
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(values, 0, 2);
                Array.Reverse(values, 2, 2);
                Array.Reverse(values, 4, 2);
            }
            // Convert three 16-bit 2’s compliment hex values to decimal values and assign to X, Z, Y, respectively.
            int x = BitConverter.ToInt16(values, 0);
            int z = BitConverter.ToInt16(values, 2);
            int y = BitConverter.ToInt16(values, 4);
            Value = new Vector3d(
                x / Range.Gain,
                y / Range.Gain,
                z / Range.Gain
                );
        }

        private Vector3d value;

        #endregion

        protected override void Dispose(bool disposing)
        {
            GoIdle();
            base.Dispose(disposing);
        }

        private void GoIdle()
        {
            const int idleMode = 0b10;
            WriteRegister(RegisterOfMode, idleMode);
        }
    }
}
