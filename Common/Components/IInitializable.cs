using System.Threading.Tasks;

namespace Common.Components
{
    public interface IInitializable
    {
        bool IsInitialized { get; }

        Task Initialize();
    }
}
