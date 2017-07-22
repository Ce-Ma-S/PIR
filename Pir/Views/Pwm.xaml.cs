using Common.Views;
using Controls;

namespace Pir.Views
{
    public partial class Pwm :
        ComponentControl,
        IViewOf<ViewModels.Pwm.Pwm>
    {
        public Pwm()
        {
            this.InitializeComponent();
        }

        public ViewModels.Pwm.Pwm Model => (ViewModels.Pwm.Pwm)DataContext;
    }
}
