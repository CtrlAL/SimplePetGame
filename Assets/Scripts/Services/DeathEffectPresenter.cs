using Assets.Scripts;
using System;
using UnityEngine;

public class DeathEffectPresenter : IDisposable
{
    private ParticleSystem _particleSystem;
    private DestroyEnemyEventPublisher _destroyEnemyEventPublisher;

    public DeathEffectPresenter()
    {
        _particleSystem = Resources.Load<ParticleSystem>("Prefubs/UI/DeathEffect");

        _destroyEnemyEventPublisher = DestroyEnemyEventPublisher.Instance;

        _destroyEnemyEventPublisher.DestroyEnemy += ShowEffect;
    }

    public void Dispose()
    {
        _destroyEnemyEventPublisher.DestroyEnemy -= ShowEffect;
    }

    private void ShowEffect(object sender, GameObject e)
    {
        var effect = GameObject.Instantiate(_particleSystem.gameObject, e.transform);
        //effect.GetComponent<ParticleSystem>().Play();
    }
}
