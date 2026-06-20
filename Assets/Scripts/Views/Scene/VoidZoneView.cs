using UniRx;
using UnityEngine;

public class VoidZoneView : MonoBehaviour
{
    private readonly Subject<Collider> _killPerformed = new();
    public IObservable<Collider> KillPerformed => _killPerformed;

    public void OnTriggerEnter(Collider other)
    {
        _killPerformed.OnNext(other);
    }

    private void OnDestroy()
    {
        _killPerformed?.Dispose();
    }
}
