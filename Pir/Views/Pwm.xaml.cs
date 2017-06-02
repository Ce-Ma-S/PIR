using Common.Views;
using Controls;

namespace Pir.Views
{
    public partial class Pwm :
        ComponentControl,
        IViewOf<ViewModels.Pwm>
    {
        public Pwm()
        {
            this.InitializeComponent();
        }

        public ViewModels.Pwm Model => (ViewModels.Pwm)DataContext;
    }
}
