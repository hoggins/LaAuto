using System;
using System.Threading;
using System.Threading.Tasks;

namespace WpfLa2
{
  public class WatchWndMacro : IMacros
  {
    private IntPtr _targetWnd;

    public async Task Initialize(CancellationToken ct)
    {
      _targetWnd = await MacroModel.GetClientTarget(ct);
    }

    public void Run(CancellationToken ct)
    {
      while (!ct.IsCancellationRequested)
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