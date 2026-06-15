---
tags:
  - entity
  - enemy
---

# Enemy

## Installer

`EnemyInstaller` → extends `CharacterInstaller`. Calls `base.InstallBindings()` last.

**Внимание**: определяет свой private `EnemyType` enum (`Default`/`Big`), который shadow-ит глобальный `Enums.EnemyType` (там только `Small`). Маппинг через `_statsMapping` словарь.

## Stats

- `EnemyStats` — базовые
- `BigEnemyStats` — extends `EnemyStats`, без переопределений (маркер-класс для DI)

## Movement

`MoveEnemyPresenter` — `NavMeshAgent` с `updatePosition = false` (ручная синхронизация через `_navMeshAgent.nextPosition`). Цель — `PlayerInstanseHandler.Instance`. Прыжок при перепаде высоты.

## Kick (от врага)

`EnemyKickZoneView` — `Subject<Collider>` + `UniTask.Delay` (`_enemyStatsSO.DelayBeforeKick`) при входе игрока в триггер. CancellationTokenSource при выходе. `EnemyKickPresenter` подписывается, дёргает `_kicker.Kick()` если `_fsm.IsIdleState()`.

## Spawn

`SpawnEnemyPresenter` — таймер (`LevelSettings`). `EnemyFactory` (constructor injection) — рандомный спавн-поинт. `EnemyVariants` — weighted random (inverse `SpawnRate`).
