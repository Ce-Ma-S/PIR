using Common.Components;
using Common.Validation;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Pir.ViewModels
{
    public class Amplifier :
        Component,
        ISwitchable
    {
        public Amplifier() :
            base("Amplifier")
        { }

        public override string Description => "Digital Amplifier";

        protected override async Task DoApplyIsOn()
        {
            if (IsOn)
            {
                controller = await GpioController.GetDefaultAsync();
                forwardPin = OpenPin(ForwardPinNumber);
                backwardPin = OpenPin(BackwardPinNumber);
                ApplyReversedPolarity();
            }
            else
            {
                ClosePin(ref forwardPin);
                ClosePin(ref backwardPin);
                controller = null;
            }
        }

        #region Pins

        #region ForwardPin

        public int ForwardPinNumber
        {
            get => forwardPinNumber;
            set => SetPropertyValue(ref forwardPinNumber, value, OnForwardPinNumberChanged);
        }

        private void OnForwardPinNumberChanged()
        {
            if (IsOn)
                ApplyPinNumber(ref forwardPin, ForwardPinNumber);
        }

        private int forwardPinNumber;
        private Gpio​Pin forwardPin;

        #endregion

        #region BackwardPin

        public int BackwardPinNumber
        {
            get => backwardPinNumber;
            set => SetPropertyValue(ref backwardPinNumber, value, OnBackwardPinNumberChanged);
        }

        private void OnBackwardPinNumberChanged()
        {
            if (IsOn)
                ApplyPinNumber(ref backwardPin, BackwardPinNumber);
        }

        private int backwardPinNumber;
        private Gpio​Pin backwardPin;

        #endregion

        private GpioPin OpenPin(int pinNumber)
        {
            GpioPin pin = controller.OpenPin(pinNumber);
            pin.SetDriveMode(GpioPinDriveMode.Output);
            AddDisposables(pin);
            return pin;
        }
        private void ClosePin(ref GpioPin pin)
        {
            if (pin == null)
                return;
            RemoveDisposables(pin);
            pin = null;
        }
        private void ApplyPinNumber(ref GpioPin pin, int pinNumber)
        {
            ClosePin(ref pin);
            pin = OpenPin(pinNumber);
            ApplyReversedPolarity();
        }

        #region ReversedPolarity

        public bool ReversedPolarity
        {
            get => reversedPolarity;
            set => SetPropertyValue(ref reversedPolarity, value, OnReversedPolarityChanged);
        }

        private void ApplyReversedPolarity()
        {
            if (ReversedPolarity)
            {
                ArgumentValidation.NonNull(backwardPin, nameof(backwardPin));
                forwardPin?.Write(GpioPinValue.Low);
                backwardPin.Write(GpioPinValue.High);
            }
            else
            {
                ArgumentValidation.NonNull(forwardPin, nameof(forwardPin));
                forwardPin.Write(GpioPinValue.High);
                backwardPin?.Write(GpioPinValue.Low);
            }
        }

        private void OnReversedPolarityChanged()
        {
            if (IsOn)
                ApplyReversedPolarity();
        }

        private bool reversedPolarity;

        #endregion

        #endregion

        private GpioController controller;
    }
}
