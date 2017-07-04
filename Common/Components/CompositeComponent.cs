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

        protected override async Task DoInitialize()
        {
            await base.DoInitialize();
            var errors = new List<Exception>();
            foreach (var component in Components)
            {
                try
                {
                    await component.Initialize();
                }
                catch (Exception e)
                {
                    errors.Add(new Exception($"Initialization of {component} failed", e));
                }
            }
            if (errors.Count > 0)
                throw new AggregateException("Initialization failed", errors);
        }

        protected override Task DoApplyIsOn()
        {
            var errors = new List<Exception>();
            var switchText = IsOn ? "on" : "off";
            foreach (var component in Components)
            {
                try
                {
                    component.IsOn = IsOn;
                }
                catch (Exception e)
                {
                    errors.Add(new Exception($"Switching {switchText} of {component} failed", e));
                }
            }
            if (errors.Count > 0)
                throw new AggregateException($"Switching {switchText} failed", errors);
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            AddDisposables(Components.ToArray());
            base.Dispose(disposing);
        }
    }
}
