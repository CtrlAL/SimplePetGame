---
tags:
  - entity
  - throwable
---

# Throwable Objects

## Detection

`ThrowableInteractionView` — 3 `Subject<GameObject>` (`OnObjectEnteredRange`, `OnObjectStayedInRange`, `OnObjectExitedRange`) через `OnTriggerEnter/Stay/Exit`.

## Presenter

`ThrowableInteractionPresenter` — constructor injection (редкий случай). Логика: pickup/put/throw.

- **Pickup**: `IdleState`, isThrowable-tagged, grounded, ближайший
- **Throw**: `IdleState` + `IsHolding`
- **IFixedTickable**: чистка null, force put если объект больше не throwable

## Service

`ThrowableInteractor` — физика: pickup (kinematic/gravity false, parent to slot), throw (`AddForce`, `ForceMode.Impulse`), put (drop перед владельцем).

## Outline

`OutlinePresenter` — `QuickOutline`. Каждый FixedTick находит ближайший grounded throwable и включает его outline, отключая остальные.
