# eurbanities2
EUrbanities 2.0 game

# some question for developers:
START MENU:
- OPTION, volume, text speed and resolution, language is missing; there are some indication on how it work or how to finalize this menu?
- INFO, can you sent me the info I had to add into the game?

AUDIO: fmod give me always an error "DllNotFoundException: fmodstudioL
FMOD.Memory.GetStats (System.Int32& currentalloced, System.Int32& maxalloced, System.Boolean blocking) (at Assets/Plugins/FMOD/src/Runtime/wrapper/fmod.cs:890)
FMODUnity.RuntimeUtils.EnforceLibraryOrder () (at Assets/Plugins/FMOD/src/Runtime/RuntimeUtils.cs:365)
FMODUnity.EditorUtils.CreateSystem () (at Assets/Plugins/FMOD/src/Editor/EditorUtils.cs:235)
FMODUnity.EditorUtils.get_System () (at Assets/Plugins/FMOD/src/Editor/EditorUtils.cs:311)
FMODUnity.EventManager.UpdateCache () (at Assets/Plugins/FMOD/src/Editor/EventManager.cs:144)
FMODUnity.EventManager.RefreshBanks () (at Assets/Plugins/FMOD/src/Editor/EventManager.cs:30)
FMODUnity.EventManager.Startup () (at Assets/Plugins/FMOD/src/Editor/EventManager.cs:458)
UnityEngine.Debug:LogException(Exception)
FMODUnity.EventManager:Startup() (at Assets/Plugins/FMOD/src/Editor/EventManager.cs:462)
UnityEditor.EditorApplication:Internal_CallDelayFunctions() (at /Users/bokken/buildslave/unity/build/Editor/Mono/EditorApplication.cs:341)"
there is some specific configuration?

MOBILE: how it works? 
- which are the function and variable it manage?
- why is not in the minigames?

SAVING SYSTEM: how it works and where and how the variable are stored?
which are the scene/script dedicated?

DIALOGUE SYSTEM: 
- May I have a little guide on how to create dialogue an monologue and how to manage the variables? 

MINIGAME:
- from where I start the minigame? It's clear the accessibility once in the smartphone but the others? Are they rooms in the CC?
- the gardening once doesn't work

CAM SYSTEM:
- what do you use? have you some indication/documentation? 
