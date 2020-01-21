namespace Common
{
  public struct TPoint<T>
  {
    public readonly T X;
    public readonly T Y;

    public TPoint(T x, T y)
    {
      X = x;
      Y = y;
    }
  }

  public struct TRect<T>
  {
    public readonly TPoint<T> TopLeft;
    public readonly TPoint<T> BottomRight;
    
    public TRect(TPoint<T> topLeft, TPoint<T> bottomRight)
    {
      TopLeft = topLeft;
      BottomRight = bottomRight;
    }

    public TRect(T x, T y, T x1, T y1) : this()
    {
      TopLeft = new TPoint<T>(x,y);
      BottomRight = new TPoint<T>(x1, y1);
    }
  }
}