using System.Threading;
using System.Threading.Tasks;

namespace WpfLa2
{
  public interface IMacros
  {
    Task Initialize(CancellationToken ct);
    void Run(CancellationToken ct);
  }
}