using Enums;
using Services.Interfaces;
using Services.Sound;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CharacterAnimantionPresenter : IInitializable, IDisposable
{
    [Inject] private IPlayerInputProvider _playerInputProvider;
    [Inject] private Animator _animator;
    [Inject] private SoundManager _soundManager;

    private void PlayAnimtion(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("Wave");
        _soundManager.PlaySound(volume: 0, soundType: SoundType.WaveAnimationSound);
    }

    public void Initialize()
    {
        _playerInputProvider.Inputs.Kick.performed += PlayAnimtion;
    }

    public void Dispose()
    {
        _playerInputProvider.Inputs.Kick.performed -= PlayAnimtion;
    }
}
