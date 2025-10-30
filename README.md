# The Silksong Racing Mod

This is a [Hollow Knight: Silksong](https://hollowknightsilksong.com/) mod that removes all waiting. It is designed as a quality of life mod for people wanting to practice the game casually or race each other in a head-to-head speedrun.

Under the hood, it uses the [BepInExPack for Silksong](https://thunderstore.io/c/hollow-knight-silksong/p/BepInEx/BepInExPack_Silksong/) to change the functionality of the game.

## Features

### Main Menu Features

#### Skip Intro

- Skips the "Team Cherry" splash screen.
- Skips the screen that explains to you what the save indicator means.

#### Fast Main Menu

- Makes the menu appear instantly (instead of slowly fading in).
- Makes the save slot screen appear instantly (instead of animating all 4 rectangles one by one).

#### Bypass Main Menu

- Makes the main menu automatically transition to the save file selection screen.

### In-Game Menu Features

#### Skip Save and Quit Confirmation

- Skips the "Yes" or "No" confirmation menu that appears after selecting "Save & Quit".

#### Fast Text

- Makes dialog text with NPCs instantaneous.

### In-Game Features

#### Skip Cutscenes

- Automatically skips certain cutscene sequences in the game, including the opening cutscene.
- Instantly grants abilities at Shrine Weaver locations by skipping the interaction sequence, dialogue, and animations.

#### Skip Area Intro

- Marks all areas as visited to skip the splash text that appears when you first visit an area.

#### Hornet Quick Start

- Gives immediate control at the beginning of the game.

### Debug Features

#### Debug Needle Damage

- Makes Hornet's needle deal 999 damage on each swing.
- This feature is disabled by default. Enable it in the configuration file for debugging or testing purposes.

#### Debug Warp

- Warps to a specific scene upon loading into the game. (Edit "DebugWarp.cs" to specify the scene.)
- This feature is disabled by default. Enable it in the configuration file for debugging or testing purposes.

## Configuration

Every feature that this mod provides is optional, if you do not like a specific feature, you can disable it by editing the configuration file. By default, it is located at:

```txt
C:\Users\[username]\AppData\Roaming\r2modmanPlus-local\HollowKnightSilksong\profiles\Default\BepInEx\config
```

## Credits

Also see Vitaxses' excelled [Silksong QoL mod](https://github.com/Vitaxses/Silksong.QoL).

## TODO

- Automatically grant Silk Spear upon reading the thing.
  - `hasNeedleThrow = True`
  - `hasSilkSpecial = True`
- Boss stuff:
  - Moss Mother intro
  - Moss Mother death animation
  - Open the Moss Mother doors faster after she dies.
  - Bell Beast intro
  - Bell Beast death animation
- Speed up grabbing silk heart into cutscene
  - Set `UnlockedFastTravel` after getting silk heart to avoid roaring cutscene when walking to the right
