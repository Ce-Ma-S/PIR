using Common.Events.Messages;
using Common.Identity;
using System;
using System.Threading.Tasks;

namespace Common.Components
{
    public abstract class Component :
        Identity<string>,
        IComponent
    {
        public Component(string id) :
            base(id)
        { }

        #region ISwitchable

        public bool? IsOn
        {
            get
            {
                bool? isOn;
                switch (SwitchState)
                {
                    case SwitchState.SwitchedOff:
                        isOn = false;
                        break;
                    case SwitchState.SwitchedOn:
                        isOn = true;
                        break;
                    case SwitchState.SwitchingOn:
                    case SwitchState.SwitchingOff:
                    default:
                        isOn = null;
                        break;
                }
                return isOn;
            }
        }
        public SwitchState SwitchState
        {
            get => switchState;
            private set => SetPropertyValue(ref switchState, value, OnSwitchStateChanged);
        }

        public async Task SwitchOn()
        {
            switch (SwitchState)
            {
                case SwitchState.SwitchingOn:
                    throw new InvalidOperationException($"{Id} is already switching on.");
                case SwitchState.SwitchedOn:
                    throw new InvalidOperationException($"{Id} is already switched on.");
                case SwitchState.SwitchingOff:
                    throw new InvalidOperationException($"{Id} is switching off.");
                case SwitchState.SwitchedOff:
                default:
                    break;
            }
            try
            {
                await DoWithProgress(async () =>
                {
                    SwitchState = SwitchState.SwitchingOn;
                    await DoSwitchOn();
                    SwitchState = SwitchState.SwitchedOn;
                },
                "Switch on"
                );
            }
            catch
            {
                SwitchState = SwitchState.SwitchedOff;
                throw;
            }
        }
        protected abstract Task DoSwitchOn();

        public async Task SwitchOff()
        {
            switch (SwitchState)
            {
                case SwitchState.SwitchingOff:
                    throw new InvalidOperationException($"{Id} is already switching off.");
                case SwitchState.SwitchedOff:
                    throw new InvalidOperationException($"{Id} is already switched off.");
                case SwitchState.SwitchingOn:
                    throw new InvalidOperationException($"{Id} is switching on.");
                case SwitchState.SwitchedOn:
                default:
                    break;
            }
            try
            {
                await DoWithProgress(async () =>
                {
                    SwitchState = SwitchState.SwitchingOff;
                    await DoSwitchOff();
                    SwitchState = SwitchState.SwitchedOff;
                },
                "Switch off"
                );
            }
            catch
            {
                SwitchState = SwitchState.SwitchedOn;
                throw;
            }
        }
        protected abstract Task DoSwitchOff();

        protected virtual void OnSwitchStateChanged() => OnPropertyChanged(nameof(IsOn));

        private SwitchState switchState;

        #endregion

        #region Message

        public Message Message
        {
            get => message;
            protected set => SetPropertyValue(ref message, value);
        }

        protected async Task DoWithProgress(Func<Task> action, string name)
        {
            try
            {
                Message = new Progress(Name, name);
                await action();
                Message = null;
            }
            catch (Exception e)
            {
                Publish(e, name);
                throw;
            }
        }

        protected void Publish(Exception error, string name = null) => Message = new Error(
            Name,
            name == null ?
                null :
                $"{name} failed",
            error
            );

        private Message message;

        #endregion
    }
}
