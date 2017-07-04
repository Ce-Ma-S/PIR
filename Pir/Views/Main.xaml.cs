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
            nameof(Message), typeof(Message), typeof(Main), new PropertyMetadata(null, OnMessageChanged));
        public static readonly DependencyProperty MessageVisibilityProperty =
            DependencyProperty.Register(nameof(MessageVisibility), typeof(Visibility), typeof(Main), null);
        
        public Message Message
        {
            get { return (Message)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        public Visibility MessageVisibility
        {
            get { return (Visibility)GetValue(MessageVisibilityProperty); }
            set { SetValue(MessageVisibilityProperty, value); }
        }

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Main)d;
            control.MessageVisibility = e.NewValue == null ?
                Visibility.Collapsed :
                Visibility.Visible;
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
                Model = new ViewModels.Main();
                Message = new Progress(Model.Name, "Initializing...");
                await Model.Initialize();
                Message = new Message(Model.Name, "Initialized");
                await Task.Delay(TimeSpan.FromSeconds(5));
                Message = null;
            }
            catch (Exception e)
            {
                Message = new Error(Model.Name, null, e);
            }
        }
    }
}
