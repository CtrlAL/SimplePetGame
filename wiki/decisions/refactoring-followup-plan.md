---
tags:
  - decisions
  - plan
  - refactoring
  - followup
---

# Refactoring Followup Plan

План по устранению проблем, выявленных при code review после первичного рефакторинга (см. [[Refactoring Plan|первоначальный план]]). Реализовывать **строго сверху вниз** — каждая группа зависит от предыдущей.

Закрывайте пункты галочками по мере выполнения:

```markdown
- [ ] — не выполнено
- [x] — выполнено
```

---

## 🔴 Phase A — Критические баги (блокируют Play Mode)

Без этих правок проект не запускается или работает некорректно.

### A.1 Закоммитить незакоммиченные правки

- [ ] Закоммитить:
  - `Assets/Scripts/Installers/GameSceneInstaller.cs` (+ PlayerProvider binding, + VoidZoneObjectSpawnerView binding)
  - `Assets/Scripts/Installers/Character/PlayerInstaller.cs` (− IPlayerProvider binding)
  - `Assets/Scripts/Services/Implementations/PlayerProvider.cs` (новый plain C# класс)
  - удаление `Assets/Scripts/Views/Character/PlayerProvider.cs` (+ .meta)

**Причина:** HEAD commit всё ещё ссылается на удалённый MonoBehaviour PlayerProvider → `git pull` ломает сборку.

### A.2 Восстановить фильтр IsPlayer() в EnemyKickPresenter

- [ ] В `EnemyKickPresenter.cs` добавить проверку `if (!other.IsPlayer()) return;` в `OnPlayerEntered(Collider)`
- [ ] Проверить `OnPlayerExited` — должен фильтровать тоже (или View должен передавать collider)
- [ ] Протестировать в Play Mode: враг рядом с throwables/другими врагами не должен их кикать

**Причина:** При strip View (Phase 2) фильтр вырезали, но в Presenter не перенесли. Сейчас враг кикает всё подряд.

**Файл:** `Assets/Scripts/Presenters/Character/EnemyKickPresenter.cs:47-54`

### A.3 Re-check IsIdleState после delay в EnemyKickPresenter

- [ ] В `StartKickDelayed`, после `await UniTask.Delay(...)` добавить: `if (other == null || !_fsm.IsIdleState()) return;`
- [ ] Протестировать: оглушить врага во время delay → кик не должен сработать

**Причина:** Сейчас проверка только перед delay → stunned-враг продолжает кикать.

**Файл:** `Assets/Scripts/Presenters/Character/EnemyKickPresenter.cs:63-78`

---

## 🟡 Phase B — Важные баги (ломают геймплей)

### B.1 Сброс Time.timeScale при Restart

- [ ] В `EndLevelPresenter.Restart()` добавить `Time.timeScale = 1;` перед `SceneManager.LoadScene()`
- [ ] Дополнительно: добавить `Time.timeScale = 1;` в `Dispose()` как страховку
- [ ] Протестировать: Game Over → Restart → игра не заморожена

**Причина:** `ShowResultView` ставит `timeScale = 0`, но нигде не сбрасывается. После рестарта сцена зависает.

**Файл:** `Assets/Scripts/Presenters/Scene/EndLevelPresenter.cs:60`

### B.2 Заменить EnemyStats на AbstractStats в Enemy презентерах

- [ ] `EnemyKickPresenter.cs:20` — `[Inject] private AbstractStats _stats;` (вместо `EnemyStats`)
- [ ] `MoveEnemyPresenter.cs:17` — то же самое
- [ ] Если где-то используется `EnemyStats.DelayBeforeKick` — переместить поле в `AbstractStats` или `EnemyStats` через приведение типов с проверкой
- [ ] Протестировать: BigEnemy префаб спавнится и работает

**Причина:** `EnemyInstaller` биндит `AbstractStats → EnemyStats | BigEnemyStats` полиморфно. Конкретный `[Inject] EnemyStats` ломается для BigEnemy. Zenject падает.

**Файлы:** `EnemyKickPresenter.cs:20`, `MoveEnemyPresenter.cs:17`

### B.3 Исправить инкремент/декремент в VoidZoneSpawnerService

- [ ] В `TrySpawnAsync`: инкрементировать `_currentCount++` **сразу после успешного спавна** (до `DestroyAfterDelay`)
- [ ] В конце lifecycle (после `DestroyAfterDelay`): `_currentCount--`
- [ ] Протестировать: при `SpawnCount = 3` одновременно не больше 3 void zones

**Причина:** Текущая логика декрементирует в конце → cap не работает → бесконечный спавн. Bug pre-existing, но скопирован при экстракции.

**Файл:** `Assets/Scripts/Services/Implementations/VoidZoneSpawnerService.cs:67-73`

### B.4 Добавить OnDestroy disposal в Views с Subject

- [ ] `ImpactDetectorView.cs` — `OnDestroy() => OnImpactDetected?.Dispose();`
- [ ] `RespawnColliderView.cs` — то же для `OnCharacterFell`
- [ ] `VoidZoneView.cs` — для `KillPerformed`
- [ ] `ThrowableInteractionView.cs` — для всех Subject в файле
- [ ] `EnemyKickZoneView.cs` — проверить, уже ли добавлено (по плану должно быть)

**Причина:** Subject удерживают ссылки, при reload сцены утекают в GC. MVP(Viewer) — презентер не должен владеть lifetime.

### B.5 Скрыть Subject как IObservable

- [ ] `EnemyKickPresenter.cs:24` — `private readonly Subject<Unit> _onKickPerformed = new(); public IObservable<Unit> OnKickPerformed => _onKickPerformed;`
- [ ] `ImpactHandlerModel.cs:10-12` — то же для `OnStrongHit`, `OnWeakHit`, `OnThresholdReached`
- [ ] `EnemyKickZoneView.cs`, `ImpactDetectorView.cs`, `RespawnColliderView.cs`, `VoidZoneView.cs`, `ThrowableInteractionView.cs` — все Subject → `private` + `public IObservable`
- [ ] Обновить всех подписчиков (Presenter'ы) — изменения не нужны, подписка через `.Subscribe` работает с `IObservable`

**Причина:** Любой класс может вызвать `OnNext` на чужом Subject → обратный поток данных. Нарушает инкапсуляцию и data flow direction.

### B.6 Убрать пустые Dispose()

- [ ] `MoveEnemyPresenter.cs` — убрать `: IDisposable` и пустой `Dispose()`
- [ ] `PlayerKickPresenter.cs` — то же самое
- [ ] Проверить, не сломаются ли Zenject bindings (не должны — эти классы уже регистрируются через `IFixedTickable`)

**Причина:** Пустые Dispose дают ложный сигнал в аудите IDisposable. Они не используют `CompositeDisposable` или `CancellationTokenSource`.

---

## 🟢 Phase C — Архитектура

### C.1 Добавить Assembly Definitions (.asmdef)

- [ ] Создать `Assets/Scripts/Core/Game.Core.asmdef` (no references) — Enums, Constants, Extensions
- [ ] Создать `Assets/Scripts/Data/Game.Data.asmdef` (refs: Core) — Models
- [ ] Создать `Assets/Scripts/Services/Game.Services.asmdef` (refs: Core, Data) — Services, FSM, Sound
- [ ] Создать `Assets/Scripts/Gameplay/Game.Gameplay.asmdef` (refs: all) — Views, Presenters, Installers, Components
- [ ] Переместить папки под новую структуру (Models → Data/, Views+Presenters+Installers+Components → Gameplay/)
- [ ] Проверить, что ScriptableObjects остаются в Core (или отдельная сборка Game.Config)
- [ ] Добавить reference на Zenject assembly в каждой .asmdef
- [ ] Протестировать compile + Play Mode

**Причина:** Без asmdef любое изменение одного файла → полная рекомпиляция 96 файлов (3-5x медленнее). Также компилятор начнёт ловить нарушения слоёв (Models не могут `using Presenters`).

**Важно:** Это большая правка — реорганизация всех папок. Делать только после Phase A+B.

### C.2 Убрать Presenter→Presenter связь

- [ ] Создать `EnemyKickModel` с `Subject<Unit> OnKickPerformed`
- [ ] `EnemyKickPresenter` пушит в model, не в свой Subject
- [ ] `EnemyAnimationPresenter` подписывается на `EnemyKickModel.OnKickPerformed` (через `[Inject]`)
- [ ] Bind `EnemyKickModel` в `EnemyInstaller`

**Причина:** MVP(P): данные текут через Model, не через Presenter. Прямая связь Presenter→Presenter нарушает разделение ответственности и затрудняет тестирование.

### C.3 Убрать `[Inject] public` поля

- [ ] `TimerPresenter.cs:12-14` — все три на `private`
- [ ] `MoveCharacterModel.cs:8-12` — `[Inject] private` properties (или fields)
- [ ] Проверить, что никто не обращается к этим полям снаружи (если есть — сделать property getter)

**Причина:** `[Inject] public` нарушает инкапсуляцию — любой класс может перезаписать зависимость.

### C.4 Сделать ReactiveProperty private с IObservable accessor

- [ ] Все `Models/*.cs`: `public ReactiveProperty<T> X = new();` → `private readonly ReactiveProperty<T> _x = new(); public IObservable<T> X => _x;`
- [ ] Для записывающих Presenters: добавить method `SetX(T value) => _x.Value = value;` или сделать `public IReactiveProperty<T>` (UniRx поддерживает)
- [ ] Обновить подписчиков: `.Subscribe(x => ...)` остаётся, `.Value =` меняется на method или остаётся через `IReactiveProperty`

**Причина:** `public ReactiveProperty` позволяет кому угодно писать в state модели. Model должна контролировать мутации.

---

## 🔵 Phase D — Cleanup

### D.1 Удалить dead code

- [ ] Удалить `Services/Interfaces/IPlayerMovementInputHandler.cs` (нет реализации, нет потребителей)
- [ ] Удалить `Enums/MovementState.cs` и пустую папку `Services/FSM/States/MovementStates/`
- [ ] Удалить `Enums/EnemyType.cs` (затенён private enum в EnemyInstaller, не используется)
- [ ] Удалить `ScriptableObjects/Extensions/EnemyEntryExtensions.cs` (дубликат `EnemyVariants.GetRandomEnemyPrefab`)
- [ ] Проверить `CharacterVFX.WaveAnimation` — если не используется, удалить
- [ ] Проверить `GameHelpers.IsThrowable` (дубликат `TagExtensions.IsThrowable`) — удалить

**Причина:** Мёртвый код путает читателей, раздувает кодовую базу, создаёт ложное впечатление API.

### D.2 Удалить неиспользуемые using'и

- [ ] После всех правок пройти по каждому .cs файлу, убрать неиспользуемые using'и
- [ ] Или использовать IDE: Rider/Visual Studio "Remove Unused Usings" feature

**Причина:** Чистота, быстрее компиляция, проще ревью.

### D.3 Исправить опечатки в коде (по мере касания файлов)

- [ ] `PlayerKickPresenter.cs` — `KickNeardyEnemies` → `KickNearbyEnemies`, `CheckKickPresed` → `CheckKickPressed`
- [ ] `CharacterAnimationPresenter.cs`, `EnemyAnimationPresenter.cs` — `PlayAnimtion` → `PlayAnimation`
- [ ] `FatigueBarPresenter.cs` — `currentFutigue` → `currentFatigue`
- [ ] `SpawnEnemyPresenter.cs`, `CharacterDeathPresenter.cs` — `_respawnColiderViews` → `_respawnColliderViews`
- [ ] `Mover.cs` — `_distanseToGround` → `_distanceToGround`, `_distanseToForwardRamp` → `_distanceToForwardRamp`
- [ ] `Helpers.cs` — `_ofsets` → `_offsets`
- [ ] `ResultMenuView.cs` — `InItScore` → `InitScore`, `timeAlived` → `timeAlive`
- [ ] ProjectSettings/TagManager.asset — переименовать layer `OveerideTexure` → `OverrideTexture`

**Причина:** Читаемость. Делать попутно при других правках в файле, не отдельным коммитом.

### D.4 Обновить AGENTS.md

- [ ] Убрать упоминание `PlayerInstanseHandler` (класс удалён, заменён на `IPlayerProvider`)
- [ ] Поправить: `FatigueRestorationService` теперь в namespace `Services`, не `Presenters`
- [ ] Поправить typo-список: `FSMExtentsions.cs`, `KickImpactSettigns.cs` переименованы; `CharacterState .cs`/`VoidZoneObjectSpawnerView .cs` — тоже
- [ ] Поправить: `StunEffectPool` и `DeathEffectService` используют `[Inject]` field injection, не constructor
- [ ] Добавить: `VoidZoneSpawnerService`, `PlayerInputProvider` используют constructor injection
- [ ] Добавить новую секцию "Sub-container rules" — shared services только на уровне сцены
- [ ] Добавить "Encapsulation rules": `private ReactiveProperty` + `public IObservable`, никаких public Subject
- [ ] Добавить "Event flow convention": Presenter → Model (Subject) → другие Presenters, не Presenter → Presenter

**Причина:** AGENTS.md — документ для AI-ассистентов и новых разработчиков. 6 утверждений устарели, нет правил о новых архитектурных решениях.

### D.5 Обновить вики

- [ ] `wiki/architecture/overview.md` — обновить DI topology (sub-containers, PlayerProvider на уровне сцены)
- [ ] `wiki/conventions/coding-standards.md` — убрать упоминание PlayerInstanseHandler, обновить gotchas
- [ ] Создать `wiki/architecture/di-container-topology.md` — диаграмма контейнеров (Scene → Player/Enemy sub-containers)
- [ ] Создать `wiki/decisions/` (или использовать эту страницу) — архитектурные ADR'ы

**Причина:** Вики должна отражать текущее состояние проекта.

---

## 🟣 Phase E — Тесты

### E.1 Настроить тестовую инфраструктуру

- [ ] Создать `Assets/Scripts/Tests/EditMode/Game.Core.Tests.asmdef` (refs: Game.Core, NUnit)
- [ ] Создать `Assets/Scripts/Tests/EditMode/Game.Services.Tests.asmdef` (refs: Game.Services, NUnit)
- [ ] Создать `Assets/Scripts/Tests/EditMode/Game.Data.Tests.asmdef` (refs: Game.Data, NUnit)
- [ ] Прогнать empty test, убедиться что Unity Test Runner видит их

**Причина:** Без asmdef тесты запускают всю сборку. Изоляция сборок = быстрые изолированные тесты.

### E.2 Тесты на критичные Services

- [ ] `VoidZoneSpawnerServiceTests` — спавн в пределах платформы, cap concurrent spawns, overlap check
- [ ] `FatigueManagerTests` — расчёт damage, восстановление, кэп 0..100
- [ ] `EnemyPoolTests` — pre-warm до лимита, reset state при Return
- [ ] `StunerTests` — StoreOldTag/TryGetOldTag по InstanceID, double-store safety

**Причина:** Это логика с тонкими багами (см. B.3). Регрессии всплывут только в Play Mode без тестов.

### E.3 Тесты на Models

- [ ] `ImpactHandlerModelTests` — пороги WeakHit count, реакция OnStrongHit
- [ ] `TimerModelTests` — обратный отсчёт, edge cases (0, negative)
- [ ] `FatigueModelTests` — кэп 0..100, реакция на изменения

**Причина:** Model = state. Любая ошибка в state ломает всю игру. Простые unit-тесты ловят большинство багов.

### E.4 Play Mode тесты (опционально)

- [ ] Настроить Zenject Test Framework для scene tests
- [ ] `FirstSceneLoadTests` — сцена загружается без NullRef, все биндинги резолвятся

**Причина:** Интеграционный smoke test на самую частую проблему — сломанные Zenject биндинги.

---

## 📋 Чек-лист приоритетов

При реализации **строго сверху вниз**:

1. ✅ Phase A (3 пункта) — без этого не запускается
2. ✅ Phase B (6 пунктов) — ломает геймплей
3. ✅ Phase C (4 пункта) — ускорит разработку в будущем
4. ✅ Phase D (5 пунктов) — cleanup
5. ✅ Phase E (4 пункта) — качество

**Всего:** 22 задачи, ~3-5 дней работы.

---

## 🔗 Связанные страницы

- [[Refactoring Plan|Первоначальный план рефакторинга]] (внешний документ `docs/refactoring-plan.md`)
- [[architecture/overview|Architecture Overview]]
- [[conventions/coding-standards|Coding Standards]]

## 📚 Источники

- Code review после завершения Phase 1-7 первичного рефакторинга
- Architecture analysis (3 Zenject контейнера, dependency graph, anti-patterns)
- Проверка AGENTS.md vs фактическое состояние кода
