using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace WpfLa2
{
  public abstract class MacroBase : IMacroVm, INotifyPropertyChanged, IDisposable
  {
    public event PropertyChangedEventHandler PropertyChanged;
    
    public abstract string Title { get; }
    public virtual string DataTemplate => "MacroBaseDt";
    
    public string Status
    {
      get => _status;
      set => SetProperty(ref _status, value);
    }
    public string Color
    {
      get => _color;
      set => SetProperty(ref _color, value);
    }
    
    protected CancellationToken Ct => _cts.Token;
    private readonly CancellationTokenSource _cts = new CancellationTokenSource();
    
    private string _status;
    private string _color;
    private int _warnCount;

    public void Start()
    {
      Task.Run(async () => await Initialize(), _cts.Token)
        .ContinueWith((ot) =>
        {
          if (!ot.IsCanceled)
            Run();
        }, _cts.Token);
    }

    public void Stop()
    {
      _cts.Cancel();
    }

    public void Dispose()
    {
      _cts.Dispose();
    }

    protected abstract Task Initialize();

    protected abstract Task Run();
    
    protected void SetWarn(bool w)
    {
      if (w)
        Interlocked.Increment(ref _warnCount);
      else
        Interlocked.Decrement(ref _warnCount);
      Color = _warnCount > 0 ? "Red" : "White";
    }

    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
    {
      if(!EqualityComparer<T>.Default.Equals(field, newValue))
      {
        field = newValue;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
      }
      return false;
    }
  }
}