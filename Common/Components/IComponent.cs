using Common.Identity;
using System;

namespace Common.Components
{
    public interface IComponent :
        IIdentity<string>,
        IInitializable,
        IDisposable
    { }
}
