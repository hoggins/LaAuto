using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using AutoIt;
using TextVision;

namespace WpfLa2
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {

      InitializeComponent();

      //takeScreenShot();
      //Class1.ConvertImageToText(null);

SourceInitialized += OnSourceInitialized;
//      AutoItX.MouseClick("LEFT", 680, 18);
//      AutoItX.Send("2");

      //Draw(Brushes.Red);


//      AutoItX.Run("notepad.exe");
//      AutoItX.WinWaitActive("Untitled");
//      AutoItX.Send("I'm in notepad");

      //IntPtr winHandle = AutoItX.WinGetHandle("Untitled");
//      AutoItX.WinKill(winHandle);
    }

    private void OnSourceInitialized(object sender, EventArgs e)
    {
      WindowInteropHelper windowHwnd =new WindowInteropHelper(this); 
      IntPtr hWnd = windowHwnd.Handle;
      AutoItX.WinSetOnTop(hWnd, 1);

      
      //takeScreenShot();

      
      
//      Class1.ConvertImageToText(null);
      //throw new Exception();
    }


    
    
  }
}