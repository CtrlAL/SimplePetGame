---
tags:
  - conventions
  - code-style
---

# Coding Standards

## DI / Zenject

- Bind via `Container.Bind<T>().To<TImpl>().AsSingle()`
- `Container.BindInterfacesAndSelfTo<T>().AsSingle()`
- Two styles: `[Inject]` field injection (most classes) and constructor injection (some presenters)

## UniRx

- `ReactiveProperty<>` for state
- `Subject<>` for events
- `CompositeDisposable` for cleanup

## Naming

- Follow existing file conventions (typos preserved — no fixes)
- Tags via `CompareTag()` with `CharacterTags.*` / `EnvironmentTags.*`
- Namespaces vary: some files use no namespace, some use root namespaces

## Gotchas

- `IPlayerMovementInputHandler` — dead code, no implementation
- `PlayerInstanseHandler` — static singleton (anti-pattern)
- `EnemyInstaller` has private `EnemyType` enum shadowing global `Enums.EnemyType`
