using System;
using System.Linq;
using System.Threading.Tasks;
using AutoIt;
using LaClient;

namespace WpfLa2.Macro
{
  public class HealMacro : MacroBase
  {
    private IntPtr _targetWnd;

    public override string Title => "Heal";
    public override string DataTemplate => "MacroHealDt";

    public int NoOperationDelay { get; set; } = 35;

    public int PercentToHeal { get; set; } = 68;

    private DateTime _lastHealTime;

    protected override async Task Initialize()
    {
      Status = "Select window to watch and click heal";
      _targetWnd = await MacroModel.GetClientTarget(Ct);
      Status = "Ok";
    }

    protected override async Task Run()
    {
      const int baseButton = 2;
      while (!Ct.IsCancellationRequested)
      {
        if (_lastHealTime.AddSeconds(NoOperationDelay) < DateTime.UtcNow)
          await DoHeal(baseButton.ToString());


        var model = MacroModel.GetClientModel(_targetWnd);

        var hpStr = string.Join(" ", model.Party.Select(m => $"hp:{m.Hp:0}"));
        var cd = (DateTime.UtcNow - _lastHealTime).TotalSeconds;
        Status = $"t: {cd:0} " + hpStr;

        for (int pIndex = 0; pIndex < model.Party.Count; pIndex++)
        {
          var m = model.Party[pIndex];
          if (m.Hp.HasValue && m.Hp < PercentToHeal)
          {
            await DoHeal((baseButton + pIndex).ToString());
            break;
          }
        }

        await Task.Delay(200, Ct).ConfigureAwait(false);

      }
    }

    private async Task DoHeal(string button)
    {
      SetWarn(true);
      using (new InjectContext())
      {
        AutoItX.WinActivate(_targetWnd);
        AutoItX.Send(button);
      }
      SetWarn(false);
      await Task.Delay(2000, Ct).ConfigureAwait(false);

      _lastHealTime = DateTime.UtcNow;
    }
  }
}