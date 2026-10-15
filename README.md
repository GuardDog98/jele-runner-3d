# Jele Runner 3D

A simple 3D runner prototype inspired by the idea of a character named **Jele**, collecting coins and buying cosmetic skins. This project is designed as an offline single-player game prototype and can be opened in Unity.

## Features
- 3D runner gameplay
- Smooth movement with lane switching and jumping
- Coin collection system
- Currency progression with local save
- Skin shop with cosmetic upgrades
- Simple generated environment and UI
- No online dependency

## Unity setup
1. Download and install Unity 2022.3 LTS or newer.
2. Open the repository as a Unity project.
3. In Unity, open the default scene or create an empty scene.
4. Add an empty GameObject named `GameBootstrap`.
5. Attach the `GameBootstrap` script from `Assets/Scripts/GameBootstrap.cs`.
6. Press Play.

## Controls
- A / D or Left / Right Arrow: move between lanes
- Space: jump
- Esc: pause (simple pause state built into the bootstrap)

## Notes
- Game progress is saved locally using PlayerPrefs.
- Coins and skins are stored on the local machine for offline play.
- This is a prototype. You can expand it with enemy AI, sound, better art, and more levels.

## Repo structure
- `Assets/Scripts`: game logic
- `Assets/Scenes`: expected location for future scenes
- `Packages/manifest.json`: Unity package manifest

## Quick idea for expansion
- Add a start menu and score screen
- Add animated coin particles
- Add shop panel with larger item catalog
- Add game over flow and restart button
- Add sound and stronger environment art

If you want, the next step can be adding a polished UI, more levels, or a better coin/track system.
