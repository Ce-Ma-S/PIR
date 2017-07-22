using Common.Events.Messages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Controls
{
    /// <summary>
    /// Shows <see cref="Message"/>s.
    /// </summary>
    public sealed class MessageControl :
        Control
    {
        public MessageControl() => DefaultStyleKey = typeof(MessageControl);

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
            nameof(Message), typeof(Message), typeof(MessageControl), new PropertyMetadata(null, OnMessageChanged));

        public Message Message
        {
            get => (Message)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            ShowMessage(false);
        }

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MessageControl)d;
            var message = (Message)e.NewValue;
            control.ShowMessage(message != null);
        }

        private void ShowMessage(bool show) => Visibility = show ?
            Visibility.Visible :
            Visibility.Collapsed;
    }
}
