using Enums;
using FSM;
using Models;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Components
{
    public class PoolableEnemyReset : MonoBehaviour, IPoolableEnemy
    {
        [Inject] private CharacterFSM _fsm;

        [Inject] private FatigueModel _fatigueModel;

        [Inject] private ImpactHandlerModel _impactHandlerModel;

        [Inject] private Animator _animator;

        public void ResetState()
        {
            _fatigueModel.CurrentFatigue.Value = 0;
            _impactHandlerModel.CurrentWeakHitCount.Value = 0;
            _fsm.ChangeToState(CharacterState.Idle);
            ResetAnimator();
        }

        private void ResetAnimator()
        {
            if (_animator == null) return;

            foreach (var param in _animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger)
                    _animator.ResetTrigger(param.nameHash);
            }

            _animator.Play(0, 0, 0f);
            _animator.Update(0f);
        }
    }
}
