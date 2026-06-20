using FSM;
using FSM.States.CharacterStates;

namespace Extensions
{
    public static class FSMExtensions
    {
        public static bool IsIdleState(this CharacterFSM characterFSM)
        {
            return characterFSM.GetCurrentState() is IdleState;
        }
    }
}
