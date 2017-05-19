using Common.Identity;
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

        public abstract Task Initialize();
    }
}
