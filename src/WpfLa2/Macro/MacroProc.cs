using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfLa2
{
  public class MacroProc : IDisposable
  {
    private readonly CancellationTokenSource _cts;

    public MacroProc(IMacros macro)
    {
      _cts = new CancellationTokenSource();
      Task.Run(async () => await macro.Initialize(_cts.Token), _cts.Token)
        .ContinueWith((ot) => macro.Run(_cts.Token), _cts.Token);
    }

    public void Dispose()
    {
      _cts.Cancel();
      _cts.Dispose();
    }
  }
}