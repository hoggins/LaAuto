using System;
using System.Drawing;
using System.Threading;
using AutoIt;
using Emgu.CV;
using Emgu.CV.Structure;

namespace LaClient
{
  public class LaClientSnapshot : IDisposable
  {
    private static object _lock = new object();

    private readonly Image<Hsv, byte> _shot;

    public LaClientSnapshot(IntPtr hwnd)
    {
      Monitor.Enter(_lock);
      var rect = AutoItX.WinGetPos(hwnd);

      var shotFrame = LaMarkup.ShotFrame;
      shotFrame.Offset(rect.Location);

      using (var bmp = TakeScreenShot(shotFrame))
        _shot = new Image<Hsv, byte>(bmp);
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
      r.Offset(-LaMarkup.ShotFrame.Location.X, -LaMarkup.ShotFrame.Location.Y);
      var len = _shot.FindBarLength(r, LaMarkup.HpRange);
      if (!len.HasValue)
        return null;
      return (int?) (len / (double) length * 100);
    }

    public void Dispose()
    {
      _shot.Dispose();
      Monitor.Exit(_lock);
    }

    private static Bitmap TakeScreenShot(Rectangle rect)
    {
      Bitmap bmp = new Bitmap(rect.Width, rect.Height);
      using (Graphics g = Graphics.FromImage(bmp))
        g.CopyFromScreen(rect.Location, Point.Empty, rect.Size);
      return bmp;
    }
  }
}