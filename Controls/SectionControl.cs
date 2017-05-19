using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Controls
{
    public class SectionControl :
        ContentControl
    {
        public SectionControl()
        {
            this.DefaultStyleKey = typeof(SectionControl);
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(string), typeof(SectionControl), null);

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            nameof(HeaderContent), typeof(object), typeof(SectionControl), null);

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            nameof(HeaderTemplate), typeof(DataTemplate), typeof(SectionControl), null);

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(SectionControl), null);

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
    }
}
