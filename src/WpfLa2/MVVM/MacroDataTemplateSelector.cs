using System.Windows;
using System.Windows.Controls;
using WpfLa2.Macro;

namespace WpfLa2.MVVM
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