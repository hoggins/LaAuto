using System;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;
using LaClient;

namespace WpfLa2
{
  public class MacroModel
  {
    public static async Task<IntPtr> GetClientTarget(CancellationToken ct = default)
    {
      return await GetWnd(ct);
    }

    public static async Task<IntPtr> GetClientObserve(CancellationToken ct = default)
    {
      return await GetWnd(ct);
    }

    public static LaClientModel GetClientModel(IntPtr hWnd)
    {
      using (var s1 = new LaClientSnapshot(hWnd))
        return s1.Build();
    }

    private static async Task<IntPtr> GetWnd(CancellationToken ct = default)
    {
      while (true)
      {
        var hwnd = AutoItX.WinGetHandle();
        var title = AutoItX.WinGetTitle(hwnd);
        if (title.Contains("Lineage"))
          return hwnd;

        await Task.Delay(500, ct).ConfigureAwait(false);
      }
    }
  }
}