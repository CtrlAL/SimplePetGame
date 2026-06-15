# SimplePetGame Wiki — Design

## Goal
Create an Obsidian-based project wiki for SimplePetGame (Unity, URP, MVP+P + Zenject) co-located in the repository for easy access during development.

## Location
`wiki/` directory at the project root, with `.obsidian/` vault configuration inside.

## Structure

```
wiki/
├── .obsidian/             # Obsidian vault config
├── _raw/                  # Staging for quick captures → promoted later
├── architecture/          # Big picture: MVP+P, DI, FSM, scene flow
├── entities/              # Game entities: Player, Enemy, Throwable, etc.
├── systems/               # Gameplay systems: combat, sound, pooling, stuns
├── conventions/           # Coding standards, naming, git workflow
├── decisions/             # ADRs
├── reference/             # External links, Unity guides
├── index.md               # Map of Content
├── glossary.md            # Project terminology
└── hot.md                 # Currently active / recent changes
```

## Key Wiki Skills Used
- `wiki-ingest` — import AGENTS.md, code exploration into wiki pages
- `wiki-capture` — save findings during development
- `wiki-query` — ask questions about the project via wiki

## First Population
1. Ingest `AGENTS.md` → `architecture/overview.md`, `conventions/`, `glossary.md`
2. Explore `Scripts/` structure → populate `architecture/`, `entities/`, `systems/`
3. Create `index.md` (MOC) and `hot.md`
