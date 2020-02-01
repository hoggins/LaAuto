using System.Drawing;

namespace WpfLa2.La
{
  public static class LaMarkup
  {
    public static int PartyHpLength = 123;
    public static int TargetHpLength = 185;
    public static Rectangle Party1Frame = Rectangle.FromLTRB(53, 217,184, 251);
    public static Rectangle TargetFrame = Rectangle.FromLTRB(24, 133, 214, 146);

    public static Rectangle PartyRoi;

    static LaMarkup()
    {
      PartyRoi = Rectangle.Union(Party1Frame, TargetFrame);
    }
  }
}