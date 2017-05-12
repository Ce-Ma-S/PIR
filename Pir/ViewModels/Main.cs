using Common;
using Common.Events;
using System.Threading.Tasks;

namespace Pir.ViewModels
{
    public class Main :
        NotifyPropertyChange,
        IInitializable
    {
        public Pwm Pwm { get; } = new Pwm();

        public async Task Initialize()
        {
            await Pwm.Initialize();
        }
    }
}
