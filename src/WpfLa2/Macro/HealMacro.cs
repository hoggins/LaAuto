using System;
using System.Threading.Tasks;
using AutoIt;
using WpfLa2.La;

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
      var lastUse = new DateTime();
      while (!Ct.IsCancellationRequested)
      {

        SetWarn(true);
        using (new InjectContext())
        {
          AutoItX.WinActivate(_targetWnd);
          AutoItX.Send("2");
        }
        SetWarn(false);

        lastUse = DateTime.Now;

        if (lastUse - DateTime.Now < TimeSpan.FromSeconds(2.5))
          await Task.Delay(2500, Ct).ConfigureAwait(false);

        for (int i = NoOperationDelay; i >= 0 && !Ct.IsCancellationRequested; i--)
        {
          using (var snapshot = new LaWndSnapshot(_targetWnd))
          {
            var hp = snapshot.GetPartyHp();

            Status = $"t: {i} hp:{hp:0}";

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
          }

          await Task.Delay(1000, Ct).ConfigureAwait(false);
        }
      }
    }
  }
}