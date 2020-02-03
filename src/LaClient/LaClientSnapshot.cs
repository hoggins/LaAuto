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
    private static readonly object Lock = new object();

    private readonly Image<Hsv, byte> _shot;

    public LaClientSnapshot(IntPtr hwnd)
    {
      Monitor.Enter(Lock);
      var rect = AutoItX.WinGetPos(hwnd);

      var shotFrame = LaMarkup.ShotFrame;
      shotFrame.Offset(rect.Location);

      using (var bmp = TakeScreenShot(shotFrame))
        _shot = new Image<Hsv, byte>(bmp);
    }

    public LaClientModel Build()
    {
      var m = new LaClientModel();
      m.TargetHp = GetBarValue(LaMarkup.ShotTargetFrame, LaMarkup.TargetHpLength, LaMarkup.HpRange);

      for (var i = 0; i < LaMarkup.ShotPartyBarFrames.Length; i++)
      {
        var f = LaMarkup.ShotPartyBarFrames[i];
        var hp = GetBarValue(f, LaMarkup.PartyHpLength, LaMarkup.HpRange);
        var mp = GetBarValue(f, LaMarkup.PartyHpLength, LaMarkup.MpRange);
        if (hp.HasValue || mp.HasValue)
        {
          // add dead members
          for (int j = 0; j < i; j++)
            m.Party.Add(new LaClientMember());

          m.Party.Add(new LaClientMember
          {
            Hp = hp,
            Mp = mp,
          });
        }
      }

      return m;
    }

    public void Debug()
    {
      var bmp = _shot.Bitmap;
      using (Graphics g = Graphics.FromImage(bmp))
      {
        var p1 = LaMarkup.PartyMemberFrame;
        p1.Offset(LaMarkup.PartyTopLeft);
        p1 = LaMarkup.Offset(p1);

        g.DrawRectangle(Pens.Blue, p1);

        g.DrawRectangle(Pens.Blue, LaMarkup.ShotTargetFrame);
        g.DrawRectangle(Pens.Blue, LaMarkup.ShotTargetFrame);
        g.DrawRectangle(Pens.Blue, LaMarkup.ShotPartyBarFrames[0]);
      }
      bmp.Save("markupTest.png");
    }

    private int? GetBarValue(Rectangle r, int length, Tuple<Hsv, Hsv> barColor)
    {
      var len = _shot.FindBarLength(r, barColor);
      if (!len.HasValue)
        return null;
      return (int?) (len / (double) length * 100);
    }

    public void Dispose()
    {
      _shot.Dispose();
      Monitor.Exit(Lock);
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