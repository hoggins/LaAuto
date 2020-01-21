using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AutoIt;

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
  public class SimpleMacro : IMacros
  {
    class InjectContext : IDisposable
    {
      private IntPtr _hwnd;
//      private Point _prevPos;

      public InjectContext()
      {
        _hwnd = AutoItX.WinGetHandle();
//        _prevPos = AutoItX.MouseGetPos();
      }

      public void Dispose()
      {
        //AutoItX.MouseMove(_prevPos.X, _prevPos.Y, 1000);
        AutoItX.WinActivate(_hwnd);
      }
    }
    
    private IntPtr _targetWnd;

    public SimpleMacro(IntPtr targetWnd)
    {
      _targetWnd = targetWnd;
    }

    public void Run()
    {
      var lastUse = new DateTime();
      while (true)
      {
        MainVm.SetWarn(true);
        Thread.Sleep(1000);

        using (new InjectContext())
        {
          AutoItX.WinActivate(_targetWnd);
          AutoItX.Send("2");
        }

        lastUse = DateTime.Now;

        MainVm.SetWarn(false);

        if (lastUse - DateTime.Now < TimeSpan.FromSeconds(2.5))
          Thread.Sleep(2500);

        for (int i = 30; i >= 0; i--)
        {
          using (var snapshot = new LaWndSnapshot(_targetWnd))
          {
            var hp = snapshot.GetPartyHp();

            MainVm.SetMessage($"t: {i} hp:{hp:0}");

            if (hp.HasValue)
            {
              if (hp < 5)
              {
                MainVm.SetMessage("Consider dead " + DateTime.Now.ToString("g"));
                return;
              }

              if (hp < 68)
                break;
            }
          }

          Thread.Sleep(1000);
        }
      }
    }
  }
}