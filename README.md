<p align="center">
    <img src="./Assets/Textures/FocusBreakout_Title.png" alt="Illume" />
</p>

<p align="center">
    <a href="https://unity.com/"><img src="https://img.shields.io/badge/Made%20with-Unity-57d357.svg?style=flat&logo=unity" /></a>
    <img src="https://img.shields.io/badge/-v2021.1.17f1-57d357?style=flat" />
    <img src="https://img.shields.io/badge/-csharp%208.0-7a5edf?style=flat" />
    <a href="https://mbajaman.itch.io/focus-breakout"><img src="https://img.shields.io/badge/Published%20on-Itch-fa5c5c.svg?style=flat&logo=itch.io" /></a>
</p>

## Overview
You are a sentient target blob trying to escape the confines of an aim trainer.

## Unity Packages

* Cinemachine
* ProBuild
* ProGrids
* PostProcessing
* Unity Particle Pack
* Universal RP

## Compilation Instructions

The project is supported on Unity 2021.1.17f1. All packages (mentioned above) are part of the project and simply opening the project in Unity should download and load them. No additional external libraries are required.

## Resources

3rd person movement/camera follow:
https://www.youtube.com/watch?v=SeBEvM2zMpY
*Requires cinemachine from package manager

Moving Platform: 
https://youtu.be/rO19dA2jksk 

Coroutines:
https://www.youtube.com/watch?v=t4m8pmahbzY 

Raycast:
https://gamedevbeginner.com/raycasts-in-unity-made-easy/
https://www.youtube.com/watch?v=THnivyG0Mvo 

Compass:
https://www.youtube.com/watch?v=aMEHOU6xpWA&t=459s&ab_channel=MattParkin

Timer:
https://www.youtube.com/watch?v=HmHPJL-OcQE&ab_channel=GameDevBeginner

Probuilder (Map making):
https://www.youtube.com/watch?v=YtzIXCKr8Wo 

Line Renderer:
https://www.youtube.com/watch?v=3hYrNu-AKPo 

Sound:
https://www.youtube.com/watch?v=6OT43pvUyfY 
https://www.youtube.com/watch?v=eQphjWreQ0U 
https://answers.unity.com/questions/1213839/destroy-object-and-play-sound.html 
https://answers.unity.com/questions/1345501/how-can-i-get-functionality-similar-to-onvalidate.html 
https://stackoverflow.com/questions/70122642/unity-audio-singleton-across-scenes 

Camera Shake:
https://www.youtube.com/watch?v=ACf1I27I6Tk 

BGM and other sound effect:
https://www.youtube.com/watch?v=EciYWWDIgB8&ab_channel=TheGameGuy
https://www.youtube.com/watch?v=6OT43pvUyfY 

Inspector Attributes:
https://www.youtube.com/watch?v=9udeBeQiZSc 

How to get all child objects:
https://answers.unity.com/questions/594210/get-all-children-gameobjects.html 

Low Poly Water:
https://assetstore.unity.com/packages/tools/particles-effects/lowpoly-water-107563 

Sound Effects Used:
https://www.soundsnap.com/
https://www.youtube.com/watch?v=OqnM7RncEOI <- I used the first couple seconds for BlobHeal SFX

Music Used:
Cyberpunk Industrial Synthwave - More Human Than Human by Karl Casey @ White Bat Audio
Rick Astley - Never Gonna Give You Up

Dash VFX:
https://www.youtube.com/watch?v=RdNnbozAPGQ&ab_channel=HovlStudio

Jump VFX:
https://www.youtube.com/watch?v=4UXYexpzGOE&ab_channel=Hegamurl

Plexus and Plasma Effects:
https://assetstore.unity.com/packages/vfx/shaders/ultimate-10-shaders-168611 

Anonymous Pro Font:
https://www.marksimonson.com/fonts/view/anonymous-pro

Azonix font:
https://www.dafont.com/azonix.font

Planet Jouse font (game logo):
https://www.fontspace.com/planet-joust-font-f97528

TextWriter:
https://www.youtube.com/watch?v=ZVh4nH8Mayg 

NavMesh Tutorial:
https://youtu.be/CHV1ymlw-P8 

Post-processing Basics:
https://youtu.be/_PzYAbPpK8k 

Push Objects Tutorial:
https://youtu.be/3BOn2gs7z04 

Pillar Texture:
https://assetstore.unity.com/packages/2d/textures-materials/floors/dungeon-ground-texture-3329 

Pillar SFX:
https://www.soundsnap.com/iceblockcrackdistantmedium_03_wav 
https://www.soundsnap.com/rock_impact_colosseum_break_4_lo_wav 

Wall, Floor, Platform Textures:
https://assetstore.unity.com/packages/2d/textures-materials/concrete/yughues-free-concrete-materials-12951 

Furniture Asset Kit:
https://www.kenney.nl/assets/furniture-kit



# Known Issues

* FR Level 2 Circle Platform: Players can’t get hit when they’re standing in the centre of the platform due to the collider “hiding” them from the shooters. This is why they only get shot when jumping onto and off of the platform.
* Exit Portal: Players can get stuck against/beneath the bottom edges of the shape with no way to move/escape.
* Searchlight: Point light child objects sometimes rotate and look at walls and ceilings instead of continuously pointing on the floor while the NavMeshAgent is moving around.
* Level 3 - Key Room bug: Player can glitch through to the hidden room in Level 3.
* Texture Bug: Texture bug found in Level 2 in Area 4-1 where the door automatically opens just past the pushable cover blocks.
* Platform + Player bug: When a Player is located under a moving platform and collides with it between the floor and the platform, the Player gets stuck inside the moving platform and is unable to move at all.

# Acknowledgements

* Alexis C. Mendiola
* Charles Huang
* Mohammed Bajaman
* Parker Chen

