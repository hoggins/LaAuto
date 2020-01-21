using System;
using System.Threading;

namespace WpfLa2
{
  public class WatchWndMacro : IMacros
  {
    private IntPtr _targetWnd;

    public WatchWndMacro(IntPtr targetWnd)
    {
      _targetWnd = targetWnd;
    }

    public void Run()
    {
      while (true)
      {
        Thread.Sleep(500);
        using (var snapshot = new LaWndSnapshot(_targetWnd))
        {
          var hp = snapshot.GetPartyHp();
          var thp = snapshot.GetTargetHp();

          MainVm.SetMessage($"hp:{hp:0} thp:{thp}");
        }
      }
    }
  }
}