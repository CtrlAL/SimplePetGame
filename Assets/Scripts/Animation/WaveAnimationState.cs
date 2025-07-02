using UnityEngine;

public class WaveAnimationState : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimationEventPublisher.Instance.PublisWaveAnimationEnded();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimationEventPublisher.Instance.PublisWaveAnimationStarted();
    }
}
