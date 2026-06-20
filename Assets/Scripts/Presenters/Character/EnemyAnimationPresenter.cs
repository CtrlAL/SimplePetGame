using Enums;
using Extensions;
using FSM;
using Presenters;
using Services.Sound;
using System;
using UniRx;
using UnityEngine;
using Zenject;

public class EnemyAnimationPresenter : IInitializable, IDisposable
{
    [Inject] private Animator _animator;
    [Inject] private EnemyKickPresenter _enemyKickPresenter;
    [Inject] private SoundManager _soundManager;
    [Inject] private CharacterFSM _characterFSM;

    private CompositeDisposable _compositeDisposable = new();

    private void PlayAnimtion()
    {
        _animator.SetTrigger("Wave");
        _soundManager.PlaySound(volume: 0.3f, soundType: SoundType.WaveAnimationSound);
    }

    public void Initialize()
    {
        _enemyKickPresenter.OnKickPerformed
            .Where(_ => _characterFSM.IsIdleState())
            .Subscribe(_ => PlayAnimtion())
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable?.Dispose();
    }
}
