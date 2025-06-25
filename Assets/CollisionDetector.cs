using Assets.Scripts;
using Assets.Scripts.Enums;
using Assets.Scripts.FSM.States.CharacterStates;
using System.Collections;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] 
    private GameObject _stunnedIcon;

    [SerializeField]
    private CharacterFSM _characterFSM;

    [SerializeField]
    private int _maxHitCount = 0;

    private int _currentHitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (_characterFSM.GetCurrentState() is IdleState)
        {
            if (collision.gameObject.CompareTag("Environment"))
            {
                _characterFSM.ChangeToState(CharacterState.Stuned);
            }

            else if (collision.gameObject.CompareTag("Throwable"))
            {
                _currentHitCount++;

                if (_currentHitCount == _maxHitCount)
                {
                    _characterFSM.ChangeToState(CharacterState.Stuned);
                }
            }
        }
    }
}
