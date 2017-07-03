using Common.Events.Messages;
using Common.Views;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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

        #region Message

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            nameof(Message), typeof(Message), typeof(Main), null);

        public Message Message
        {
            get { return (Message)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        #endregion

        public ViewModels.Main Model
        {
            get { return (ViewModels.Main)DataContext; }
            private set { DataContext = value; }
        }

        private async Task Initialize()
        {
            try
            {
                Message = new Progress(Model.Name, "Initializing...");
                Model = new ViewModels.Main();
                await Model.Initialize();
            }
            catch (Exception e)
            {
                Message = new Error(Model.Name, e);
            }
            finally
            {
                Message = new Message(Model.Name, "Initialized");
                await Task.Delay(TimeSpan.FromSeconds(5));
                Message = null;
            }
        }
    }
}
