using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfLa2.Macro;

namespace WpfLa2.MVVM
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

    public bool IsCreateVisible
    {
      get => _isCreateVisible;
      set => SetProperty(ref _isCreateVisible, value);
    }

    public ICommand ToggleCreate { get; }

    private bool _isCreateVisible;

    public MacroWndVm()
    {
      ToggleCreate = new CommandHandler(()=> IsCreateVisible = !IsCreateVisible, () => true);

      MacroLaunchers.Add(new MacroLauncher("Test watch window", StartMacro<WatchWndMacro>));
      MacroLaunchers.Add(new MacroLauncher("Heal", StartMacro<HealMacro>));
      MacroLaunchers.Add(new MacroLauncher("Assist", StartMacro<AssistMacro>));
      MacroLaunchers.Add(new MacroLauncher("Spam button", StartMacro<SpamButtonMacro>));
    }

    private void StartMacro<T>() where T : IMacroVm, new()
    {
      IsCreateVisible = false;

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