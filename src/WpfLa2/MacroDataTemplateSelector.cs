using System.Windows;
using System.Windows.Controls;

namespace WpfLa2
{
  public class MacroDataTemplateSelector : DataTemplateSelector
  {
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      if (container is FrameworkElement element && item is IMacroVm macro)
      {
        return element.FindResource(macro.DataTemplate) as DataTemplate;
      }

      return null;
    }
  }
}