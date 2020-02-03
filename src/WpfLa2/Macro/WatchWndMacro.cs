using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LaClient;

namespace WpfLa2.Macro
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
        using (var snapshot = new LaClientSnapshot(_targetWnd))
        {
          var client = snapshot.Build();
          var sb = new StringBuilder();
          sb.AppendLine($"tHp: {client.TargetHp}%");
          foreach (var member in client.Party)
          {
            sb.AppendLine($"party hp {member.Hp}% mp {member.Mp}");
          }

          Status = sb.ToString();
        }
      }
    }
  }
}