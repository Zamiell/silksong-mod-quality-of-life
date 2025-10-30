# Instructions for Claude

## General Instructions

- Read the "README.md" file.
- Read the "features/SkipIntro.cs" file to see an example of a feature that uses Harmony to hook in-game functions.
- Read the "FSMEdits/ShrineWeaverAbility.cs" file to see an example of a feature that uses [Silksong.FsmUtil](https://github.com/silksong-modding/Silksong.FsmUtil) to modify an in-game FSM.
  - An FSM is a finite state machine, which is a simple script that allows the developers of the game to easily define object and NPC behavior.
- If needed for research purposes, you can find the extracted C# game files at: D:\Games\PC\Silksong\1.0.28891\ExportedProject\Assets\Scripts\Assembly-CSharp\
  - Most in-game objects and NPCs do not have any C# code relating to them, because they are completely defined by FSM.
- When adding comments, always make sure they are complete sentences with an ending period.
- After editing a file, make sure that the project still successfully compiles with: `dotnet build -warnaserror`
- After editing a file, make sure that it is formatted with: `dotnet csharpier format .`
- After adding a new feature, make sure it is documented in the "README.md" file.
- If the feature name is not specified by the user, assume that it is part of the "SkipCutscenes" feature.

## Dealing with Errors

If you get any errors like this:

```txt
D:\Repositories\SilksongModQualityOfLife\features\FastMenu.cs(286,68): error CS1069: The type name 'Animator' could not be found in the namespace 'UnityEngine'. This type has been forwarded to assembly
     'UnityEngine.AnimationModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' Consider adding a reference to that assembly. [D:\Repositories\SilksongModQualityOfLife\QoL.csproj]
```

Then modify the "QoL.csproj" file and add the reference. In this case, you would need to add:

```xml
    <Reference Include="UnityEngine.AnimationModule">
      <HintPath>..\..\SteamLibrary\steamapps\common\Hollow Knight Silksong\Hollow Knight Silksong_Data\Managed\UnityEngine.AnimationModule.dll</HintPath>
    </Reference>
```
