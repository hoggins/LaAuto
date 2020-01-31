using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WpfLa2
{
  public class MacroWndVm : ViewModelBase
  {
    public class MacroLauncher
    {
      public string Title { get; set; }
      public ICommand Start { get; }

      public MacroLauncher(string title, Action startMacro)
      {
        Title = title;
        Start = new CommandHandler(startMacro, () => true);
      }
    }

    public class MacroWrapper
    {
      public IMacroVm Macro { get; set; }
      public ICommand Stop { get; }

      public MacroWrapper(IMacroVm macro, Action<MacroWrapper> stopMacro)
      {
        Macro = macro;
        Stop = new CommandHandler(()=>stopMacro(this), () => true);;
      }
    }

    public ObservableCollection<MacroLauncher> MacroLaunchers { get; } = new ObservableCollection<MacroLauncher>();
    public ObservableCollection<MacroWrapper> ActiveMacro { get; } = new ObservableCollection<MacroWrapper>();

    public MacroWndVm()
    {
      MacroLaunchers.Add(new MacroLauncher("Test watch window", StartMacro<WatchWndMacro>));
      MacroLaunchers.Add(new MacroLauncher("Heal", StartMacro<HealMacro>));
      MacroLaunchers.Add(new MacroLauncher("Assist", StartMacro<AssistMacro>));
    }

    private void StartMacro<T>() where T : IMacroVm, new()
    {
      
      var macro = new T();
      macro.Start();
      var w = new MacroWrapper(macro, StopMacro);
      ActiveMacro.Add(w);
    }

    private void StopMacro(MacroWrapper wrapper)
    {
      if (ActiveMacro.Remove(wrapper))
        wrapper.Macro?.Stop();
      else 
        ; // todo log error
    }
  }
}