using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Components
{
    public abstract class CompositeComponent :
        Component
    {
        public CompositeComponent(string id) :
            base(id)
        { }

        public abstract IEnumerable<IComponent> Components { get; }
        public IComponent this[string id] => Components.
            FirstOrDefault(i => i.Id == id);

        protected override async Task DoSwitchOn() => await SwitchComponents(true);
        protected override async Task DoSwitchOff() => await SwitchComponents(false);

        private async Task SwitchComponents(bool on)
        {
            string name = on ? "on" : "off";
            var errors = new List<Exception>();
            foreach (var component in Components)
            {
                try
                {
                    if (on)
                    {
                        if (component.IsOn == false)
                            await component.SwitchOn();
                    }
                    else
                    {
                        if (component.IsOn == true)
                            await component.SwitchOff();
                    }
                }
                catch (Exception e)
                {
                    errors.Add(new Exception($"{component.Id} failed", e));
                }
            }
            if (errors.Count > 0)
                throw new AggregateException($"{Id} switching {name} failed", errors);
        }

        protected override void Dispose(bool disposing)
        {
            AddDisposables(Components.ToArray());
            base.Dispose(disposing);
        }
    }
}
