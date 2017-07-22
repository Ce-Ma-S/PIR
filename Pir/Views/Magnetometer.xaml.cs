using Common.Views;
using Controls;

namespace Pir.Views
{
    public partial class Magnetometer :
        ComponentControl,
        IViewOf<ViewModels.Magnetometer.Hmc5883l>
    {
        public Magnetometer()
        {
            this.InitializeComponent();
        }

        public ViewModels.Magnetometer.Hmc5883l Model => (ViewModels.Magnetometer.Hmc5883l)DataContext;
    }
}
