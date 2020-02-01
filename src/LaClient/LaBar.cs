using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace LaClient
{
  public static class LaBar
  {
    public static int? FindBarLength(this Image<Hsv, byte> img, Rectangle rect, Tuple<Hsv, Hsv> barColor)
    {
      var oldRoi = img.ROI;
      img.ROI = rect;
      using (var hpMask = img.InRange(barColor.Item1, barColor.Item2))
      {
        var hpLines = LineCorners(hpMask);

        img.ROI = oldRoi;

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