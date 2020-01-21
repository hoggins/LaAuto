using System;
using System.Threading;
using AutoIt;

namespace WpfLa2
{
  class InjectContext : IDisposable
  {
    private static readonly object Lock = new object();
    private IntPtr _hwnd;

    public InjectContext()
    {
      MainVm.SetWarn(true);
      Thread.Sleep(1000);
      
      Monitor.Enter(Lock);
      
      _hwnd = AutoItX.WinGetHandle();
    }

    public void Dispose()
    {
      AutoItX.WinActivate(_hwnd);
      Monitor.Exit(Lock);
      MainVm.SetWarn(false);
    }
  }
}