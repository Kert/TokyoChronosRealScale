This is a mod for Tokyo Chronos

It changes game world scale to match real world because by default everything looks way too huge

When using the mod the floor in game is synced perfectly with the floor in the real world

Reset VR position in standing/sitting pose depending on how you want to play the game

# How to install
First you need to prepare the game for using mods
## BepInEx 5 Install
Grab an x64 zip with [BepInEx 5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4)

Extract the contents into the game folder

Run the game once and close it.

## Mod Install
Grab ```MMHOOK.zip``` from [here](https://github.com/Kert/TokyoChronosRealScale/releases/tag/MMHOOK) and extract archive into your game folder

It's a pre-compiled library that was made using [MonoMod HookGen](https://github.com/MonoMod/MonoMod/blob/master/README-RuntimeDetour.md) to make modding easier

Take mod's *.dll file and put it into ```%game folder%/BepInEx/plugins/```

Compiled *.dll file is [here](https://github.com/Kert/TokyoChronosRealScale/releases)

# Compiling mod yourself (for software engineers)
After that you just run **build.bat**

The project expects that your game folder is ```C:\Program Files (x86)\Steam\steamapps\common\TOKYO CHRONOS\```

You should change all instances of that in **.csproj** and **build.bat** if yours is different 
