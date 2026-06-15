---
tags:
  - system
  - fsm
---

# FSM / State Machine

## Architecture

Двухслойная:

1. **`StateMachine`** — generic, держит `IState`, `ChangeState`/`Enter`/`Exit`
2. **`CharacterFSM`** — `IFixedTickable` + `IInitializable` + `IDisposable`. Владеет `Dictionary<CharacterState, IState>` (Idle, Stunned). Таймер стана: `Observable.Timer(4s)` → авто-возврат в Idle.

## States

- `IState` — default no-op `Enter`/`Update`/`Exit` (C# 8 default interface methods)
- `ObservableState` — abstract, добавляет `Subject<Unit>` для `OnStateEnter`/`OnStateExit`
- `IdleState` — пустой (extends `ObservableState`)
- `StunnedState` — `[Inject]` поля, `IStuner.ApplyStun()` на Enter, `RemoveStun()` на Exit

## Usage

`FSMExtentsions.IsIdleState()` — extension через `is` pattern. Используется повсеместно для гейтинга действий (kick, pickup, movement).

## Quirks

- `CharacterState .cs` — с пробелом в имени файла
- `FSMExtentsions.cs` — опечатка ("Extentsions")
- `MovementState` enum (`Grounded`, `Jumping`, `Falling`) — объявлен, скорее всего не используется
