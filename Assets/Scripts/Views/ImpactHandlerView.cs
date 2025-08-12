using FSM;
using FSM.States.CharacterStates;
using UniRx;
using UnityEngine;
using Zenject;

namespace Views.Scene.Characters
{
    public class ImpactHandlerView : MonoBehaviour
    {
        [Inject] private CharacterFSM _characterFSM;

        public readonly Subject<float> OnImpactDetected = new();

        private void OnCollisionEnter(Collision collision)
        {
            if (_characterFSM.GetCurrentState() is IdleState)
            {
                float impactForce = collision.relativeVelocity.magnitude;
                OnImpactDetected.OnNext(impactForce);
            }
        }
    }
}