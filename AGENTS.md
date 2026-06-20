# SimplePetGame

Unity **2022.3.62f3** — URP, single-scene build (`FirstScene.unity`, `NavMeshTest.unity` exists but not in build).

## Architecture

**MVP(P) + Zenject DI** — Models hold reactive state (UniRx `ReactiveProperty<>`), Views are dumb MonoBehaviours, Presenters wire them together, Services contain logic.

| Folder | Role |
|--------|------|
| `Scripts/Models/` | State with `ReactiveProperty<>`, implement `IDisposable` |
| `Scripts/Views/` | `MonoBehaviour`, `[SerializeField]` references to Unity components |
| `Scripts/Presenters/` | Subscribe to Models, update Views. Often implement `IInitializable`, `IFixedTickable`, `IDisposable` |
| `Scripts/Services/` | Logic behind interfaces (`IMover`, `IKiker`, etc.) |
| `Scripts/Installers/` | `MonoInstaller` subclasses. Bind via `Container.Bind<>().To<>().AsSingle()` |
| `Scripts/ScriptableObjects/` | Config assets (stats, settings, VFX, sound libraries). Bound in `ScriptableObjectInstaller` |
| `Scripts/Enums/` | Shared enums (`CharacterState`, `MovementState`, etc.) |
| `Scripts/Extensions/` | `CompareTag()` helper extensions using `Constants.*` classes |
| `Scripts/Constants/` | Tag strings (`CharacterTags`, `EnvironmentTags`) in `ScriptableObjects/Constants/` |

## Key dependencies

- **Zenject** — all DI, scene + scriptable object installers; bindings use `Container.Bind<T>().To<TImpl>().AsSingle()` and `Container.BindInterfacesAndSelfTo<T>().AsSingle()`
- **UniRx** — `ReactiveProperty<>`, `Subject<>`, `CompositeDisposable` for data flow
- **UniTask** — async/await (`Cysharp.Threading.Tasks`)
- **FMOD** — audio (via `Assets/Plugins/FMOD/`)
- **Input System** — `PlayerInputActions` auto-generated from `InputAsset/PlayerInputActions.inputactions`
- **AI Navigation** — `NavMeshAgent` on enemies
- **QuickOutline** — outline effect for throwable objects
- **glTFast** — `com.unity.cloud.gltfast` for glTF asset import

## Patterns

- **Character hierarchy**: `CharacterInstaller` (base, assumes `BoxCollider` on root) → `PlayerInstaller` / `EnemyInstaller`. Both call `base.InstallBindings()` last.
- **FSM**: `StateMachine` + `CharacterFSM` manages `IState` implementations (`IdleState`, `StunnedState`)
- **Object pools**: `EnemyPool`, `DeathEffectPool`, `StunEffectPool` — Zenject-singletons implementing `IObjectPool<T>`
- **Tags**: Use `CompareTag()` with `CharacterTags.*` / `EnvironmentTags.*` constants (not raw strings)
- **Collision detection**: `TagExtensions` (`IsPlayer()`, `IsEnemy()`, `IsThrowable()`)
- **Sound**: `SoundManager.PlaySound(volume, SoundType)` driven by `SoundType` enum
- **Scene loading**: `FirstScene.unity` is the only build scene
- **Bind config via** `ScriptableObjectInstaller` (ScriptableObject installer) — add new fields + `Container.BindInstance()` there

## Quirks & gotchas

- **Two injection styles coexist**: most classes use `[Inject]` field injection; a few (`CharacterStunPresenter`, `ThrowableInteractionPresenter`, `EnemyFactory`, `StunEffectService`, `StunEffectPool`, `DeathEffectService`) use **constructor injection**. Match the file's existing style.
- **`IPlayerMovementInputHandler`** is declared but has no implementation — dead code.
- **`PlayerInstanseHandler`** is a static singleton (anti-pattern in DI project), used by `EnemyKickZoneView` and `OutlinePresenter` to find the player GameObject.
- **Namespaces are inconsistent**: some files use no namespace (`TimerModel`, `CharacterAnimantionPresenter`, `VoidZoneView`, `VoidZoneObjectSpawnerView`), most use root namespaces like `Enums`, `Models`, `Presenters`, `Services`, `Services.Sound`, etc. Check the file's namespace before adding new code.
- **Typo filenames** (don't fix, but be aware for grep): `FSMExtentsions.cs`, `CharacterState .cs` (space), `VoidZoneObjectSpawnerView .cs` (space), `KickImpactSettigns.cs`, `CharacterAnimantionPresenter.cs`, `IKiker.cs`/`Kicker.cs` (inconsistent), `PlayerInstanseHandler.cs`, `FatigueRestorationService.cs` (in `Presenters` namespace, not `Services`).
- **`EnemyInstaller`** defines its own private `EnemyType` enum (`Default`/`Big`) that shadows the global `Enums.EnemyType` (`Small`).
- **`EnemyType.cs`** uses `Assets.Scripts.Enums` (fully qualified) — all other enums use just `Enums`.

## Commands

No project-specific commands. Standard Unity workflow:
- Open `FirstScene.unity`, enter Play Mode
- Build targets: Standalone Windows, Android, WebGL (Burst enabled on all)
- Test framework (`com.unity.test-framework 1.1.33`) is available in packages but no project tests exist.
