using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.FSM.States.CharacterStates
{
    public class IdleState : IState
    {
        public void Enter()
        {
        }

        public void Update()
        {
        }

        public void Exit()
        {
            Debug.Log("Покинул состояние: Stunned");
        }
    }
}
