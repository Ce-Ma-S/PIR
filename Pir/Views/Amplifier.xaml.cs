using Common.Views;
using Controls;

namespace Pir.Views
{
    public partial class Amplifier :
        ComponentControl,
        IViewOf<ViewModels.Amplifier>
    {
        public Amplifier()
        {
            this.InitializeComponent();
        }

        public ViewModels.Amplifier Model => (ViewModels.Amplifier)DataContext;
    }
}
