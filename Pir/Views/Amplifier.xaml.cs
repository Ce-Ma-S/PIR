using Common.Views;
using Controls;

namespace Pir.Views
{
    public partial class Amplifier :
        ComponentControl,
        IViewOf<ViewModels.Amplifier.Ibt2>
    {
        public Amplifier()
        {
            this.InitializeComponent();
        }

        public ViewModels.Amplifier.Ibt2 Model => (ViewModels.Amplifier.Ibt2)DataContext;
    }
}
