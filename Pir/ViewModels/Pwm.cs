using Common;
using Common.Events;
using System;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace Pir.ViewModels
{
    public class Pwm :
        NotifyPropertyChange,
        IInitializable
    {
        #region Controller

        public async Task Initialize() => controller = await Pwm​Controller.GetDefaultAsync();

        public double Frequency
        {
            get => controller?.ActualFrequency ?? 0;
            set => SetPropertyValue(
                () => Frequency,
                () => controller?.SetDesiredFrequency(value)
                );
        }
        public double MinFrequency => controller?.MinFrequency ?? 0;
        public double MaxFrequency => controller?.MaxFrequency ?? 0;

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

        public double DutyCycle
        {
            get => pin?.GetActiveDutyCyclePercentage() ?? 0;
            set => SetPropertyValue(
                () => DutyCycle,
                () => pin?.SetActiveDutyCyclePercentage(value));
        }

        public bool Negated
        {
            get => pin?.Polarity == PwmPulsePolarity.ActiveLow;
            set => SetPropertyValue(
                () => Negated,
                () =>
                {
                    if (pin != null)
                    {
                        pin.Polarity = value ?
                            PwmPulsePolarity.ActiveLow :
                            PwmPulsePolarity.ActiveHigh;
                    }
                });
        }

        public bool IsStarted
        {
            get => pin?.IsStarted ?? false;
            set => SetPropertyValue(
                () => IsStarted,
                () =>
                {
                    if (pin != null)
                    {
                        if (value)
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
