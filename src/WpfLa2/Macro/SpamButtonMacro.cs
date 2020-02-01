using System;
using System.Threading.Tasks;
using AutoIt;

namespace WpfLa2.Macro
{
  public class SpamButtonMacro : MacroBase
  {
    private IntPtr _targetWnd;

    public override string Title => "SpamButton";
    public override string DataTemplate => "MacroSpamButtonDt";

    public int Delay { get; set; } = 30;
    public string Key { get; set; } = "{NUMPAD8}";

    protected override async Task Initialize()
    {
      Status = "Select window to click";
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
          AutoItX.Send(Key);
        }
        SetWarn(false);

        for (var i = Delay; i >= 0 && !Ct.IsCancellationRequested; i--)
        {
          Status = $"Click {Key} in {i} sec";
          await Task.Delay(1000, Ct).ConfigureAwait(false);
        }
      }
    }
  }
}