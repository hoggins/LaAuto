using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AutoIt;

namespace WpfLa2
{
  public static class DrawUtil
  {
    [DllImport("User32.dll")]
    static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("User32.dll")]
    static extern void ReleaseDC(IntPtr dc);
    
    public static void Draw(Brush brush)
    {
      IntPtr desktop = GetDC(IntPtr.Zero);
      using (Graphics g = Graphics.FromHdc(desktop)) {
        g.FillRectangle(brush, 0, 0, 500, 100);
      }
      ReleaseDC(desktop);
    }

    public static void DrawLabel(string text, Brush bg = null, Brush fg = null)
    {
      bg = bg ?? Brushes.Green;
      fg = fg ?? Brushes.Black;
      IntPtr desktop = GetDC(IntPtr.Zero);
      using (Graphics g = Graphics.FromHdc(desktop)) {
        g.FillRectangle(bg, 0, 0, 500, 30);
        g.DrawString(text, new Font(System.Drawing.FontFamily.GenericSerif, 20), fg, 0, 0);
      }
      ReleaseDC(desktop);
    }

    public static Bitmap TakeScreenShot()
    {
      Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
        return bmp;
      }
    }

    public static Bitmap TakeScreenShot(Rectangle rect)
    {
      Bitmap bmp = new Bitmap(rect.Width, rect.Height);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        g.CopyFromScreen(rect.Location, Point.Empty, rect.Size);
        return bmp;
      }                 
    }
    
    public static Bitmap TakeScreenShotIna()
    {
      
      Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
        return bmp;
      }                 
    }

    public static void Draw(Bitmap screenshot)
    {
            IntPtr desktop = GetDC(IntPtr.Zero);
            using (Graphics g = Graphics.FromHdc(desktop)) {
              g.DrawImage(screenshot, new Point(0,0));
            }
            ReleaseDC(desktop);
    }
  }
}