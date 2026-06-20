# MVP Refactoring Plan

Based on MVP review. Each file changed **exactly once**, ordered by dependency.

## Phase 1: DI Infrastructure (new files)

| File | Action |
|------|--------|
| `IPlayerProvider.cs` | **NEW** — interface with `GameObject Instance { get; }` |
| `PlayerProvider.cs` | **NEW** — MonoBehaviour on Player root. `Container.Bind<IPlayerProvider>().To<PlayerProvider>().FromComponentOnRoot().AsSingle()`. Method: `return gameObject;` |

## Phase 2: Business logic out of Views (→ Services / Presenters)

Strip View of all business logic BEFORE deleting statics (avoids dead-reference gap).

| File | Action |
|------|--------|
| `EnemyKickZoneView.cs` | Strip to bare minimum: only `Subject<Collider> KickPerformed` + `OnTriggerEnter/Exit`. No `CancellationTokenSource`, no `UniTask`, no player reference. View doesn't filter who enters — Presenter decides |
| `EnemyKickPresenter.cs` | Move async delay + `CancellationTokenSource` here. Call `_kiker.Kick()` after delay. Ensure `Dispose` cancels token |
| `ImpactDetectorView.cs` | Remove `[Inject] CharacterFSM`, remove `is IdleState` check. Pure `OnCollisionEnter → OnImpactDetected.OnNext(force)` |
| `ImpactHandlerPresenter.cs` | Add `.Where(_ => _characterFSM.GetCurrentState() is IdleState)` to `OnImpactDetected` subscription |
| `VoidZoneObjectSpawnerView.cs` | Extract spawn logic to `VoidZoneSpawnerService`. View keeps only inspector references + calls service |
| `IVoidZoneSpawnerService.cs` | **NEW** — interface for spawn service |
| `VoidZoneSpawnerService.cs` | **NEW** — extracted logic from View (async spawn, bounds checks, VFX preview, overlap checks) |

## Phase 3: Remove statics + fix consumers (with IDisposable)

*(Safe now — no View references `PlayerInstanseHandler` anymore)*

| File | Action |
|------|--------|
| `Stuner.cs` | Absorb `StunDataStorage` logic: private `Dictionary<int, string> _storedTags` (keyed by InstanceID, not GameObject). `StoreOldTag()`, `TryGetOldTag()`, `RemoveOldTag()` → private instance methods |
| `StunDataStorage.cs` | **DELETE** — logic moved to `Stuner` |
| `PlayerInstanseHandler.cs` | **DELETE** — replaced by `IPlayerProvider` |
| `MoveEnemyPresenter.cs` | `PlayerInstanseHandler` → `IPlayerProvider`. Implement `IDisposable` + `CompositeDisposable` |
| `OutlinePresenter.cs` | `PlayerInstanseHandler` → `IPlayerProvider`. Already has `IDisposable` + `CompositeDisposable` |

## Phase 4: Memory leaks (missing IDisposable)

| File | Action |
|------|--------|
| `EndLevelPresenter.cs` | Implement `IDisposable`. Dispose `_compositeDisposable`. Remove `onClick` listeners in `Dispose()` |
| `PlayerKickPresenter.cs` | Implement `IDisposable` |
| `ThrowableInteractionModel.cs` | Implement `IDisposable`. Dispose `PickedObject` ReactiveProperty |

*Note: `MoveEnemyPresenter` already fixed in Phase 3.*

## Phase 5: Subjects belong in Model

| File | Action |
|------|--------|
| `ImpactHandlerModel.cs` | Add `Subject<Unit> OnStrongHit`, `OnWeakHit`, `OnThresholdReached`. Implement `IDisposable`. Expose as `IObservable<Unit>` |
| `ImpactHandlerPresenter.cs` | Remove `Subject<Unit>` fields. Remove `IObservable<Unit>` properties. Use `_model.OnStrongHit`, `OnWeakHit`, `OnThresholdReached` instead |

## Phase 6: Installer bindings

