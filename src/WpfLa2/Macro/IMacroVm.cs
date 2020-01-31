using System.Threading;
using System.Threading.Tasks;

namespace WpfLa2
{
  public interface IMacroVm
  {
    string Title { get; }
    string DataTemplate { get; }
    void Start();
    void Stop();
  }
}