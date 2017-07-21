using Common.Components;
using Common.Validation;
using Microsoft.IoT.Lightning.Providers;
using System;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace Pir.ViewModels
{
    public class Pwm :
        Component
    {
        public Pwm() :
            base("PWM")
        { }

        public override string Description => "Pulse Width Modulation";

        protected override async Task DoSwitchOn()
        {
            //controller = await Pwm​Controller.GetDefaultAsync();
            // TODO: how to get soft PWM without guessing or studying source code?
            // https://github.com/ms-iot/lightning/blob/develop/Providers/PwmDeviceProvider.cpp
            var controllers = await Pwm​Controller.GetControllersAsync(LightningPwmProvider.GetPwmProvider());
            controller = controllers[1];    // software PWM
            if (Frequency < controller.MinFrequency)
                Frequency = controller.MinFrequency;
            else if (Frequency > controller.MaxFrequency)
                Frequency = controller.MaxFrequency;
            else
                ApplyFrequency();
            ApplyPinNumber();
        }
        protected override Task DoSwitchOff()
        {
            ClosePin();
            controller = null;
            return Task.CompletedTask;
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
            if (IsOn == true)
                ApplyFrequency();
        }

        public double MinFrequency => controller?.MinFrequency ?? 0;
        public double MaxFrequency => controller?.MaxFrequency ?? double.MaxValue;

        private Pwm​Controller controller;
        private double frequency;

        #endregion

        #region Pin

        public int PinNumber
        {
            get => pinNumber;
            set => SetPropertyValue(ref pinNumber, value, OnPinNumberChanged);
        }

        private void OnPinNumberChanged()
        {
            if (IsOn == true)
                ApplyPinNumber();
        }
        private void OpenPin()
        {
            pin = controller.OpenPin(PinNumber);
            AddDisposables(pin);
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
            if (IsOn == true)
                pin.Start();
        }

        private int pinNumber;

        public double DutyCycle
        {
            get => dutyCycle;
            set => SetPropertyValue(ref dutyCycle, value, OnDutyCycleChanged);
        }

        private void OnDutyCycleChanged()
        {
            if (IsOn == true)
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
            if (IsOn == true)
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
