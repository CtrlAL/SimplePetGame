# SimplePetGame Wiki Setup — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Create an Obsidian-based project wiki in `wiki/` at the project root

**Architecture:** Standard Obsidian vault layout with `_raw/` staging area + categorized knowledge folders. Minimal `.obsidian/` config to enable core features (graph, tags, quick switcher).

**Tech Stack:** Obsidian, Markdown, YAML frontmatter

---

### Task 1: Create directory structure + `.obsidian/` config

**Files:**
- Create: `wiki/_raw/`
- Create: `wiki/architecture/`
- Create: `wiki/entities/`
- Create: `wiki/systems/`
- Create: `wiki/conventions/`
- Create: `wiki/decisions/`
- Create: `wiki/reference/`
- Create: `wiki/.obsidian/app.json`
- Create: `wiki/.obsidian/core-plugins.json`
- Create: `wiki/.obsidian/community-plugins.json`
- Create: `wiki/.obsidian/appearance.json`
- Create: `wiki/.gitignore` (append `wiki/.obsidian/workspace.json` to existing if needed)

- [ ] **Step 1: Create wiki directory tree**

Run:
```powershell
New-Item -ItemType Directory -Path "wiki\_raw" -Force
New-Item -ItemType Directory -Path "wiki\architecture" -Force
New-Item -ItemType Directory -Path "wiki\entities" -Force
New-Item -ItemType Directory -Path "wiki\systems" -Force
New-Item -ItemType Directory -Path "wiki\conventions" -Force
New-Item -ItemType Directory -Path "wiki\decisions" -Force
New-Item -ItemType Directory -Path "wiki\reference" -Force
```

- [ ] **Step 2: Create `.obsidian/app.json`**

```json
{
  "alwaysUpdateLinks": true,
  "attachmentFolderPath": "_raw/attachments",
  "promptDelete": false,
  "showFrontmatter": true,
  "showLineNumber": true,
  "strictLineBreaks": false,
  "useMarkdownLinks": true,
  "newLinkFormat": "relative"
}
```

- [ ] **Step 3: Create `.obsidian/core-plugins.json`**

```json
[
  "file-explorer",
  "global-search",
  "switcher",
  "graph",
  "backlink",
  "outgoing-link",
  "tag-pane",
  "page-preview",
  "command-palette",
  "markdown-importer",
  "word-count",
  "open-with-default-app",
  "file-recovery"
]
```

- [ ] **Step 4: Create `.obsidian/appearance.json`**

```json
{
  "accentColor": "#7c3aed",
  "cssTheme": "",
  "enabledCssSnippets": [],
  "baseFontSize": 15,
  "theme": "obsidian"
}
```

- [ ] **Step 5: Create `.obsidian/community-plugins.json`**

```json
[]
```

- [ ] **Step 6: Add `wiki/.obsidian/workspace.json` to `.gitignore`**

Append to `.gitignore`:
```
# Obsidian workspace (auto-generated, machine-specific)
wiki/.obsidian/workspace.json
wiki/.obsidian/workspace-mobile.json
```

### Task 2: Create foundational wiki pages

**Files:**
- Create: `wiki/index.md`
- Create: `wiki/glossary.md`
- Create: `wiki/hot.md`

- [ ] **Step 1: Create `wiki/index.md`**

```markdown
---
tags:
  - moc
  - hub
---

# SimplePetGame Wiki

**Unity 2022.3.62f3 · URP · MVP+P + Zenject**

## Архитектура

- [[architecture/overview|Architecture Overview]]
- [[architecture/mvp-pattern|MVP+P Pattern]]
- [[architecture/zenject-di|Zenject DI Setup]]

## Игровые сущности

- [[entities/player|Player]]
- [[entities/enemy|Enemy]]
- [[entities/throwable|Throwable Objects]]

## Системы

- [[systems/state-machine|FSM / State Machine]]
- [[systems/combat|Combat Mechanics]]
- [[systems/sound|Sound & FMOD]]
- [[systems/pooling|Object Pooling]]

## Разработка

- [[conventions/coding-standards|Coding Standards]]
- [[conventions/naming|Naming Conventions]]
- [[reference/links|External References]]

## Быстрый доступ

- Открыть Obsidian: `File → Open Vault → Open folder as vault → выбрать `wiki/`
```

- [ ] **Step 2: Create `wiki/glossary.md`**

```markdown
---
tags:
  - glossary
  - index
---

# Глоссарий

| Термин | Описание |
|--------|---------|
| **MVP+P** | Model-View-Presenter + Passive View — архитектурный паттерн проекта |
| **Zenject** | DI-фреймворк (Extenject) |
| **UniRx** | Reactive Extensions for Unity — `ReactiveProperty<>`, `Subject<>` |
| **UniTask** | Async/await библиотека (Cysharp) |
| **FSM** | Finite State Machine — управление состояниями персонажей |
| **FMOD** | Аудио-мидлварь |
| **CharacterFSM** | FSM-контроллер персонажа |
| **ReactiveProperty** | Observable property из UniRx |
| **Installer** | Zenject-класс для настройки DI |
| **Presenter** | Связка Model ↔ View |
| **Pool** | Пул объектов (EnemyPool, DeathEffectPool) |
```

- [ ] **Step 3: Create `wiki/hot.md`**

```markdown
---
tags:
  - hot
  - meta
---

# Hot / Active

## Текущее

- Создание вики проекта (2026-06-15)

## Последние изменения

- Инициализация vault'а
```

### Task 3: Ingest AGENTS.md into wiki

**Files:**
- Create: `wiki/architecture/overview.md`
- Create: `wiki/conventions/coding-standards.md`

- [ ] **Step 1: Extract architecture overview from AGENTS.md**

```markdown
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
```

- [ ] **Step 2: Create conventions page**

```markdown
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
```

### Task 4: Explore codebase and populate entity pages

**Files:**
- Create: `wiki/entities/player.md`
- Create: `wiki/entities/enemy.md`
- Create: `wiki/entities/throwable.md`
- Create: `wiki/systems/state-machine.md`
- Create: `wiki/systems/sound.md`
- Create: `wiki/systems/pooling.md`

- [ ] **Step 1: Read key files to extract entity knowledge**

Explore the codebase to populate wiki pages. Use glob/grep to find the main files for each system.

- [ ] **Step 2: Create entity pages with key facts**
