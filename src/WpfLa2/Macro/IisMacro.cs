using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using AutoIt;

namespace WpfLa2
{
  public class AssistMacro : IMacros
  {
    private IntPtr _watchWnd;
    private readonly IntPtr _actWnd;
    private int? _lastHp;

    public AssistMacro(IntPtr watchWnd, IntPtr actWnd)
    {
      _watchWnd = watchWnd;
      _actWnd = actWnd;
    }

    public void Run()
    {
      var canSwitch = false;
      while (true)
      {
        using (var snapshot = new LaWndSnapshot(_watchWnd))
        {
          var hp = snapshot.GetTargetHp();
          if (!hp.HasValue)
          {
            Thread.Sleep(1000);
            continue;
          }
          
          MainVm.SetMessage2($"Assist: last {_lastHp} tHP:{hp}%");

          if (_lastHp.HasValue && hp > _lastHp)
            canSwitch = true;
          if (canSwitch && hp < 97)
          {
            canSwitch = false;
            using (new InjectContext())
            {
              AutoItX.WinActivate(_actWnd);
              AutoItX.Send("{NUMPAD0}");
            }
            Thread.Sleep(1000);
          }
          
          _lastHp = hp;
          
          Thread.Sleep(1000);
        }
      }
    }
  }

  public class IisMacro : IMacros
  {
    private IntPtr _targetWnd;

    public IisMacro(IntPtr targetWnd)
    {
      _targetWnd = targetWnd;
    }

    public void Run()
    {
      var lastUse = new DateTime();
      while (true)
      {

        using (new InjectContext())
        {
          AutoItX.WinActivate(_targetWnd);
          AutoItX.Send("2");
        }

        lastUse = DateTime.Now;

        if (lastUse - DateTime.Now < TimeSpan.FromSeconds(2.5))
          Thread.Sleep(2500);

        for (int i = 35; i >= 0; i--)
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