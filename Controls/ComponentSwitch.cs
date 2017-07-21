using Common.Components;
using Controls.Converters;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Controls
{
    public static class ComponentSwitch
    {
        public static readonly DependencyProperty SetupProperty = DependencyProperty.RegisterAttached(
            "Setup", typeof(bool), typeof(ComponentSwitch), new PropertyMetadata(false, OnSetupChanged));

        public static bool GetSetup(DependencyObject obj) =>
            (bool)obj.GetValue(SetupProperty);
        public static void SetSetup(DependencyObject obj, bool value) =>
            obj.SetValue(SetupProperty, value);

        private static void OnSetupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
                return;
            var @switch = (ToggleSwitch)d;
            if (@switch == null)
                return;
            @switch.SetBinding(
                ToggleSwitch.IsOnProperty,
                new Binding()
                {
                    Path = new PropertyPath(nameof(IComponent.IsOn)),
                    Mode = BindingMode.OneWay
                });
            @switch.SetBinding(
                Control.IsEnabledProperty,
                new Binding()
                {
                    Path = new PropertyPath(nameof(IComponent.IsOn)),
                    Converter = IsNotNullConverter.Instance
                });
            @switch.Toggled += OnSwitchToggled;
        }

        private static async void OnSwitchToggled(object sender, RoutedEventArgs e)
        {
            var @switch = (ToggleSwitch)sender;
            var component = (IComponent)@switch.DataContext;
            if (component == null)
                return;
            if (!@switch.IsOn == component.IsOn)
            {
                if (@switch.IsOn)
                    await component.SwitchOn();
                else
                    await component.SwitchOff();
            }
        }
    }
}
