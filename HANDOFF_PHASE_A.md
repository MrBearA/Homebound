# Homebound — Phase A handoff (for Claude Code in Unity)

You are continuing work on **Homebound**, a Unity 6.4 (6000.4.6f1, URP, new Input System) 3D game.
The owner is a beginner learning C#. Keep changes small, explain what you did in plain words, and
**do not rewrite the student's scripts** beyond the exact edits listed in step 3.

## What already exists (do not recreate)
- Student scripts: `Assets/Scripts/Main Dog/` (DogMovement, DogEat, DogSniff, DogController),
  `Assets/Scripts/Interactions/` (FoodItem, HomeGoal), `Assets/Scripts/NPC Dogs/StrayDog.cs`,
  `Assets/Scripts/UI/` (UIManager, MainMenu).
- New teacher-written systems in `Assets/Scripts/Systems/`: `GameState` (static), `SceneFader`,
  `SceneExit`, `Memory`, `Talker`, `EndingTrigger`.
- Editor tool `Assets/Editor/BuildGddScenes.cs` → menu **Homebound > Build All GDD Scenes**.
  It copies `Assets/Scenes/Dev Test.unity` four times and builds the GDD level maps into
  `Scene1_Home`, `Scene2_Barangay`, `Scene3_Province`, `Scene4_NewTown`, then sets the build list to
  Main Menu, Scene1..4 (Dev Test and GamePlay disabled). Dev Test itself must not be modified.
- Older tool `Assets/Editor/BuildBarangayGreybox.cs` (builds GamePlay.unity) — leave it alone.

## Tasks, in order
1. **Compile.** Let Unity recompile and read the Console. If `BuildGddScenes.cs` or any `Systems/` script
   has compile errors, fix them with the smallest possible change and report what you changed.
2. **Run the builder:** execute the menu item `Homebound/Build All GDD Scenes` (it shows a confirm
   dialog; if you run it from code, call `BuildGddScenes.BuildAll()` logic without the dialog, or accept it).
   Then confirm the 4 scenes exist and the build list is:
   `0 Main Menu, 1 Scene1_Home, 2 Scene2_Barangay, 3 Scene3_Province, 4 Scene4_NewTown`.
3. **Three small code edits** (show the student the diff afterwards):
   - `Assets/Scripts/UI/MainMenu.cs`, in `PlayGame()`: call `GameState.ResetAll();` then
     `SceneManager.LoadScene("Scene1_Home");` (replace the old "GamePlay" load).
   - `Assets/Scripts/NPC Dogs/StrayDog.cs`, in `ReceiveFood`, right after `helped = true;` add
     `GameState.pipHelped = true;`
   - `Assets/Scripts/UI/UIManager.cs`, in `Update()`, the R-key restart: load `"Main Menu"` instead of
     reloading the active scene.
4. **Check each scene** (open it, enter Play mode, or inspect the hierarchy):
   - The dog starts on the map, the camera follows, the ground and edge walls block falling off.
   - Scene exits (objects under `Level/Exits` with `SceneExit`) lead to the next scene with a fade + title card.
   - Sniff (Space) shows paw prints along the route (`DogSniff.trailPoints` were regenerated).
   - Memories (`Level/Memories`) count up and show a line; NPC `TalkZone`s (`Talker`) speak.
   - Scene 3 has rain and Pip under the nipa hut; Scene 4 has 3 `EndingTrigger`s
     (New Pack is hidden unless `GameState.miloHelped && GameState.pipHelped`).
   Report anything broken, with the fix you made.
5. **Commit** with message `Phase A: GDD scenes, scene flow, memories, endings` and push.

## Known and intended (do NOT "fix" these — they are future lessons/phases)
- Scene 3 canal cannot be crossed: `CrossingBlocker` stays on until the student's plank lesson (Lesson 17).
- Brutus's `BrutusBlock` blocks the Scene 2 eskinita until the standoff (Phase C). The long way works.
- The Scene 2 `Collar` has no script yet (Lesson 14: Carryable base class).
- Milo's helped flag, Bark, food bubble, getting better with water, dog color/name select,
  the Scene 1 lipat-bahay event: not built yet.

## Coordinates (if you need to adjust the level)
1 Unity unit = 1 m. Map x = Unity x; map y (downwards on the GDD map) = Unity z = mapHeight − y.
Map sizes: Scene1 56×36, Scene2 84×56, Scene3 90×56, Scene4 70×46.
