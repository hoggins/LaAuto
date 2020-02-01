using System;
using System.Drawing;
using Emgu.CV.Structure;

namespace LaClient
{
  public static class LaMarkup
  {
    public static int PartyHpLength = 123;
    public static int TargetHpLength = 185;
    public static Rectangle Party1Frame = Rectangle.FromLTRB(53, 217,184, 251);
    public static Rectangle TargetFrame = Rectangle.FromLTRB(24, 133, 214, 146);

    public static Rectangle ShotFrame;

    public static Tuple<Hsv, Hsv> HpRange = Tuple.Create(new Hsv(0, 181, 109), new Hsv(1, 214, 141));

    static LaMarkup()
    {
      ShotFrame = Rectangle.Union(Party1Frame, TargetFrame);
    }
  }
}