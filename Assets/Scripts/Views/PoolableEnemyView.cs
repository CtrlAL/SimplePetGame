using Enums;
using FSM;
using Models;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Views
{
    public class PoolableEnemyView : MonoBehaviour, IPoolableEnemy
    {
        [Inject] private CharacterFSM _fsm;

        [Inject] private FatigueModel _fatigueModel;

        [Inject] private ImpactHandlerModel _impactHandlerModel;

        public void ResetState()
        {
            _fatigueModel.CurrentFatigue.Value = 0;
            _impactHandlerModel.CurrentWeakHitCount.Value = 0;
            _fsm.ChangeToState(CharacterState.Idle);
        }
    }
}
