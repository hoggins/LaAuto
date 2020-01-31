using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfLa2
{
  public class WatchWndMacro : MacroBase
  {
    public override string Title => "Test wnd screenshot";
    
    private IntPtr _targetWnd;

    protected override async Task Initialize()
    {
      _targetWnd = await MacroModel.GetClientTarget(Ct);
    }

    protected override async Task Run()
    {
      while (!Ct.IsCancellationRequested)
      {
        await Task.Delay(100, Ct).ConfigureAwait(false);
        Thread.Sleep(500);
        using (var snapshot = new LaWndSnapshot(_targetWnd))
        {
          var hp = snapshot.GetPartyHp();
          var thp = snapshot.GetTargetHp();

          Status = $"hp:{hp:0} thp:{thp}";
        }
      }
    }
  }
}