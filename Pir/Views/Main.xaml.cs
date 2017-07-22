using Common.Components;
using Common.Views;
using Windows.UI.Xaml.Controls;

namespace Pir.Views
{
    /// <summary>
    /// Main page.
    /// </summary>
    public sealed partial class Main :
        Page,
        IViewOf<ViewModels.Main>
    {
        public Main()
        {
            InitializeComponent();
            Initialize();
        }

        public ViewModels.Main Model
        {
            get { return (ViewModels.Main)DataContext; }
            private set { DataContext = value; }
        }

        private async void Initialize()
        {
            Model = new ViewModels.Main();
            await Model.SwitchOnSafe();
        }
    }
}
