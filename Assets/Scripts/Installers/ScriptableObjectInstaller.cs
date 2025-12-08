using ScriptableObjects;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableObjectInstaller", menuName = "Installers/ScriptableObjectInstaller")]
public class ScriptableObjectInstaller : ScriptableObjectInstaller<ScriptableObjectInstaller>
{
    [SerializeField] private BackgroundSounds _backgroundSounds;

    [SerializeField] private CharacterSounds _characterSounds;

    [SerializeField] private LevelSettings _levelSettings;

    [SerializeField] private PoolingSettings _poolingSettings;

    [SerializeField] private PlayerStats _playerStatsSO;

    [SerializeField] private EnemyStats _enemyStatsSO;

    [SerializeField] private BigEnemyStats _bigEnemyStatsSO;

    [SerializeField] private InteractionSettings _throwableInteractionSettingsSO;

    [SerializeField] private KickImpactSettigns _kickImpactSettigns;

    [SerializeField] private CharacterVFX _characterVFX;

    [SerializeField] private EnemyVariants _enemyLibrary;

    public override void InstallBindings()
    {
        Container.BindInstance(_bigEnemyStatsSO);
        Container.BindInstance(_backgroundSounds);
        Container.BindInstance(_characterSounds);
        Container.BindInstance(_levelSettings);
        Container.BindInstance(_poolingSettings);
        Container.BindInstance(_playerStatsSO);
        Container.BindInstance(_enemyStatsSO);
        Container.BindInstance(_throwableInteractionSettingsSO);
        Container.BindInstance(_kickImpactSettigns);
        Container.BindInstance(_characterVFX);
        Container.BindInstance(_enemyLibrary);
    }
}