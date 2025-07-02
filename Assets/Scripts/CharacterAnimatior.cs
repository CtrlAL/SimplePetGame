using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class CharacterAnimatior : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private string _boolName = "Wave";

        public void Awake()
        {
            PlayerInputProvider.Inputs.Inputs.Kick.performed += PlayAnimtion;
            AnimationEventPublisher.Instance.WaveAnimationEnded += EndAnimation;
        }

        private void EndAnimation(object sender, EventArgs e)
        {
            _animator.SetBool(_boolName, false);
        }

        private void PlayAnimtion(InputAction.CallbackContext context)
        {
            _animator.SetBool(_boolName, true);
        }

        private void OnDestroy()
        {
            PlayerInputProvider.Inputs.Inputs.Kick.performed -= PlayAnimtion;
            AnimationEventPublisher.Instance.WaveAnimationEnded -= EndAnimation;
        }
    }
}

