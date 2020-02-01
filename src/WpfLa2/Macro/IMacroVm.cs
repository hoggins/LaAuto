namespace WpfLa2.Macro
{
  public interface IMacroVm
  {
    string Title { get; }
    string DataTemplate { get; }
    void Start();
    void Stop();
  }
}