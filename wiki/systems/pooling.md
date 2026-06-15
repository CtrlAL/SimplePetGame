---
tags:
  - system
  - pooling
---

# Object Pooling

## Interface

Generic `IObjectPool<T>`: `SpawnObject()` / `ReturnToPool(T)`. Типизированные интерфейсы (`IEnemyPool`, `IDeathEffectPool`, `IStunEffectPool`) — только для DI, новых членов не добавляют.

## Internal

Все три пула используют `ConcurrentQueue` (из `System.Collections.Concurrent`) — thread-safe, хотя игра однопоточная.

## EnemyPool

Pre-warm в `Initialize()`: до `EnemyPoolSizeLimit + BigEnemyPoolSizeLimit` инстансов через `_diContainer.InstantiatePrefab()`. Созданные объекты сразу деактивируются.

`SpawnObject()` — честно деактивейт один элемент из очереди. Если очередь пуста или объект убит — создаёт новый.

## DeathEffectPool / StunEffectPool

Спавнят `ParticleSystem` через `DiContainer.InstantiatePrefabForComponent<ParticleSystem>().` `ReturnToPool` проверяет лимит размера: `_enemies.Count < EnemyPoolSizeLimit`.

## Reset state

`IPoolableEnemy` / `PoolableEnemyReset` — MonoBehaviour на корне врага в `Components/`. Вызывает `ResetState()` при спавне из пула:
- `FatigueModel.CurrentFatigue` → 0
- `ImpactHandlerModel.CurrentWeakHitCount` → 0
- `CharacterFSM.ChangeToState(Idle)`

## Services

- `DeathEffectService` — обёртка с позиционированием и auto-return после `particleSystem.main.duration`
- `StunEffectService` — `Dictionary<int, ParticleSystem>` (keyed by `GetInstanceID()`). Обновляет позиции в `FixedTick` через `CharacterStunPresenter`
