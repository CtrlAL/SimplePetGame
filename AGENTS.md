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

- **Prefer interfaces over abstract classes**: when creating multiple implementations of a contract (`IMover` → `PlayerMover`/`EnemyMover`, etc.), write standalone classes that implement the interface. Do NOT share code via an abstract base class. Minor duplication across implementations is acceptable and preferred over inheritance.
- **Character hierarchy**: `CharacterInstaller` (base, assumes `BoxCollider` on root) → `PlayerInstaller` / `EnemyInstaller`. Both call `base.InstallBindings()` last. Subclass installers bind their own `IMover` (`PlayerMover` / `EnemyMover`) **before** calling `base.InstallBindings()`; the base class does NOT bind `IMover`.
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


# Context Mode

# context-mode — MANDATORY routing rules

context-mode MCP tools available. Rules protect context window from flooding. One unrouted command dumps 56 KB into context.

## Think in Code — MANDATORY

Analyze/count/filter/compare/search/parse/transform data: **write code** via `context-mode_ctx_execute(language, code)`, `console.log()` only the answer. Do NOT read raw data into context. PROGRAM the analysis, not COMPUTE it. Pure JavaScript — Node.js built-ins only (`fs`, `path`, `child_process`). `try/catch`, handle `null`/`undefined`. One script replaces ten tool calls.

## BLOCKED — do NOT attempt

### curl / wget — BLOCKED
Shell `curl`/`wget` intercepted and blocked. Do NOT retry.
Use: `context-mode_ctx_fetch_and_index(url, source)` or `context-mode_ctx_execute(language: "javascript", code: "const r = await fetch(...)")`

### Inline HTTP — BLOCKED
`fetch('http`, `requests.get(`, `requests.post(`, `http.get(`, `http.request(` — intercepted. Do NOT retry.
Use: `context-mode_ctx_execute(language, code)` — only stdout enters context

### Direct web fetching — BLOCKED
Use: `context-mode_ctx_fetch_and_index(url, source)` then `context-mode_ctx_search(queries)`

## REDIRECTED — use sandbox

### Shell (>20 lines output)
Shell ONLY for: `git`, `mkdir`, `rm`, `mv`, `cd`, `ls`, `npm install`, `pip install`.
Otherwise: `context-mode_ctx_batch_execute(commands, queries)` or `context-mode_ctx_execute(language: "javascript", code: "...")`. Use `language: "shell"` only when code matches the host shell.

### File reading (for analysis)
Reading to **edit** → reading correct. Reading to **analyze/explore/summarize** → `context-mode_ctx_execute_file(path, language, code)`.

### grep / search (large results)
Use `context-mode_ctx_execute(language: "javascript", code: "...")` in sandbox for portable filtering/counting.

## Tool selection

0. **MEMORY**: `context-mode_ctx_search(sort: "timeline")` — after resume, check prior context before asking user.
1. **GATHER**: `context-mode_ctx_batch_execute(commands, queries)` — runs all commands, auto-indexes, returns search. ONE call replaces 30+. Each command: `{label: "header", command: "..."}`.
2. **FOLLOW-UP**: `context-mode_ctx_search(queries: ["q1", "q2", ...])` — all questions as array, ONE call (default relevance mode).
3. **PROCESSING**: `context-mode_ctx_execute(language, code)` | `context-mode_ctx_execute_file(path, language, code)` — sandbox, only stdout enters context.
4. **WEB**: `context-mode_ctx_fetch_and_index(url, source)` then `context-mode_ctx_search(queries)` — raw HTML never enters context.
5. **INDEX**: `context-mode_ctx_index(content, source)` — store in FTS5 for later search.

## Parallel I/O batches

For multi-URL fetches or multi-API calls, **always** include `concurrency: N` (1-8):

- `context-mode_ctx_batch_execute(commands: [3+ network commands], concurrency: 5)` — gh, curl, dig, docker inspect, multi-region cloud queries
- `context-mode_ctx_fetch_and_index(requests: [{url, source}, ...], concurrency: 5)` — multi-URL batch fetch

**Use concurrency 4-8** for I/O-bound work (network calls, API queries). **Keep concurrency 1** for CPU-bound (npm test, build, lint) or commands sharing state (ports, lock files, same-repo writes).

GitHub API rate-limit: cap at 4 for `gh` calls.

## Output

Write artifacts to FILES — never inline. Return: file path + 1-line description.
Descriptive source labels for `search(source: "label")`.

## Session Continuity

Skills, roles, and decisions persist for the entire session. Do not abandon them as the conversation grows.

## Memory

Session history is persistent and searchable. On resume, search BEFORE asking the user:

| Need | Command |
|------|---------|
| What did we decide? | `context-mode_ctx_search(queries: ["decision"], source: "decision", sort: "timeline")` |
| What constraints exist? | `context-mode_ctx_search(queries: ["constraint"], source: "constraint")` |

DO NOT ask "what were we working on?" — SEARCH FIRST.
If search returns 0 results, proceed as a fresh session.

## ctx commands

| Command | Action |
|---------|--------|
| `ctx stats` | Call `stats` MCP tool, display full output verbatim |
| `ctx doctor` | Call `doctor` MCP tool, run returned shell command, display as checklist |
| `ctx upgrade` | Call `upgrade` MCP tool, run returned shell command, display as checklist |
| `ctx purge` | Call `purge` MCP tool with confirm: true. Warns before wiping knowledge base. |

After /clear or /compact: knowledge base and session stats preserved. Use `ctx purge` to start fresh.

<!-- CODEGRAPH_START -->
## CodeGraph

In repositories indexed by CodeGraph (a `.codegraph/` directory exists at the repo root), reach for it BEFORE grep/find or reading files when you need to understand or locate code:

- **MCP tool** (when available): `codegraph_explore` answers most code questions in one call — the relevant symbols' verbatim source plus the call paths between them, including dynamic-dispatch hops grep can't follow. Name a file or symbol in the query to read its current line-numbered source. If it's listed but deferred, load it by name via tool search.
- **Shell** (always works): `codegraph explore "<symbol names or question>"` prints the same output.

If there is no `.codegraph/` directory, skip CodeGraph entirely — indexing is the user's decision.
<!-- CODEGRAPH_END -->


