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
            DataTemplate template;
            if (name == null)
                template = null;
            else
            {
                var templateName = name + "Template";
                var element = (FrameworkElement)container;
                template =
                    GetTemplate(element.Resources, templateName) ??
                    GetTemplate(Application.Current.Resources, templateName);
            }
            if (template == null)
                template = base.SelectTemplateCore(item, container);
            return template;
        }

        private DataTemplate GetTemplate(ResourceDictionary resources, string name) => resources.TryGetValue(name, out var resource) ?
            resource as DataTemplate :
            null;
    }
}
