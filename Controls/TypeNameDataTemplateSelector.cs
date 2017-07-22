using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Controls
{
    public class TypeNameDataTemplateSelector :
        DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var name = item?.GetType().Name;
            object resource;
            return
                (name == null ?
                    null :
                    ((FrameworkElement)container).Resources.TryGetValue(name + "Template", out resource) ?
                        resource as DataTemplate :
                        null
                ) ??
                base.SelectTemplateCore(item, container);
        }
    }
}
