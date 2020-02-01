using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace CvShotTest
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      

      Debugger.Break();
//      ImageViewer.Show(img);


    }

    public static void CycleTest()
    {
      var bmp = TakeScreenShot(new Rectangle(0, 0, 500, 500));
      var img = new Image<Hsv, byte>(bmp);
      
      bmp.Dispose();
      
      img.ROI = new Rectangle(0,0,400,400);
      
      var hpMask = img.InRange(new Hsv(0, 181, 109), new Hsv(1, 214, 141));
      
      
      hpMask.Dispose();
      img.Dispose();
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
  }
}