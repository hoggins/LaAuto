using System;
using System.Drawing;
using Emgu.CV.Structure;

namespace LaClient
{
  public static class LaMarkup
  {
    public const int PartyHpLength = 123;
    public const int TargetHpLength = 185;

    public static readonly Point PartyTopLeft = new Point(23, 194);

    public static readonly Rectangle PartyMemberFrame = new Rectangle(0,0, 163, 58);
    public static readonly Rectangle PartyMemberNameFrame = new Rectangle(37,0, 120, 22);
    public static readonly Rectangle PartyMemberBarsFrame = new Rectangle(29,27, 132, 30);

    public static readonly Rectangle[] PartyBarFrames = new Rectangle[4];

    public static Rectangle TargetFrame = Rectangle.FromLTRB(24, 131, 216, 146);

    public static Rectangle ShotFrame;
    public static Rectangle[] ShotPartyBarFrames;
    public static Rectangle ShotTargetFrame;

    public static Tuple<Hsv, Hsv> HpRange = Tuple.Create(new Hsv(0, 181, 109), new Hsv(1, 214, 141));
    public static Tuple<Hsv, Hsv> MpRange = Tuple.Create(new Hsv(103, 241, 105), new Hsv(108, 248, 173));

    static LaMarkup()
    {
      var srcParty = PartyTopLeft;
      var h = PartyMemberFrame.Height;
      var bars = PartyMemberBarsFrame;
      for (var i = 0; i < PartyBarFrames.Length; i++)
      {
        var mLoc = new Point(srcParty.X,srcParty.Y + h * i);
        var barsLoc = new Point(mLoc.X + bars.X, mLoc.Y + bars.Y);
        PartyBarFrames[i] = new Rectangle(barsLoc, bars.Size);
      }

      ShotFrame = TargetFrame;
      ShotFrame = Rectangle.Union(ShotFrame, new Rectangle(0,0, 1,1));
      foreach (var frame in PartyBarFrames)
      {
        ShotFrame = Rectangle.Union(ShotFrame, frame);
      }

      ShotTargetFrame = Offset(TargetFrame);

      ShotPartyBarFrames = new Rectangle[PartyBarFrames.Length];
      for (var i = 0; i < PartyBarFrames.Length; i++)
      {
        ShotPartyBarFrames[i] = Offset(PartyBarFrames[i]);
      }
    }

    public static Rectangle Offset(Rectangle rect)
    {
      rect.Offset(-ShotFrame.Location.X, -ShotFrame.Location.Y);
      return rect;
    }
  }
}