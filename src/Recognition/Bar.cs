using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Recognition
{
  public static class Bar
  {
    public static int? FindHpBarLength(this Bitmap bmp, Rectangle rect)
    {
      var img = new Image<Hsv, byte>(bmp);
      var mat = new Mat(img.Mat, rect);
      {
        var mask = mat.ToImage<Hsv, byte>();
        //todo color param
        var hpMask = mask.InRange(new Hsv(0, 181, 109), new Hsv(1, 214, 141));
        var hpLines = LineCorners(hpMask);
        if (!hpLines.Any())
          return null;
        //todo find best
        var line = hpLines.FirstOrDefault();
        return (int) line.Size.Width;
      }
    }

    private static List<RotatedRect> LineCorners(Image<Gray, byte> mImgThreshold)
    {
      List<RotatedRect> boxList = new List<RotatedRect>();

      using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
      {
        CvInvoke.FindContours(mImgThreshold, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
        var count = contours.Size;
        for (var i = 0; i < count; i++)
        {
          using (VectorOfPoint contour = contours[i])
          using (VectorOfPoint approxContour = new VectorOfPoint())
          {
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
            //if (CvInvoke.ContourArea(approxContour, false) < 10) //only consider contours with area greater than 250
            // continue;

            if (approxContour.Size != 2) // line
              continue;
            if (approxContour[1].X - approxContour[0].X < 5
//                              || approxContour[1].Y - approxContour[0].Y < 2
            )
              continue;
            
            boxList.Add(CvInvoke.MinAreaRect(approxContour));

          }
        }
      }

      return boxList;
    }
  }
}