# Base Game Unity (Andorid & iOS), Use Scriptable Architecture
- Unity 2022.3 (LTS)
  
Description: Gamebase for mobile hyper casual, casual game

- Use [sunflower](https://github.com/VirtueSky/sunflower) package

- GameFlow

```mermaid
flowchart TD

subgraph ServiceScene["<i class="fa-brands fa-unity"></i> Service Scene"]
    Initialization(Initialization)
    LevelLoader(LevelLoader)
    AudioManager(AudioManager)
    end

    subgraph GameScene["<i class="fa-brands fa-unity"></i> Game Scene"]
    GameManager(GameManager)
    PopupManager(PopupManager)
    end
    EntryGame{Entry Game} --> ServiceScene
    AudioManager --> SoundComponent{{Pooling: SoundComponent-AudioSource}}
    Initialization --Load Game Scene--> GameScene 
    GameManager --> StartGame{Start Game} --> LevelLoader --Instantiate--> Level(Level)
    PopupManager --Show PopupInGame--> StartGame
    Level --Win Level--> WinGame{Win Game} --Next Level-->GameManager
    Level --Lose Level--> LoseGame{Lose Game} --Replay or Skip Level-->GameManager
    Level --Replay Level--> ReplayGame{Replay Game}
    ReplayGame --Replay Level--> GameManager
    PopupManager --Show PopupWin--> WinGame
    PopupManager --Show PopupLose--> LoseGame
```

## Note
### GameConfig Window
- Shortcut (`Ctrl + ~` or `Command + ~`) to open TheBeginnig Window


https://github.com/user-attachments/assets/2f47a9bc-94b3-4982-aeb7-d9d20835591c


- Open the `GameConfig` script to add or edit configs,


![Screenshot 2024-07-26 093403](https://github.com/user-attachments/assets/10b0a2ce-7f34-48ea-b6fe-487b640c3cbf)

### DebugView

- Enable Debug View `true` in GameConfig window
- Swipe up on the edge (left or right) of the screen to open (shortcut in editor (`Alt+D` or `Option+D`))


![Unity_9YRD8rJRE1](https://github.com/user-attachments/assets/73692ff8-918a-4721-bd7b-c380d4a9cb14)


- Note: Version use addressable to load scenes, popups and levels [here](https://github.com/VirtueSky/TheBeginning/tree/use_addressable)

