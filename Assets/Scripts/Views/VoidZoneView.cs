using UniRx;
using UnityEngine;

public class VoidZoneView : MonoBehaviour
{
    public Subject<Collider> KillPerformed = new();

    public void OnTriggerEnter(Collider other)
    {
        KillPerformed.OnNext(other);
    }
}
