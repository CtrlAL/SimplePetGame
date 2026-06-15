---
tags:
  - system
  - combat
---

# Combat Mechanics

## Kicking (Player → Enemy)

`PlayerKickPresenter` — сфера каждые FixedTick. Проверка `_fsm.IsIdleState()` + `TagExtensions.IsEnemy()`. Звук при попадании.

## Kicking (Enemy → Player)

`EnemyKickZoneView` — триггер-зона с задержкой (`UniTask.Delay`), отменяемой при выходе. `EnemyKickPresenter` вызывает `_kicker.Kick()`.

## Stun

`StunnedState` — `IStuner.ApplyStun()` / `RemoveStun()`. Таймер 4s → авто-возврат в Idle. `StunEffectService` управляет VFX через `Dictionary<int, ParticleSystem>`.

## Throwables

Pickup → throw (Impulse force) → damage/stun. Детали дамага — в `ThrowableInteractor` и соответствующих stats.
