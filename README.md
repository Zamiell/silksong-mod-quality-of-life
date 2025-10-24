# Quality of Life Mod

This is a [Hollow Knight: Silksong](https://hollowknightsilksong.com/) mod that removes all waiting.

It uses the [BepInExPack for Silksong](https://thunderstore.io/c/hollow-knight-silksong/p/BepInEx/BepInExPack_Silksong/).

## Features

### Instant Main Menu

Skips the intro sequence (`Pre_Menu_Intro`) and loads the main menu (`Menu_Title`) instantly when you launch the game. No more waiting through splash screens or intro animations!

## How It Works

The mod uses Harmony to patch the `AddressablesLoadScene.Start()` method. When the game tries to load the intro scene (`Scenes/Pre_Menu_Intro`), the patch intercepts it and redirects to the main menu (`Scenes/Menu_Title`) instead.
