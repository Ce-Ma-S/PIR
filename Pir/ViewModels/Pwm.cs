using Common.Events;
using System;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace Pir.ViewModels
{
    public class Pwm :
        NotifyPropertyChange
    {
        #region Controller

        public async Task Init() => controller = await Pwm​Controller.GetDefaultAsync();

        public double? Frequency
        {
            get => controller?.ActualFrequency;
            set => SetPropertyValue(
                () => Frequency,
                () => controller?.SetDesiredFrequency(value ?? MinFrequency.Value)
                );
        }
        public double? MinFrequency => controller?.MinFrequency;
        public double? MaxFrequency => controller?.MaxFrequency;

        private Pwm​Controller controller;

        #endregion

        #region Pin

        public int? PinNumber
        {
            get => pinNumber;
            set => SetPropertyValue(ref pinNumber, value, OnPinNumberChanged);
        }
        private void OnPinNumberChanged()
        {
            if (pin != null)
            {
                RemoveDisposables(pin);
                pin = null;
            }
            if (PinNumber.HasValue)
            {
                pin = controller.OpenPin(PinNumber.Value);
                AddDisposables(pin);
            }
        }
        private int? pinNumber;

        public double? DutyCycle
        {
            get => pin?.GetActiveDutyCyclePercentage();
            set => SetPropertyValue(
                () => DutyCycle,
                () => pin?.SetActiveDutyCyclePercentage(value.GetValueOrDefault()));
        }

        public bool? Negated
        {
            get => pin?.Polarity == PwmPulsePolarity.ActiveLow;
            set => SetPropertyValue(
                () => Negated,
                () =>
                {
                    if (
                        pin != null &&
                        value.HasValue
                        )
                    {
                        pin.Polarity = value.Value ?
                            PwmPulsePolarity.ActiveLow :
                            PwmPulsePolarity.ActiveHigh;
                    }
                });
        }

        public bool? IsStarted
        {
            get => pin?.IsStarted;
            set => SetPropertyValue(
                () => IsStarted,
                () =>
                {
                    if (
                        pin != null &&
                        value.HasValue
                        )
                    {
                        if (value.Value)
                            pin.Start();
                        else
                            pin.Stop();
                    }
                });
        }

        private Pwm​Pin pin;

        #endregion
    }
}
