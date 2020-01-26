using System;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;

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
    
    private static async Task<IntPtr> GetWnd(CancellationToken ct = default)
    {
      for (int i = 0; i < 15; i++)
      {
        var hwnd = AutoItX.WinGetHandle();
        var title = AutoItX.WinGetTitle(hwnd);
        if (title.Contains("Lineage"))
          return hwnd;
        await Task.Delay(1000, ct).ConfigureAwait(false);
      }

      return default(IntPtr);
    }
  }
}