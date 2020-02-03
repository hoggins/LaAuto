using System;
using System.Threading.Tasks;
using AutoIt;
using LaClient;

namespace WpfLa2.Macro
{
  public class AssistMacro : MacroBase
  {
    public int HpToStartDd { get; set; } = 97;

    private IntPtr _watchWnd;
    private IntPtr _actWnd;
    private int? _lastHp;

    public override string Title => "Assist";
    public override string DataTemplate => "MacroAssistDt";

    protected override async Task Initialize()
    {
      Status = "Go to window to watch target HP";
      _watchWnd = await MacroModel.GetClientObserve(Ct);

      Status = "Go to window to click assist";
      IntPtr t = default;
      while (!Ct.IsCancellationRequested)
      {
        t = await MacroModel.GetClientTarget(Ct);
        if (t != _watchWnd)
          break;
      }

      _actWnd = t;
    }

    protected override async Task Run()
    {
      var canSwitch = false;
      while (!Ct.IsCancellationRequested)
      {
        await Task.Delay(200, Ct).ConfigureAwait(false);
        var watch = MacroModel.GetClientModel(_watchWnd);
        var hp = watch.TargetHp;

        Status = $"Assist: last {_lastHp} tHP:{hp}%";

        if (!watch.TargetHp.HasValue)
          continue;

        if (_lastHp.HasValue && hp > _lastHp)
          canSwitch = true;

        if (canSwitch && hp < HpToStartDd)
        {
          canSwitch = false;
          SetWarn(true);
          using (new InjectContext())
          {
            AutoItX.WinActivate(_actWnd);
            AutoItX.Send("{NUMPAD0}");
          }
          SetWarn(false);
          await Task.Delay(1500, Ct).ConfigureAwait(false);
        }

        _lastHp = hp;
      }
    }
  }
}