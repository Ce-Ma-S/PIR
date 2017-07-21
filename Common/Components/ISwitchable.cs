using System.Threading.Tasks;

namespace Common.Components
{
    public interface ISwitchable
    {
        bool? IsOn { get; }
        SwitchState SwitchState { get; }
        Task SwitchOn();
        Task SwitchOff();
    }
}
