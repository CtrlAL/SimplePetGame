---
tags:
  - entity
  - player
---

# Player

## Installer

`PlayerInstaller` → extends `CharacterInstaller`, calls `base.InstallBindings()` last.

Adds: `PlayerKickPresenter`, `ThrowableInteractionModel`/`ThrowableInteractionPresenter`, `OutlinePresenter`, `MovePlayerPresenter`, `CharacterAnimantionPresenter`, `FatigueBarPresenter`. Binds `AbstractStats` → `PlayerStats`.

## Movement

`MovePlayerPresenter` (`IFixedTickable` + `IInitializable`). Reads input from `IPlayerInputProvider` (Unity `PlayerInputActions` wrapper), delegates to `IMover`. Jump → sound via `SoundManager`.

## Kick

`PlayerKickPresenter` — `Physics.OverlapSphere` каждый FixedTick. Проверка `_fsm.IsIdleState()` перед ударом. Цели по `TagExtensions.IsEnemy()`.

## Static singleton

`PlayerInstanseHandler.Instance` — `GameObject.FindWithTag("Player")`. Используется `EnemyKickZoneView` и `OutlinePresenter` вместо DI.

## Dead code

`IPlayerMovementInputHandler` — объявлен, нет реализации.
