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

    protected override async Task Initialize()
    {
      Status = "Select window to watch and click heal";
      _targetWnd = await MacroModel.GetClientTarget(Ct);
      Status = "Ok";
    }

    protected override async Task Run()
    {
      while (!Ct.IsCancellationRequested)
      {
        SetWarn(true);
        using (new InjectContext())
        {
          AutoItX.WinActivate(_targetWnd);
          AutoItX.Send("2");
        }

        SetWarn(false);

        await Task.Delay(2000, Ct).ConfigureAwait(false);

        for (int i = NoOperationDelay*5; i >= 0 && !Ct.IsCancellationRequested; i--)
        {
          var client = MacroModel.GetClientModel(_targetWnd);
          var hp = client.Party.FirstOrDefault()?.Hp;

          Status = $"t: {i/5} hp:{hp:0}";

          if (hp.HasValue)
          {
            if (hp < 5)
            {
              Status = "Consider dead " + DateTime.Now.ToString("g");
              return;
            }

            if (hp < PercentToHeal)
              break;
          }

          await Task.Delay(200, Ct).ConfigureAwait(false);
        }
      }
    }
  }
}