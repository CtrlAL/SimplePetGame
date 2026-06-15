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

Pre-warm в `Initialize()`: до `EnemyPoolSizeLimit + BigEnemyPoolSizeLimit` копий из `EnemyVariants.GetRandomEnemyPrefab()`.

**Баг**: `SpawnObject()` — while-loop опустошает всю очередь, возвращает только последний элемент.

## DeathEffectPool / StunEffectPool

Спавнят `ParticleSystem` через `DiContainer.InstantiatePrefabForComponent<ParticleSystem>().` `ReturnToPool` проверяет лимит размера.

## Services

- `DeathEffectService` — обёртка с позиционированием и auto-return после `particleSystem.main.duration`
- `StunEffectService` — `Dictionary<int, ParticleSystem>` (keyed by `GetInstanceID()`). Обновляет позиции в `FixedTick` через `CharacterStunPresenter`
