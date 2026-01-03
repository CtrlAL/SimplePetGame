using Enums;
using Presenters;
using Services.Sound;
using System;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

public class EnemyAnimationPresenter : IInitializable, IDisposable
{
    [Inject] private Animator _animator;
    [Inject] private EnemyKickZoneView _enemyKickZoneView;
    [Inject] private SoundManager _soundManager;

    private CompositeDisposable _compositeDisposable = new();

    private void PlayAnimtion()
    {
        _animator.SetTrigger("Wave");
        _soundManager.PlaySound(volume: 0.3f, soundType: SoundType.WaveAnimationSound);
    }

    public void Initialize()
    {
        _enemyKickZoneView.KickPerformed.Subscribe(c =>
        {
            PlayAnimtion();
        })
        .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable?.Dispose();
    }
}
