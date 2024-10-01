//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Diagnostics;

namespace UILab.Art.Framework.Core.DesignPatterns.Observer;

public abstract class Observable<T> : IObservable<T>
{
    private List<IObserver<T>> Observers { get; init; }

    private class Unsubscriber : Disposable
    {
        private List<IObserver<T>> Observers { get; init; }

        private IObserver<T> Observer { get; init; }

        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            Assert.NonNullReference(observers, nameof(observers));
            Assert.NonNullReference(observer, nameof(observer));

            Observers = observers;
            Observer = observer;
        }

        protected override void DisposeManagedResources()
        {
            if(Observer is not null)
            {
                Observers.Remove(Observer);
            }
        }

        protected override async ValueTask DisposeManagedResourcesAsync()
        {
            DisposeManagedResources();
            await Task.CompletedTask;
        }

        protected override void DisposeUnmanagedResources()
        {
        }

        protected override async ValueTask DisposeUnmanagedResourcesAsync()
        {
            await Task.CompletedTask;
        }
    }

    public Observable()
    {
        Observers = new();
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        Assert.NonNullReference(observer, nameof(observer));

        if(!Observers.Contains(observer))
        {
            Observers.Add(observer);
        }

        return new Unsubscriber(Observers, observer);
    }
}
