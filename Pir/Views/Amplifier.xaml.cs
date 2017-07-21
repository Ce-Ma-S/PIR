using Common.Views;
using Controls;

namespace Pir.Views
{
    public partial class Amplifier :
        ComponentControl,
        IViewOf<ViewModels.Ibt2>
    {
        public Amplifier()
        {
            this.InitializeComponent();
        }

        public ViewModels.Ibt2 Model => (ViewModels.Ibt2)DataContext;
    }
}
