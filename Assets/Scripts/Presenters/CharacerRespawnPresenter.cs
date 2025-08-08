using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class CharacerRespawnPresenter : IInitializable, IDisposable
    {
        [Inject]
        private RespawnColiderView _respawnColiderView;

        [Inject]
        private PlayerSpawnPoint _playerSpawnPoint;

        private CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _respawnColiderView.OnPlayerFell.Subscribe(co => Respawn(co))
                .AddTo(_compositeDisposable);
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
