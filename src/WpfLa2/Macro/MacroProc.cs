using System;
using System.Threading;

namespace WpfLa2
{
  public class MacroProc : IDisposable
  {
    private Thread _trd;

    public MacroProc(IMacros macro)
    {
      _trd = new Thread(macro.Run)
      {
        IsBackground = true
      };
      _trd.Start();
    }

    public void Dispose()
    {
      _trd.Abort();
    }
  }
}