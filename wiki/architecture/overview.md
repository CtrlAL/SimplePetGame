---
tags:
  - architecture
  - overview
---

# Architecture Overview

## MVP+P + Zenject DI

Models — reactive state (UniRx `ReactiveProperty<>`), Views — dumb MonoBehaviours, Presenters — wire them together, Services — logic.

| Layer | Folder | Role |
|-------|--------|------|
| Models | `Scripts/Models/` | State with `ReactiveProperty<>`, implement `IDisposable` |
| Views | `Scripts/Views/` | `MonoBehaviour`, `[SerializeField]` references |
| Presenters | `Scripts/Presenters/` | Subscribe to Models, update Views. Often implement `IInitializable`, `IFixedTickable`, `IDisposable` |
| Services | `Scripts/Services/` | Logic behind interfaces |
| Installers | `Scripts/Installers/` | `MonoInstaller` subclasses. `Container.Bind<T>().To<TImpl>().AsSingle()` |
| ScriptableObjects | `Scripts/ScriptableObjects/` | Config assets. Bound in `ScriptableObjectInstaller` |
| Enums | `Scripts/Enums/` | Shared enums |
| Extensions | `Scripts/Extensions/` | `CompareTag()` helpers using `Constants.*` |

## Character hierarchy

`CharacterInstaller` (base, `BoxCollider` on root) → `PlayerInstaller` / `EnemyInstaller`. Both call `base.InstallBindings()` last.

## Scene

`FirstScene.unity` — единственная сцена в build.
