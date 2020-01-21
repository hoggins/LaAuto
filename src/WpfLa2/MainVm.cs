using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoIt;

namespace WpfLa2
{
  public class MainVm : ViewModelBase
  {
    public static MainVm Instance;
    private string _color;
    public string Color
    {
      get => _color;
      set => SetProperty(ref _color, value);
    }
    
    private string _message;
    public string Message
    {
      get => _message;
      set => SetProperty(ref _message, value);
    }
    
    private string _hp;
    public string Hp
    {
      get => _message;
      set => SetProperty(ref _message, value);
    }
    
    public MainVm()
    {
      Instance = this;
      //var t = new Timer(s => WatchHP(), null, 0, 1000);
      
    }
    
    private ICommand _startMacroCommand;
    public ICommand StartMacroCommand
    {
      get
      {
        return _startMacroCommand ?? (_startMacroCommand = new CommandHandler(() => StartMacro(), ()=> true));
      }
    }
    
    private ICommand _markupCommand;
    public ICommand MarkupCommand
    {
      get
      {
        return _markupCommand ?? (_markupCommand = new CommandHandler(() => MarkupLaWindow(), ()=> true));
      }
    }
    
    private MacroProc _activeMacro;

    private async void StartMacro()
    {
      _activeMacro?.Dispose();

      var t = await GetTargetWnd();
      
      _activeMacro = new MacroProc(new SimpleMacro(t));
    }

    
    
    
    
    private async void MarkupLaWindow()
    {
      var target = await GetTargetWnd().ConfigureAwait(false);
      
      _activeMacro?.Dispose();
      _activeMacro = new MacroProc(new WatchWndMacro(target));
      return;
      
        
      var sw = new Stopwatch();
      using (var snapshot = new LaWndSnapshot(target))
      {
        SetMessage("p: " + snapshot.GetPartyHp()
                   + " t: " + snapshot.GetTargetHp());
      }
    }

    private async Task<IntPtr> GetTargetWnd()
    {
      for (int i = 0; i < 15; i++)
      {
        var hwnd = AutoItX.WinGetHandle();
        var title = AutoItX.WinGetTitle(hwnd);
        if (title.Contains("Lineage"))
          return hwnd;
        await Task.Delay(1000);
      }

      return default(IntPtr);
    }

    public static void SetMessage(string text)
    {
      if (Instance == null)
        return;
      Instance.Message = text;
    }
    
    public static void SetWarn(bool w)
    {
      if (Instance == null)
        return;
      Instance.Color = w ? "Red" : "White";
    }


    private void WatchHP()
    {
      var hp = CalcHP();
      Hp = hp.ToString();
    }

    private double? CalcHpSafe()
    {
      try
      {
        return CalcHP();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return null;
      }
    }

    private double CalcHP()
    {
      //var bmp = (Bitmap)Bitmap.FromFile("screenshot.png");
      var bmp = memScreenShot();

      var filledHp1 = unchecked((int) 0xFF7D4442);
      var filledHp2 = unchecked((int) 0xFF711F1D);
      
//      var emptyHp1 = unchecked((int) 0xFF40322E);
//      var emptyHp3 = System.Drawing.Color.FromArgb(65, 49, 44).ToArgb();
//      var emptyHp4 = System.Drawing.Color.FromArgb(49, 28, 24).ToArgb();
//      var emptyHp5 = -12570323;
//      var emptyHp2 = unchecked((int) 0xFF3A231E);

      
      var hpFrom = FindTopLeft(bmp, new Point(0, 300), new Point(200, 360), filledHp1);
      var hpTo = FindTopRight(bmp, new Point(0, 300), new Point(200, 360), filledHp1);

//      var emptyFrom = FindTopLeft(bmp, new Point(0, 300), new Point(200, 360), emptyHp5);
//      var emptyTo = FindTopRight(bmp, new Point(0, 300), new Point(200, 360), emptyHp5);

      
      
//      using (Graphics g = Graphics.FromImage(bmp))
//      {
//        DrawPoint(g, hpFrom);
//        DrawPoint(g, hpTo);
//        DrawPoint(g, emptyFrom);
//        DrawPoint(g, emptyTo);
//        bmp.Save("screenshot3.png");  // saves the image
//      }
      
      bmp.Dispose();

      if (!hpFrom.HasValue || !hpTo.HasValue)
        throw new Exception("hp bars not visible");

//      if (!emptyFrom.HasValue || !emptyTo.HasValue)
//        return 1.5;
      var emptyFrom = hpTo;
      var emptyTo = (Point?)new Point(172, hpTo.Value.Y);

      var hpLen = hpTo.Value.X - hpFrom.Value.X;
      var emptyLen = emptyTo.Value.X - emptyFrom.Value.X;

      var hp = hpLen / (double) (hpLen + emptyLen);
      return hp;
    }

    private static void DrawPoint(Graphics g, Point? hpFrom)
    {
      if (hpFrom.HasValue)
        g.DrawRectangle(Pens.Blue, hpFrom.Value.X, hpFrom.Value.Y, 1, 1);
    }

    private static Point? FindTopLeft(Bitmap bmp, Point from, Point to, int tc)
    {
      var hits = 0;
      for (int y = from.Y; y < to.Y-1; y++)
      {
        for (int x = from.X; x < to.X; x++)        
        {
          var c = bmp.GetPixel(x, y);
          var argb = c.ToArgb();
          if (argb == tc)
          {
            if (++hits >= 3)
              return new Point(x - 2, y);
            else
            {
              var a = 0;
            }
          }
          else
          {
            hits = 0;
          }
        }
      }

      return null;
    }

    private static Point? FindTopRight(Bitmap bmp, Point from, Point to, int targetColor)
    {
      var hits = 0;
      for (int y = from.Y; y < to.Y; y++)
      {
        for (int x = to.X - 1; x >= @from.X; x--)        
        {
          var c = bmp.GetPixel(x, y);
          var argb = c.ToArgb();
          if (argb == targetColor)
          {
            if (++hits >= 3)
              return new Point(x + 2, y);
          }
          else
          {
            hits = 0;
          }
        }
      }

      return null;
    }

    private Bitmap memScreenShot()
    {
      Bitmap bmp = new Bitmap(500, 500);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        g.CopyFromScreen(0, 0, 0, 0, new Size(500, 500));
        
      }

      return bmp;
    }
    private void takeScreenShot()
    {
      Bitmap bmp = new Bitmap(500, 500);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        g.CopyFromScreen(0, 0, 0, 0, new Size(500, 500));
        bmp.Save("screenshot.png");  // saves the image
      }                 
    }
    
  }
}