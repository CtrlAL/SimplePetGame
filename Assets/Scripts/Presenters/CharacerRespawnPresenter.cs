using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class CharacerRespawnPresenter : IInitializable, IDisposable
    {
        [Inject] private List<RespawnColiderView> _respawnColiderViews;

        [Inject] private PlayerSpawnPoint _playerSpawnPoint;

        private CompositeDisposable _compositeDisposable;

        public void Initialize()
        {
            _compositeDisposable = new CompositeDisposable();

            _respawnColiderViews.ForEach(x =>
            {
                x.OnPlayerFell
                .Subscribe(Respawn)
                .AddTo(_compositeDisposable);
            });
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

        public void Respawn(Collider gameObject)
        {
            var rb = gameObject.attachedRigidbody;
            rb.MovePosition(_playerSpawnPoint.transform.position);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
