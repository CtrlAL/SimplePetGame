using Services.EventPublishers;
using UnityEngine;
using Zenject;

public class WaveAnimationState : StateMachineBehaviour
{
    [Inject] private AnimationEventPublisher _animationEventPublisher;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animationEventPublisher.PublisWaveAnimationEnded();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animationEventPublisher.PublisWaveAnimationStarted();
    }
}
