using Common.Views;
using Windows.UI.Xaml.Controls;

namespace Pir.Views
{
    public sealed partial class Pwm :
        UserControl,
        IViewOf<ViewModels.Pwm>
    {
        public Pwm()
        {
            this.InitializeComponent();
        }

        public ViewModels.Pwm Model
        {
            get { return (ViewModels.Pwm)DataContext; }
            private set { DataContext = value; }
        }
    }
}
