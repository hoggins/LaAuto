using System;
using System.Drawing;
using System.Net.Mime;
using Patagames.Ocr;
using Patagames.Ocr.Enums;

namespace TextVision
{
  public class Class1
  {
    public static void ConvertImageToText(Bitmap img)
    {
      using (var api = OcrApi.Create())
      {
//        api.Init(dataPath:@"F:\!prog\La2\WpfLa2\WpfLa2\bin\Debug\x64\tesseract.dll",language:Languages.English);
        api.Init(language:Languages.English);
        
//        string plainText = api.GetTextFromImage(img);
        string plainText = api.GetTextFromImage("screenshot2.png");
        Console.WriteLine(plainText);
        Console.Read();
      }
    }
  }
}