---
tags:
  - system
  - sound
---

# Sound & Audio

## Architecture

**Unity AudioSource** (не FMOD!). Несмотря на наличие `Assets/Plugins/FMOD/`, кодовая аудиосистема использует `AudioSource.PlayOneShot()`.

## SoundManager

`MonoBehaviour` с `[RequireComponent(typeof(AudioSource))]`. Дёргается через `SoundManager.PlaySound(volume, SoundType)`.

## SoundType

Enum с `#region` блоками (Actions, Events, Game States, Extensions). Индекс массива в `CharacterSounds` соответствует `(int)SoundType` — хрупко к изменению порядка.

## BackgroundMusicPlayer

Отдельный `MonoBehaviour` со своим `AudioSource`. Плейлист с опциональным shuffle. Последовательное воспроизведение через `UniTask.Delay(clip.length)`. `SwitchToTrack(BackgroundSoundType)`.

## Binding

- `SoundManager`, `BackgroundMusicPlayer` — `FromComponentInHierarchy().AsSingle()` в `GameSceneInstaller`
- `CharacterSounds`, `BackgroundSounds` — `ScriptableObjectInstaller`
