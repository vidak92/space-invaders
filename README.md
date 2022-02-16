# Space Invaders
This is a sample game project created with [Unity](https://unity.com/). It's a simplified version of the classic [Space Invaders](https://en.wikipedia.org/wiki/Space_Invaders) game.

The game is pretty simple, but the project's been set up as for a bigger game. Think of this as a *small scale model* of a bigger project.

<img src="GIFs/space_invaders.gif" width="480" />


## Features
- The player fights against infinite waves of enemies.
- The enemies and player shoot at each other.
- The player has a limited number of lives.
- The game is over when the player loses all lives.
- The top 10 high scores are saved to local storage.
- The game runs on desktop and mobile platforms.
    - *Tested on Windows and Android, but should also work on MacOS, Linux and iOS.*
- The destructible bunkers/barricades are excluded in this version of the game.
- There's currently no sound in the game.

## Dependencies
- [MijanTools](https://github.com/vidak92/mijan-tools): A utility-style library for Unity.
- [Zenject(Extenject)](https://github.com/Mathijs-Bakker/Extenject): For dependency injection.

## Usage
Clone the repo or download it's contents, then open the project in Unity. This project uses Unity 2020 LTS.

**Note:** After cloning the repo, run `git submodule update --init` to initialize the *MijanTools* submodule.

## Code Architecture
Code is generally split in the following types of classes:
- `AppController`: Central starting point for the game, handles top-level state/logic.
- `GameplayConfig`: Contains all tweakable gameplay parameters.
- `XXXController`: Used for handling high-level, gameplay-related logic.
- `XXXService`: Services with common functionality used throughout the code.
- `XXXState`: Used for handling each high-level game state.
- `UIController` and `XXXScreen`: Used for handling UI logic.
- The rest are gameplay-related(`Player`, `Enemy`, etc.) or utility-style classes.

The project uses *dependency injection*(via Zenject) and a *state machine* for handling the high-level game state.

## Roadmap
- ~~**v 0.1**~~
	- ~~Implement basic game.~~
	- ~~Support for desktop and mobile platforms.~~
- **v 0.2**
    - Explosion particle effects for the player and enemies.
    - Background particle effects.
    - Sound effects.
- **v 0.3+**
    - Destructable bunkers/barricades.

## Credits
- The 3D models used in the game are from [Kenney's Space Kit](https://www.kenney.nl/assets/space-kit).
