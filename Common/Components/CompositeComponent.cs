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

        public override async Task Initialize()
        {
            foreach (var component in Components)
                await component.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            AddDisposables(Components.ToArray());
            base.Dispose(disposing);
        }
    }
}
