using Assets.Scripts;
using Assets.Scripts.Constants;
using Assets.Scripts.Enums;
using Assets.Scripts.FSM.States.CharacterStates;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField]
    private CharacterFSM _characterFSM;

    [SerializeField] 
    private float _strongImpactThreshold = 5f;

    [SerializeField] 
    private int _weakHitCountNeeded = 3;

    private int _currentWeakHitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (_characterFSM.GetCurrentState() is IdleState)
        {
            float impactForce = collision.relativeVelocity.magnitude;

            if (collision.gameObject.CompareTag(EnvironmentTags.Environment) || 
                collision.gameObject.CompareTag(EnvironmentTags.Environment))
            {
                CheckHit(impactForce);
            }
        }
    }

    private void CheckHit(float impactForce)
    {
        if (impactForce > _strongImpactThreshold)
        {
            _currentWeakHitCount = 0;
            _characterFSM.ChangeToState(CharacterState.Stuned);
        }
        else
        {
            _currentWeakHitCount++;

            if (_currentWeakHitCount >= _weakHitCountNeeded)
            {
                _currentWeakHitCount = 0;
                _characterFSM.ChangeToState(CharacterState.Stuned);
            }
        }
    }
}
