using Services.EventPublishers;
using Services.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Views.Scene.Animation
{
    public class CharacterAnimatior : MonoBehaviour
    {
        [Inject] 
        private IPlayerInputProvider _playerInputProvider;

        [SerializeField] private Animator _animator;

        private string _boolName = "Wave";

        public void Awake()
        {
            _playerInputProvider.Inputs.Kick.performed += PlayAnimtion;
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
            _playerInputProvider.Inputs.Kick.performed -= PlayAnimtion;
            AnimationEventPublisher.Instance.WaveAnimationEnded -= EndAnimation;
        }
    }
}