| File | Action |
|------|--------|
| `PlayerInstaller.cs` | Add `Container.Bind<IPlayerProvider>().To<PlayerProvider>().FromComponentOnRoot().AsSingle()` |
| `GameSceneInstaller.cs` | Add `Container.BindInterfacesTo<VoidZoneSpawnerService>().AsSingle().NonLazy()` |

## Phase 7: Renames

| File | New name |
|------|----------|
| `CharacterState .cs` (space) | `CharacterState.cs` |
| `VoidZoneObjectSpawnerView .cs` (space) | `VoidZoneObjectSpawnerView.cs` |
| `KickImpactSettigns.cs` | `KickImpactSettings.cs` |
| `CharacterAnimantionPresenter.cs` | `CharacterAnimationPresenter.cs` |
| `FSMExtentsions.cs` | `FSMExtensions.cs` |
| `RespawnColiderView.cs` | `RespawnColliderView.cs` |
| `FatigueRestorationService.cs` (in Presenters folder) | Move to `Services/Implementations/` folder |

---

## Final Folder Structure

```
Scripts/
├── Enums/
│   ├── CharacterState.cs
│   ├── EnemyType.cs
│   ├── MovementState.cs
│   ├── SoundType.cs
│   └── BackgroundSoundType.cs
├── Constants/
│   ├── CharacterTags.cs
│   ├── EnvironmentTags.cs
│   └── FatigueDamage.cs
├── Extensions/
│   ├── TagExtensions.cs
│   └── FSMExtensions.cs
│
├── Models/
│   ├── Scene/       TimerModel, StatsModel, GameOverModel
│   └── Character/   FatigueModel, ImpactHandlerModel, MoveCharacterModel, ThrowableInteractionModel
│
├── Views/
│   ├── Scene/       VoidZoneView, VoidZoneObjectSpawnerView, TimerView, PlayerSpawnPointView
│   ├── Character/   EnemyKickZoneView, ImpactDetectorView, RespawnColliderView, ThrowableInteractionView, PlayerProvider
│   └── UI/          FatigueBarView, PauseMenuView, ResultMenuView
│
├── Presenters/
│   ├── Scene/       TimerPresenter, SpawnEnemyPresenter, EndLevelPresenter, VoidZoneKillPresenter, PauseMenuPresenter
│   └── Character/   CharacterStunPresenter, CharacterDeathPresenter, CharacterAnimationPresenter,
│                    EnemyKickPresenter, EnemyAnimationPresenter, ImpactHandlerPresenter,
│                    MovePlayerPresenter, MoveEnemyPresenter, PlayerKickPresenter,
│                    ThrowableInteractionPresenter, OutlinePresenter, FatigueBarPresenter
│
├── Services/
│   ├── Interfaces/  IMover, IKicker, IPlayerInputProvider, IPlayerProvider, IVoidZoneSpawnerService,
│   │                IObjectPool, IEnemyPool, IEnemyFactory, IDeathEffectService, IDeathEffectPool,
│   │                IFatigueManager, IPoolableEnemy, IStunEffectPool, IStuner, IStunEffectService, IThrowableInteractor
│   ├── Implementations/  Mover, Kicker, PlayerInputProvider, PlayerProvider, VoidZoneSpawnerService,
│   │                     ThrowableInteractor, FatigueRestorationService, FatigueManager,
│   │                     EnemyPool, EnemyFactory, DeathEffectService, DeathEffectPool,
│   │                     StunEffectPool, StunEffectService, Stuner
│   ├── Helpers/     GroundChecker, PositionHelper
│   ├── FSM/         StateMachine, CharacterFSM, States/
│   └── Sound/       SoundManager, BackgroundMusicPlayer, Sound
│
├── Components/      PoolableEnemyReset
├── ScriptableObjects/  (15 files, incl. KickImpactSettings)
└── Installers/      GameSceneInstaller, ScriptableObjectInstaller, VoidZoneInstaller,
                     Character/  (CharacterInstaller, PlayerInstaller, EnemyInstaller)

Deleted:  PlayerInstanseHandler.cs, StunDataStorage.cs
```

---

## Status

All phases implemented. All typo files renamed. All static singletons removed.
All Views stripped of business logic. All Presenters/Models implement IDisposable.
Subjects moved to Model. New services bound in installers.
