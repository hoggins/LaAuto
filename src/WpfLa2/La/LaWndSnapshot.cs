using System;
using System.Drawing;
using AutoIt;
using Recognition;

namespace WpfLa2.La
{
  public class LaWndSnapshot : IDisposable
  {
    private Bitmap _shot;

    public LaWndSnapshot(IntPtr hwnd)
    {
      var rect = AutoItX.WinGetPos(hwnd);
        
      var shotFrame = LaMarkup.PartyRoi;
      shotFrame.Offset(rect.Location);
      _shot = DrawUtil.TakeScreenShot(shotFrame);
    }

    public int? GetPartyHp()
    {
      return GetHp(LaMarkup.Party1Frame, LaMarkup.PartyHpLength);
    }

    public int? GetTargetHp()
    {
      return GetHp(LaMarkup.TargetFrame, LaMarkup.TargetHpLength);
    }

    private int? GetHp(Rectangle r, int length)
    {
      r.Offset(-LaMarkup.PartyRoi.Location.X, -LaMarkup.PartyRoi.Location.Y);
      var len = _shot.FindHpBarLength(r);
      if (!len.HasValue)
        return null;
      return (int?) (len / (double) length * 100);
    }

    public void Dispose()
    {
      //_shot.Dispose();
    }
  }
}