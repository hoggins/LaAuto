using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoIt;

namespace WpfLa2
{
  public class AssistMacro : IMacros
  {
    private IntPtr _watchWnd;
    private IntPtr _actWnd;
    private int? _lastHp;

    public async Task Initialize(CancellationToken ct)
    {
      MainVm.SetMessage2("Go to wnd to watch target HP");
      _watchWnd = await MacroModel.GetClientObserve(ct);
      
      MainVm.SetMessage2("Go to wnd to click assis");
      IntPtr t = default;
      while (!ct.IsCancellationRequested)
      {
        t = await MacroModel.GetClientTarget(ct);
        if (t != _watchWnd)
          break;
      }

      _actWnd = t;
    }

    public void Run(CancellationToken ct)
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

    public async Task Initialize(CancellationToken ct)
    {
      _targetWnd = await MacroModel.GetClientTarget(ct);
    }

    public void Run(CancellationToken ct)
    {
      var lastUse = new DateTime();
      while (!ct.IsCancellationRequested)
      {

        using (new InjectContext())
        {
          AutoItX.WinActivate(_targetWnd);
          AutoItX.Send("2");
        }

        lastUse = DateTime.Now;

        if (lastUse - DateTime.Now < TimeSpan.FromSeconds(2.5))
          Thread.Sleep(2500);

        for (int i = 35; i >= 0 && !ct.IsCancellationRequested; i--)
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