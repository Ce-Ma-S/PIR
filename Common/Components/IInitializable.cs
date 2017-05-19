using System.Threading.Tasks;

namespace Common.Components
{
    public interface IInitializable
    {
        Task Initialize();
    }
}
