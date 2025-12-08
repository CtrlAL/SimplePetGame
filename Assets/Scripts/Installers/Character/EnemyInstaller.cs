using Presenters;
using ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Services.Installers
{
    public class EnemyInstaller : CharacterInstaller
    {
        [SerializeField] private EnemyType _enemyType;

        private readonly Dictionary<EnemyType, Type> _statsMapping = new()
        {
            { EnemyType.Default, typeof(EnemyStats) },
            { EnemyType.Big, typeof(BigEnemyStats) }
        };

        public override void InstallBindings()
        {
            BindStats();
            BindAnimator();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<EnemyKickZoneView>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterAnimantionPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyKickPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MoveEnemyPresenter>().AsSingle().NonLazy();

            base.InstallBindings();
        }

        private void BindStats()
        {
            if (_statsMapping.TryGetValue(_enemyType, out var statsType))
            {
                Container.Bind<AbstractStats>().To(statsType).AsSingle();
            }
            else
            {
                throw new InvalidOperationException($"Unsupported enemy type: {_enemyType}");
            }
        }

        private void BindAnimator()
        {
            Container.Bind<Animator>().FromInstance(gameObject.GetComponentInChildren<Animator>()).AsSingle();
        }

        private enum EnemyType
        {
            Default,
            Big
        }
    }
}