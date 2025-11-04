using Services.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CharacterAnimantionPresenter : IInitializable, IDisposable
{
    [Inject] private IPlayerInputProvider _playerInputProvider;

    [Inject] private Animator _animator;

    private void PlayAnimtion(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("Wave");
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
