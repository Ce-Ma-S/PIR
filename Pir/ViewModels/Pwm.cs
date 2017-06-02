using Common.Components;
using Common.Validation;
using System;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace Pir.ViewModels
{
    public class Pwm :
        Component,
        ISwitchable
    {
        public Pwm() :
            base("PWM")
        { }

        public override string Description => "Pulse Width Modulation";

        protected override async Task DoApplyIsOn()
        {
            if (IsOn)
            {
                controller = await Pwm​Controller.GetDefaultAsync();
                ApplyFrequency();
                ApplyPinNumber();
            }
            else
            {
                ClosePin();
                controller = null;
            }
        }

        #region Controller

        public double Frequency
        {
            get => frequency;
            set => SetPropertyValue(ref frequency, value, OnFrequencyChanged);
        }

        private void ApplyFrequency()
        {
            ArgumentValidation.NonNull(controller, nameof(controller));
            var value = controller.SetDesiredFrequency(Frequency);
            SetPropertyValue(ref frequency, value, propertyName: nameof(Frequency));
        }

        private void OnFrequencyChanged()
        {
            if (IsOn)
                ApplyFrequency();
        }

        public double MinFrequency => controller?.MinFrequency ?? 0;
        public double MaxFrequency => controller?.MaxFrequency ?? double.MaxValue;

        private Pwm​Controller controller;
        private double frequency;

        #endregion

        #region Pin

        public int? PinNumber
        {
            get => pinNumber;
            set => SetPropertyValue(ref pinNumber, value, OnPinNumberChanged);
        }

        private void OnPinNumberChanged()
        {
            if (IsOn)
                ApplyPinNumber();
        }
        private void OpenPin()
        {
            if (PinNumber.HasValue)
            {
                pin = controller.OpenPin(PinNumber.Value);
                AddDisposables(pin);
            }
        }
        private void ClosePin()
        {
            if (pin != null)
            {
                RemoveDisposables(pin);
                pin = null;
            }
        }
        private void ApplyPinNumber()
        {
            ClosePin();
            OpenPin();
            ApplyNegated();
            ApplyDutyCycle();
            ApplyPinIsOn();
        }
        private void ApplyPinIsOn()
        {
            ValidatePin();
            if (IsOn)
                pin.Start();
        }

        private int? pinNumber;

        public double DutyCycle
        {
            get => dutyCycle;
            set => SetPropertyValue(ref dutyCycle, value, OnDutyCycleChanged);
        }

        private void OnDutyCycleChanged()
        {
            if (IsOn)
                ApplyDutyCycle();
        }

        private void ApplyDutyCycle()
        {
            ValidatePin();
            pin.SetActiveDutyCyclePercentage(DutyCycle);
            var value = pin.GetActiveDutyCyclePercentage();
            SetPropertyValue(ref dutyCycle, value, propertyName: nameof(DutyCycle));
        }

        public bool Negated
        {
            get => negated;
            set => SetPropertyValue(ref negated, value, OnNegatedChanged);
        }

        private void OnNegatedChanged()
        {
            if (IsOn)
                ApplyNegated();
        }

        private void ApplyNegated()
        {
            ValidatePin();
            pin.Polarity = Negated ?
                PwmPulsePolarity.ActiveLow :
                PwmPulsePolarity.ActiveHigh;
        }

        private void ValidatePin() => ArgumentValidation.NonNull(pin, nameof(pin));

        private Pwm​Pin pin;
        private double dutyCycle;
        private bool negated;

        #endregion
    }
}
