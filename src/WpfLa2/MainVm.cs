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

    #region VM

    private string _color;
    public string Color
    {
      get => _color;
      set => SetProperty(ref _color, value);
    }
    
    private string _message1;
    public string Message1
    {
      get => _message1;
      set => SetProperty(ref _message1, value);
    }
    
    private string _message2;
    public string Message2
    {
      get => _message2;
      set => SetProperty(ref _message2, value);
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
    
    private ICommand _startAssistCommand;
    public ICommand StartAssistCommand
    {
      get
      {
        return _startAssistCommand ?? (_startAssistCommand = new CommandHandler(() => StartAssist(), ()=> true));
      }
    }

    #endregion
    
    private MacroBase _iisMacro;
    private MacroBase _assistMacro;
    
    private int WarnCount;

    private async void StartMacro()
    {
      _iisMacro?.Dispose();
      _iisMacro = new HealMacro();
      _iisMacro.Start();
    }

    
    private async void StartAssist()
    {
      _assistMacro?.Dispose();
      _assistMacro = new AssistMacro();
      _assistMacro.Start();
    }
    
    
    private async void MarkupLaWindow()
    {
      _iisMacro?.Dispose();
      _iisMacro = new WatchWndMacro();
      _iisMacro.Start();
      return;
      
        
      var sw = new Stopwatch();
      var target = await MacroModel.GetClientObserve();
      using (var snapshot = new LaWndSnapshot(target))
      {
        SetMessage("p: " + snapshot.GetPartyHp()
                   + " t: " + snapshot.GetTargetHp());
      }
    }

    public static void SetMessage(string text)
    {
      if (Instance == null)
        return;
      Instance.Message1 = text;
    }

    public static void SetMessage2(string text)
    {
      if (Instance == null)
        return;
      Instance.Message2 = text;
    }
    
    public static void SetWarn(bool w)
    {
      if (Instance == null)
        return;
      if (w)
        Interlocked.Increment(ref Instance.WarnCount);
      else
        Interlocked.Decrement(ref Instance.WarnCount);
      Instance.Color = Instance.WarnCount > 0 ? "Red" : "White";
    }
  }
}