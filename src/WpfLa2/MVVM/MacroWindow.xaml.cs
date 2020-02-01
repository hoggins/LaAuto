using System;
using System.Windows;
using System.Windows.Interop;
using AutoIt;

namespace WpfLa2.MVVM
{
  public partial class MacroWindow : Window
  {
    public MacroWindow()
    {
      InitializeComponent();
      SourceInitialized += OnSourceInitialized;
    }
    
    private void OnSourceInitialized(object sender, EventArgs e)
    {
      WindowInteropHelper windowHwnd =new WindowInteropHelper(this); 
      IntPtr hWnd = windowHwnd.Handle;
      AutoItX.WinSetOnTop(hWnd, 1);
    }
  }
}