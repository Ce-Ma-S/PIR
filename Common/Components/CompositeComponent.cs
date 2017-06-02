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

        protected override async Task DoInitialize()
        {
            foreach (var component in Components)
                await component.Initialize();
        }

        protected override async Task DoApplyIsOn()
        {
            foreach (var component in Components)
                component.IsOn = IsOn;
        }

        protected override void Dispose(bool disposing)
        {
            AddDisposables(Components.ToArray());
            base.Dispose(disposing);
        }
    }
}
