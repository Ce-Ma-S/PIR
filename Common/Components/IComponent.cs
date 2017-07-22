using Common.Events.Messages;
using Common.Identity;
using System;

namespace Common.Components
{
    public interface IComponent :
        IIdentity<string>,
        IHaveMessage,
        ISwitchable,
        IDisposable
    { }
}
