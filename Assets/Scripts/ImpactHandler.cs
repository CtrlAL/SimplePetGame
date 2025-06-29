using Assets.Scripts;
using Assets.Scripts.Constants;
using Assets.Scripts.Enums;
using Assets.Scripts.FSM.States.CharacterStates;
using UnityEngine;

public class ImpactHandler : MonoBehaviour
{
    [SerializeField]
    private CharacterFSM _characterFSM;

    [SerializeField]
    private Fatigue _fatigue;

    [SerializeField]
    private float _minStrongImpact = 100f;

    [SerializeField]
    private float _minWeakImpact = 50f;

    [SerializeField]
    private int _weakHitCountNeeded = 3;

    private int _currentWeakHitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (_characterFSM.GetCurrentState() is IdleState)
        {
            float impactForce = collision.relativeVelocity.magnitude;

            if (collision.gameObject.CompareTag(EnvironmentTags.Environment))
            {
                CheckHit(impactForce);
            }
        }
    }

    private void CheckHit(float impactForce)
    {
        if (impactForce > _minStrongImpact)
        {
            _currentWeakHitCount = 0;
            _fatigue.MakeFatigueDamage(FatigueDamage.StrongHitDamage);
            _characterFSM.ChangeToState(CharacterState.Stunned);
        }
        else if (impactForce > _minWeakImpact && impactForce < _minStrongImpact)
        {
            _currentWeakHitCount++;
            _fatigue.MakeFatigueDamage(FatigueDamage.WeakHitDamage);

            if (_currentWeakHitCount >= _weakHitCountNeeded)
            {
                _currentWeakHitCount = 0;
                _characterFSM.ChangeToState(CharacterState.Stunned);
            }
        }
    }
}