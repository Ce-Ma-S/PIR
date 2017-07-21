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

        protected override async Task DoSwitchOn() => await SwitchComponents(i => i.SwitchOn(), "on");
        protected override async Task DoSwitchOff() => await SwitchComponents(i => i.SwitchOff(), "off");

        private async Task SwitchComponents(Func<IComponent, Task> action, string name)
        {
            var errors = new List<Exception>();
            foreach (var component in Components)
            {
                try
                {
                    await action(component);
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
