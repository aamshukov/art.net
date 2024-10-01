//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DesignPatterns.Observer;

public abstract class Observer<T> : IObserver<T>
{
    public IDisposable? Disposable { get; set; }

    public Observer()
    {
    }

    public virtual void Subscribe(IObservable<T> observable)
    {
        Assert.NonNullReference(observable, nameof(observable));
        Disposable = observable.Subscribe(this);
    }

    public virtual void Unsubscribe()
    {
        Disposable?.Dispose();
    }

    public abstract void OnNext(T value);

    public abstract void OnCompleted();

    public abstract void OnError(Exception ex);
}
