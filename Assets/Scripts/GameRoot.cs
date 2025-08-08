using Services;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace GameRoot
{
    public class GameRoot : MonoBehaviour
    {
        [Header("Input Consumers")]
        [Inject] private IKiker _kiker;
        [Inject] private IMover _mover;

        [Header("Effects")]
        [Inject]
        private DeathEffectPresenter _deathEffectPresenter;
        [Inject]
        private StunEffectPresenter _stunEffectPresenter;
    }
}


