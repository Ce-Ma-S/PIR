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

        #region IInitializable

        public bool IsInitialized { get; private set; }

        public async Task Initialize()
        {
            if (IsInitialized)
                throw new InvalidOperationException($"{Id} is already initialized.");
            try
            {
                await DoInitialize();
            }
            catch
            {
                if (IsOn)
                    IsOn = false;
                throw;
            }
        }

        protected virtual async Task DoInitialize() => await ApplyIsOn();

        #endregion

        #region ISwitchable

        public bool IsOn
        {
            get => isOn;
            set => SetPropertyValue(ref isOn, value, OnIsOnChanged);
        }

        protected async Task ApplyIsOn()
        {
            try
            {
                await DoApplyIsOn();
            }
            catch
            {
                SetPropertyValue(ref isOn, !IsOn, propertyName: nameof(IsOn));
                throw;
            }
        }
        protected abstract Task DoApplyIsOn();

        protected virtual async void OnIsOnChanged()
        {
            if (IsInitialized)
                await ApplyIsOn();
        }

        private bool isOn;

        #endregion
    }
}
