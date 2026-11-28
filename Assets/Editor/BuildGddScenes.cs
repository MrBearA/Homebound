// Editor tool written by your teacher (Phase A).
// Menu: Homebound > Build All GDD Scenes
// Builds the 4 scenes from the GDD level maps (1 Unity unit = 1 meter), each from a copy of Dev Test,
// so the dog, camera and UI stay wired. Map x = Unity x, map y (down) = Unity z (up) flipped.
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BuildGddScenes
{
    const string Source = "Assets/Scenes/Dev Test.unity";
    const string Folder = "Assets/Scenes";
    const string MatFolder = "Assets/Arts/Greybox";
    const string PandesalPrefab = "Assets/Arts/3D assets/Prefab/Pandesal.prefab";
    const string RocksPrefab = "Assets/Arts/3D assets/Prefab/Rocks.prefab";

    public static readonly string[] SceneNames = { "Scene1_Home", "Scene2_Barangay", "Scene3_Province", "Scene4_NewTown" };

    static float W, H;
    static Transform level;
    static Scene current;

    // Materials
    static Material ground, street, path, backdrop, kanal, water, rice, grass, wood, roof, teal, church, tower,
        houseA, houseB, houseC, counter, truck, red, court, trunk, leaf, glow, person, milo, brutus, pip, window, rainMat;
    static Material[] flags;

    [MenuItem("Homebound/Build All GDD Scenes")]
    public static void BuildAll()
    {
        if (!EditorUtility.DisplayDialog("Build GDD scenes",
            "This creates (or replaces) Scene1_Home, Scene2_Barangay, Scene3_Province and Scene4_NewTown from a copy of Dev Test.\n\nDev Test is not changed.",
            "Build", "Cancel")) return;
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        MakeMaterials();
        Build(SceneNames[0], 56, 36, false, Scene1);
        Build(SceneNames[1], 84, 56, false, Scene2);
        Build(SceneNames[2], 90, 56, true, Scene3);
        Build(SceneNames[3], 70, 46, false, Scene4);
        UpdateBuildList();
        AssetDatabase.SaveAssets();
        EditorSceneManager.OpenScene(Folder + "/" + SceneNames[0] + ".unity");
        EditorUtility.DisplayDialog("Done", "4 scenes built and added to the build list after Main Menu.", "OK");
    }

    // ---------------------------------------------------------------- scenes

    static void Scene1()
    {
        Path("Road", new float[] { 0, 19, 56, 19 }, 6, street);
        Flat("Yard", 22, 3, 14, 12.5f, grass);
        RectBox("YourHouse", 24, 4, 10, 6, 4, teal);
        RectBox("YourHouse_Roof", 23.5f, 3.5f, 11, 7, 0.4f, roof, false, 4f);
        // Yard fence with the gate (closed) and the loose board (the way out)
        var fence = Group("Fence");
        WallLine("Fence_Top", fence, 22, 3, 36, 3, 1.2f, 0.15f, wood);
        WallLine("Fence_Left", fence, 22, 3, 22, 15.5f, 1.2f, 0.15f, wood);
        WallLine("Fence_Bottom_W", fence, 22, 15.5f, 28, 15.5f, 1.2f, 0.15f, wood);
        WallLine("Fence_Bottom_E", fence, 32, 15.5f, 36, 15.5f, 1.2f, 0.15f, wood);
        WallLine("Fence_Right_N", fence, 36, 3, 36, 8, 1.2f, 0.15f, wood);
        WallLine("Fence_Right_S", fence, 36, 10, 36, 15.5f, 1.2f, 0.15f, wood);
        RectBox("Gate", 28, 15.35f, 4, 0.3f, 1.4f, red, true, 0, true, fence);
        var board = RectBox("LooseBoard", 35.9f, 8, 0.15f, 2, 1.2f, wood, false, 0, true, fence);
        board.transform.rotation = Quaternion.Euler(0, 0, 25);
        Flat("SidePath", 36.5f, 3, 2, 13, path);
        RectBox("MovingTruck", 26, 17, 6, 3.4f, 3, truck);
        RectBox("MovingTruck_Cab", 32, 17.3f, 2, 2.8f, 2.2f, truck);
        Houses(new float[] { 2, 8, 8, 7, 0, 40, 8, 6, 6, 2, 2, 23, 10, 8, 1, 13.5f, 23, 8, 7, 0, 23, 23, 6, 6, 2, 30.5f, 23, 10, 8, 1, 42, 23, 8, 7, 0 });
        RectBox("Backdrop_N", 0, 0, 56, 2.2f, 4, backdrop);
        RectBox("Backdrop_S", 0, 33, 56, 3, 4, backdrop);

        var child = Person("Child", 30, 8, person, 0.75f, 180);
        AddTalker(child, "Anak", new[] {
            "{name}! Come here, boy! Fetch!",
            "Tomorrow is lipat-bahay. We're moving to the province, and you're coming with us!",
            "Stay close while we pack the truck, okay?" });
        AddMemory(33, 12, "The child throwing your ball in the yard, laughing every time you brought it back.");
        Exit("Exit_ToBarangay", 54, 16, 1.5f, 6, SceneNames[1], "Scene 2 · Barangay");
        Trail(new float[] { 25, 12, 35.6f, 9, 37.5f, 9, 37.5f, 19, 54, 19 });
        PlaceDog(25, 12, 90);
        SetLight(new Color(1f, 0.92f, 0.8f), 1.2f, new Vector3(50, -30, 0));
    }

    static void Scene2()
    {
        Path("Road", new float[] { 0, 28, 84, 28 }, 6, street);
        Path("NorthRoad", new float[] { 79, 25, 79, 0 }, 5, street);
        Path("Kanal", new float[] { 0, 31.6f, 84, 31.6f }, 0.6f, kanal, 0.015f);
        Flat("Eskinita_Up", 52, 6, 2, 19, path, 0.015f);
        Flat("Eskinita_East", 54, 6, 22.5f, 2, path, 0.015f);
        var walls = Group("EskinitaWalls");
        WallLine("Eskinita_W", walls, 51.8f, 0, 51.8f, 25, 3, 0.4f, backdrop);
        WallLine("Block_W", walls, 54.2f, 8.2f, 54.2f, 25, 3, 0.4f, backdrop);
        WallLine("Block_N", walls, 54.2f, 8.2f, 76.5f, 8.2f, 3, 0.4f, backdrop);
        WallLine("Block_E", walls, 76.5f, 8.2f, 76.5f, 25, 3, 0.4f, backdrop);
        WallLine("Block_S", walls, 54.2f, 25, 76.5f, 25, 3, 0.4f, backdrop);

        RectBox("SariSari", 8, 16, 10, 7, 3.5f, teal);
        RectBox("SariSari_Counter", 9, 23, 8, 0.6f, 0.9f, counter);
        Flat("BasketballCourt", 30, 34, 16, 11, court);
        RectBox("Hoop_W", 30.8f, 39.3f, 0.4f, 0.4f, 3, wood);
        RectBox("Hoop_E", 44.8f, 39.3f, 0.4f, 0.4f, 3, wood);
        RectBox("Kapilya", 62, 12, 10, 10, 7, teal);
        Flat("Garden", 2, 36, 10, 9, grass);
        Bush(5, 39, 1.4f); Bush(9, 42, 1.4f);
        Houses(new float[] { 20, 17.5f, 6, 6, 2, 27.5f, 16.5f, 8, 7, 0, 37, 15.5f, 10, 8, 1, 14, 33, 8, 7, 0, 48, 33, 10, 8, 1, 59.5f, 33, 6, 6, 2, 67, 33, 8, 7, 0 });
        RectBox("Backdrop_NW", 0, 0, 51, 8, 4, backdrop);
        RectBox("Backdrop_S", 0, 48, 84, 8, 4, backdrop);
        RectBox("Backdrop_N", 56, 0, 20, 5.5f, 4, backdrop);

        var nena = Person("AlingNena", 15.5f, 24.5f, person, 1f, 180);
        AddTalker(nena, "Aling Nena", new[] {
            "Ay, ang cute mong aso! Gutom ka? Here, take the pandesal.",
            "Pero huwag mong kainin 'yung fishball sa kanal ha. Madumi 'yan, magkakasakit ka!",
            "If you're not sure, sniff it first." });
        var miloDog = DogDummy("Milo", 20, 21, milo, 1f, 200);
        AddTalker(miloDog, "Milo", new[] {
            "Psst. New here, ano? I'm Milo.",
            "Your family's scent goes that way. Press SPACE to sniff and you'll see it.",
            "Careful in the eskinita. Brutus guards it. You can also take the long way around.",
            "And if you find my collar... I lost it in the garden south of the road." });
        Food(PandesalPrefab, "Pandesal", FoodType.Clean, 11, 29.6f);
        Food(RocksPrefab, "Fishball", FoodType.Dirty, 16, 31.6f);
        var collar = Prim(PrimitiveType.Cylinder, "Collar", level, U(7, 41, 0.1f), new Vector3(0.7f, 0.05f, 0.7f), red, false);
        collar.transform.SetParent(Group("Items"), true);
        AddMemory(38, 39.5f, "Watching the barangay basketball game from the child's lap, everyone cheering.");
        var brutusDog = DogDummy("Brutus", 53, 22, brutus, 1.4f, 180);
        AddTalker(brutusDog, "Brutus", new[] { "GRRRR... Bawal dito! This is MY eskinita!" }, 3.5f);
        var block = Trigger("BrutusBlock", 52, 21, 2, 2, 3);
        block.GetComponent<Collider>().isTrigger = false;   // solid until the standoff is won (Phase C)
        block.transform.SetParent(brutusDog, true);
        Exit("Exit_ToProvince", 76.5f, 0.5f, 5, 1.5f, SceneNames[2], "Scene 3 · Province road");
        Trail(new float[] { 1, 28, 53, 28, 53, 7, 79, 7, 79, 1 });
        PlaceDog(1.5f, 28, 90);
        SetLight(new Color(1f, 1f, 0.95f), 1.3f, new Vector3(55, 30, 0));
    }

    static void Scene3()
    {
        var fields = Group("RiceFields");
        Flat("Rice_1", 2, 2, 30, 20, rice, 0.01f, fields);
        Flat("Rice_2", 36, 2, 30, 16, rice, 0.01f, fields);
        Flat("Rice_3", 2, 26, 24, 14, rice, 0.01f, fields);
        Flat("Rice_4", 34, 34, 26, 20, rice, 0.01f, fields);
        Flat("Rice_5", 70, 8, 8, 40, rice, 0.01f, fields);
        // Irrigation canal: visible water + invisible walls, with a gap at the plank crossing
        Path("Canal", new float[] { 70, 0, 71, 20, 69, 38, 72, 56 }, 4, water, 0.03f);
        Path("CanalBlock_N", new float[] { 70, 0, 71, 20, 69, 38 }, 4.4f, null, 0, false, true, 3);
        Path("CanalBlock_S", new float[] { 70, 44, 72, 56 }, 4.4f, null, 0, false, true, 3);
        RectBox("CrossingBlocker", 67.3f, 38, 4.4f, 6, 3, null, true, 0, false);   // turned off when the plank is in place (Lesson 17)
        Path("Road_West", new float[] { 0, 46, 28, 44, 50, 32, 67, 24 }, 5, street);
        Path("Road_East", new float[] { 73, 22, 90, 20 }, 5, street);
        RectBox("BrokenBridge", 66, 21, 3, 6, 0.3f, red, false);
        var plank = RectBox("BambooPlank", 64.3f, 36.5f, 1.4f, 9, 0.2f, wood);
        var target = new GameObject("PlankTarget").transform;
        target.SetParent(level, false);
        target.position = U(71, 40.7f, 0.1f);
        target.rotation = Quaternion.Euler(0, 90, 0);
        plank.transform.SetParent(Group("Plank"), true);
        target.SetParent(plank.transform.parent, true);

        RectBox("Carinderia", 22, 48, 9, 5.5f, 3, teal);
        RectBox("Carinderia_Counter", 23, 47.4f, 7, 0.5f, 0.9f, counter);
        NipaHut(40, 22, 7, 6);
        RectBox("TrashBag", 32.4f, 51.8f, 1, 1, 0.9f, truck);
        Houses(new float[] { 8, 48, 8, 7, 0, 80, 24, 10, 8, 1, 80, 40, 6, 6, 2 });
        Tree(30, 30, 1.6f); Tree(33, 28, 1.6f); Tree(62, 48, 1.6f); Tree(58, 6, 1.6f); Tree(85, 50, 1.6f); Tree(4, 24, 1.6f);

        Food(PandesalPrefab, "Kanin at isda", FoodType.Clean, 26.5f, 47);
        Food(RocksPrefab, "Panis na kanin", FoodType.Spoiled, 34.2f, 51.5f);
        AddMemory(56, 46, "A road-trip stop with the family: everyone eating mangoes beside the rice fields.");
        var pipDog = Object.FindFirstObjectByType<StrayDog>();
        if (pipDog != null)
        {
            Vector3 p = U(43.5f, 25);
            p.y = pipDog.transform.position.y;
            pipDog.transform.SetPositionAndRotation(p, Quaternion.Euler(0, 180, 0));
        }
        Exit("Exit_ToNewTown", 88, 17.5f, 1.5f, 5, SceneNames[3], "Scene 4 · New town");
        Trail(new float[] { 1, 46, 28, 44, 45, 35, 60, 38, 67, 41, 75, 41, 79, 33, 76, 22, 88, 20 });
        PlaceDog(1.5f, 46, 85);
        SetLight(new Color(0.65f, 0.72f, 0.85f), 0.7f, new Vector3(35, 40, 0));
        Rain();
        var sniff = Object.FindFirstObjectByType<DogSniff>();
        if (sniff != null) { sniff.trailDuration = 2.5f; sniff.strongTrailDuration = 5f; EditorUtility.SetDirty(sniff); }
    }

    static void Scene4()
    {
        Path("Road", new float[] { 0, 30, 70, 30 }, 5, street);
        Path("CrossRoad", new float[] { 26, 0, 26, 46 }, 4, street);
        Flat("Plaza", 29, 13, 17, 12, grass);
        Tree(32, 16, 1.2f); Tree(43, 16, 1.2f);
        RectBox("Church", 30, 2, 12, 8, 6, church);
        RectBox("BellTower", 43, 2.5f, 4, 4, 11, tower);
        RectBox("NewHouse", 54, 34, 10, 8, 4.5f, teal);
        RectBox("NewHouse_Roof", 53.5f, 33.5f, 11, 9, 0.4f, roof, false, 4.5f);
        RectBox("NewHouse_Door", 57.2f, 33.8f, 1.6f, 0.25f, 2.2f, wood, false);
        RectBox("NewHouse_Window", 60, 33.85f, 2, 0.15f, 1.2f, window, false, 1.2f);
        Houses(new float[] { 2, 19, 8, 7, 2, 11.5f, 20, 6, 6, 2, 48, 18, 10, 8, 1, 59.5f, 19, 8, 7, 0, 2, 34, 6, 6, 2, 9.5f, 34, 8, 7, 0, 30, 34, 8, 7, 0, 39.5f, 34, 6, 6, 2 });
        RectBox("Backdrop_NW", 0, 0, 23, 8, 4, backdrop);
        RectBox("Backdrop_NE", 50, 0, 20, 14, 4, backdrop);
        RectBox("Backdrop_S", 0, 43, 70, 3, 4, backdrop);
        Banderitas();

        var endings = Group("Endings");
        var door = Trigger("Ending_Reunion", 56.8f, 32.8f, 2.4f, 1.2f, 3, endings);
        Ending(door, EndingType.Reunion, "REUNION",
            "The door opens. The child drops the leash and hugs you tight.\nLola cooks you a plate of rice and fish, and the family gives you a name tag with your new address.\nYou're home, {name}.");
        var corner = Trigger("Ending_NewPack", 28.4f, 33.4f, 2.2f, 2.2f, 3, endings);
        Ending(corner, EndingType.NewPack, "A NEW PACK",
            "You stay with Milo and Pip.\nFrom the window, the child sees you safe and happy, and leaves a bowl out for the three of you every night.");
        var miloW = DogDummy("Milo_Waiting", 29.2f, 34.2f, milo, 1f, 0); miloW.SetParent(corner.transform, true);
        var pipW = DogDummy("Pip_Waiting", 30.6f, 34.4f, pip, 0.8f, 330); pipW.SetParent(corner.transform, true);
        var road = Trigger("Ending_OpenRoad", 68, 27.5f, 1.5f, 5, 3, endings);
        Ending(road, EndingType.OpenRoad, "THE OPEN ROAD",
            "You rest on a hill at sunset and look at the road ahead.\nWherever it goes, you know you'll find your way.");
        Trail(new float[] { 1, 30, 58, 30, 58, 33.5f });
        PlaceDog(1.5f, 30, 90);
        SetLight(new Color(1f, 0.8f, 0.55f), 1.1f, new Vector3(20, -60, 0));
    }

    // ---------------------------------------------------------------- build helpers

    static void Build(string name, float w, float h, bool keepPip, System.Action content)
    {
        string scenePath = Folder + "/" + name + ".unity";
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath) != null) AssetDatabase.DeleteAsset(scenePath);
        AssetDatabase.CopyAsset(Source, scenePath);
        current = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        W = w; H = h;
        Cleanup(keepPip);

        level = new GameObject("Level").transform;
        RectBox("Ground", 0, 0, w, h, 1, ground, true, -1);
        var edges = Group("Edges");
        WallLine("Edge_N", edges, 0, 0, w, 0, 4, 0.5f, backdrop);
        WallLine("Edge_S", edges, 0, h, w, h, 4, 0.5f, backdrop);
        WallLine("Edge_W", edges, 0, 0, 0, h, 4, 0.5f, backdrop);
        WallLine("Edge_E", edges, w, 0, w, h, 4, 0.5f, backdrop);

        content();

        EditorSceneManager.MarkSceneDirty(current);
        EditorSceneManager.SaveScene(current);
    }

    static void Cleanup(bool keepPip)
    {
        foreach (var root in current.GetRootGameObjects())
            if (root.name == "Floor" || root.name == "Cube" || root.name == "Level") Object.DestroyImmediate(root);
        foreach (var hg in Object.FindObjectsByType<HomeGoal>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            Object.DestroyImmediate(hg.gameObject);
        foreach (var f in Object.FindObjectsByType<FoodItem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (f == null) continue;
            var rootGo = PrefabUtility.GetOutermostPrefabInstanceRoot(f.gameObject);
            Object.DestroyImmediate(rootGo != null ? rootGo : f.gameObject);
        }
        if (!keepPip)
            foreach (var s in Object.FindObjectsByType<StrayDog>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                Object.DestroyImmediate(s.gameObject);
    }

    static Vector3 U(float x, float y, float height = 0f)
    {
        return new Vector3(x, height, H - y);
    }

    static Transform Group(string name)
    {
        var t = level.Find(name);
        if (t != null) return t;
        t = new GameObject(name).transform;
        t.SetParent(level, false);
        return t;
    }

    static void Finish(GameObject go, Material mat, bool visible, bool collider)
    {
        if (!collider) { var c = go.GetComponent<Collider>(); if (c != null) Object.DestroyImmediate(c); }
        if (!visible) { var r = go.GetComponent<MeshRenderer>(); if (r != null) Object.DestroyImmediate(r); }
        else if (mat != null) go.GetComponent<Renderer>().sharedMaterial = mat;
    }

    static GameObject Prim(PrimitiveType type, string name, Transform parent, Vector3 worldPos, Vector3 scale, Material mat, bool collider)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = worldPos;
        go.transform.localScale = scale;
        Finish(go, mat, true, collider);
        return go;
    }

    static GameObject RectBox(string name, float x, float y, float w, float d, float height, Material mat,
        bool collider = true, float baseY = 0f, bool visible = true, Transform parent = null)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent != null ? parent : level, false);
        go.transform.position = U(x + w / 2f, y + d / 2f, baseY + height / 2f);
        go.transform.localScale = new Vector3(w, height, d);
        Finish(go, mat, visible, collider);
        return go;
    }

    static GameObject Flat(string name, float x, float y, float w, float d, Material mat, float yOff = 0.01f, Transform parent = null)
    {
        return RectBox(name, x, y, w, d, 0.02f, mat, false, yOff - 0.01f, true, parent);
    }

    static GameObject Trigger(string name, float x, float y, float w, float d, float height, Transform parent = null)
    {
        var go = RectBox(name, x, y, w, d, height, null, true, 0, false, parent);
        go.GetComponent<Collider>().isTrigger = true;
        return go;
    }

    static void WallLine(string name, Transform parent, float x1, float y1, float x2, float y2, float height, float thick, Material mat)
    {
        Vector3 a = U(x1, y1), b = U(x2, y2);
        Vector3 d = b - a;
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.position = (a + b) / 2f + Vector3.up * height / 2f;
        go.transform.rotation = Quaternion.LookRotation(d.normalized, Vector3.up);
        go.transform.localScale = new Vector3(thick, height, d.magnitude + thick);
        Finish(go, mat, true, true);
    }

    // A road/path/water strip along a polyline (x,y pairs). solid = invisible wall of the given height.
    static void Path(string name, float[] p, float width, Material mat, float yOff = 0.01f, bool visible = true, bool solid = false, float height = 0.02f)
    {
        var root = new GameObject(name).transform;
        root.SetParent(level, false);
        for (int i = 0; i + 3 < p.Length; i += 2)
        {
            Vector3 a = U(p[i], p[i + 1]), b = U(p[i + 2], p[i + 3]);
            Vector3 d = b - a;
            if (d.magnitude < 0.01f) continue;
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name + "_" + (i / 2);
            go.transform.SetParent(root, false);
            go.transform.position = (a + b) / 2f + Vector3.up * (solid ? height / 2f : yOff);
            go.transform.rotation = Quaternion.LookRotation(d.normalized, Vector3.up);
            go.transform.localScale = new Vector3(width, solid ? height : 0.02f, d.magnitude + width * 0.9f);
            Finish(go, mat, visible, solid);
        }
    }

    // Houses: groups of 5 numbers (x, y, w, h, type 0=A 1=B 2=C)
    static void Houses(float[] data)
    {
        var g = Group("Houses");
        Material[] mats = { houseA, houseB, houseC };
        float[] heights = { 4.5f, 5.5f, 4f };
        for (int i = 0; i + 4 < data.Length; i += 5)
        {
            int type = (int)data[i + 4];
            RectBox("House_" + "ABC"[type] + "_" + (i / 5 + 1), data[i], data[i + 1], data[i + 2], data[i + 3], heights[type], mats[type], true, 0, true, g);
        }
    }

    static Transform DogDummy(string name, float x, float y, Material mat, float s = 1f, float yaw = 0f)
    {
        var root = new GameObject(name).transform;
        root.SetParent(Group("Characters"), false);
        root.SetPositionAndRotation(U(x, y), Quaternion.Euler(0, yaw, 0));
        Prim(PrimitiveType.Cube, "Body", root, root.TransformPoint(new Vector3(0, 0.45f, 0) * s), new Vector3(0.6f, 0.6f, 1.1f) * s, mat, true);
        Prim(PrimitiveType.Cube, "Head", root, root.TransformPoint(new Vector3(0, 0.85f, 0.6f) * s), new Vector3(0.45f, 0.45f, 0.45f) * s, mat, false);
        Prim(PrimitiveType.Cube, "Tail", root, root.TransformPoint(new Vector3(0, 0.75f, -0.65f) * s), new Vector3(0.1f, 0.1f, 0.4f) * s, mat, false);
        return root;
    }

    static Transform Person(string name, float x, float y, Material mat, float s = 1f, float yaw = 0f)
    {
        var root = new GameObject(name).transform;
        root.SetParent(Group("Characters"), false);
        root.SetPositionAndRotation(U(x, y), Quaternion.Euler(0, yaw, 0));
        Prim(PrimitiveType.Capsule, "Body", root, root.position + Vector3.up * 0.85f * s, new Vector3(0.5f, 0.85f, 0.5f) * s, mat, true);
        return root;
    }

    static void AddTalker(Transform npc, string speaker, string[] lines, float radius = 3f)
    {
        var zone = new GameObject("TalkZone");
        zone.transform.SetParent(npc, false);
        var sc = zone.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = radius;
        var t = zone.AddComponent<Talker>();
        t.speakerName = speaker;
        t.lines = lines;
    }

    static void Tree(float x, float y, float r)
    {
        var g = Group("Trees");
        var root = new GameObject("Tree").transform;
        root.SetParent(g, false);
        root.position = U(x, y);
        Prim(PrimitiveType.Cylinder, "Trunk", root, root.position + Vector3.up * 1.2f, new Vector3(0.35f, 1.2f, 0.35f), trunk, true);
        Prim(PrimitiveType.Sphere, "Leaves", root, root.position + Vector3.up * (2.4f + r * 0.7f), Vector3.one * r * 2f, leaf, false);
    }

    static void Bush(float x, float y, float r)
    {
        Prim(PrimitiveType.Sphere, "Bush", Group("Trees"), U(x, y, r * 0.4f), new Vector3(r * 2f, r * 1.2f, r * 2f), leaf, true);
    }

    static void NipaHut(float x, float y, float w, float d)
    {
        var g = Group("NipaHut");
        float postH = 1.8f;
        float[,] corners = { { x + 0.3f, y + 0.3f }, { x + w - 0.3f, y + 0.3f }, { x + 0.3f, y + d - 0.3f }, { x + w - 0.3f, y + d - 0.3f } };
        for (int i = 0; i < 4; i++)
            Prim(PrimitiveType.Cylinder, "Post", g, U(corners[i, 0], corners[i, 1], postH / 2f), new Vector3(0.25f, postH / 2f, 0.25f), wood, true);
        RectBox("Floor", x, y, w, d, 0.25f, wood, true, postH, true, g);
        RectBox("BackWall", x, y, w, 0.15f, 1.6f, wood, true, postH + 0.25f, true, g);
        RectBox("Roof", x - 0.5f, y - 0.5f, w + 1, d + 1, 0.35f, roof, false, postH + 1.85f, true, g);
    }

    static void Food(string prefabPath, string label, FoodType type, float x, float y)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null) { Debug.LogWarning("Homebound: missing prefab " + prefabPath); return; }
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, current);
        go.name = label;
        go.transform.SetParent(Group("Food"), true);
        Vector3 p = U(x, y);
        p.y = prefab.transform.position.y;
        go.transform.position = p;
        var f = go.GetComponent<FoodItem>();
        if (f != null)
        {
            f.foodName = label;
            f.type = type;
            PrefabUtility.RecordPrefabInstancePropertyModifications(f);
        }
    }

    static void AddMemory(float x, float y, string line)
    {
        var go = Prim(PrimitiveType.Sphere, "Memory", Group("Memories"), U(x, y, 0.9f), Vector3.one * 0.6f, glow, true);
        var sc = go.GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = 2f;
        go.AddComponent<Memory>().line = line;
    }

    static void Exit(string name, float x, float y, float w, float d, string next, string title)
    {
        var go = Trigger(name, x, y, w, d, 4, Group("Exits"));
        var e = go.AddComponent<SceneExit>();
        e.nextScene = next;
        e.titleCard = title;
    }

    static void Ending(GameObject trigger, EndingType type, string title, string text)
    {
        var e = trigger.AddComponent<EndingTrigger>();
        e.ending = type;
        e.title = title;
        e.text = text;
    }

    static void Banderitas()
    {
        var g = Group("Banderitas");
        Vector3 a = U(0, 29, 4.8f), b = U(70, 29, 4.8f);
        var rope = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rope.name = "Rope";
        rope.transform.SetParent(g, false);
        rope.transform.position = (a + b) / 2f;
        rope.transform.localScale = new Vector3(70, 0.03f, 0.03f);
        Finish(rope, wood, true, false);
        int n = 0;
        for (float x = 1; x < 70; x += 1.5f)
        {
            var flag = Prim(PrimitiveType.Cube, "Flag", g, U(x, 29, 4.55f), new Vector3(0.6f, 0.45f, 0.03f), flags[n % flags.Length], false);
            flag.transform.rotation = Quaternion.Euler(0, 0, 45);
            n++;
        }
    }

    static void Trail(float[] p, float spacing = 6f)
    {
        var sniff = Object.FindFirstObjectByType<DogSniff>();
        if (sniff == null) { Debug.LogWarning("Homebound: no DogSniff found"); return; }
        var parentGo = GameObject.Find("ScentTrail");
        Transform parent = parentGo != null ? parentGo.transform : new GameObject("ScentTrail").transform;
        for (int i = parent.childCount - 1; i >= 0; i--) Object.DestroyImmediate(parent.GetChild(i).gameObject);

        var points = new List<Vector3> { U(p[0], p[1], 0.02f) };
        for (int i = 0; i + 3 < p.Length; i += 2)
        {
            Vector3 a = U(p[i], p[i + 1], 0.02f), b = U(p[i + 2], p[i + 3], 0.02f);
            int steps = Mathf.Max(1, Mathf.RoundToInt(Vector3.Distance(a, b) / spacing));
            for (int s = 1; s <= steps; s++) points.Add(Vector3.Lerp(a, b, s / (float)steps));
        }
        var arr = new Transform[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            var t = new GameObject("Trail " + (i + 1)).transform;
            t.SetParent(parent, false);
            t.position = points[i];
            arr[i] = t;
        }
        sniff.trailPoints = arr;
        EditorUtility.SetDirty(sniff);
    }

    static void PlaceDog(float x, float y, float yaw)
    {
        var dog = Object.FindFirstObjectByType<DogMovement>();
        if (dog == null) return;
        Vector3 p = U(x, y, 1f);
        dog.transform.SetPositionAndRotation(p, Quaternion.Euler(0, yaw, 0));
        Vector3 camPos = p - dog.transform.forward * 6f + Vector3.up * 3f;
        foreach (string n in new[] { "FreeLook Camera", "Main Camera" })
        {
            var c = GameObject.Find(n);
            if (c != null) { c.transform.position = camPos; c.transform.LookAt(p); }
        }
    }

    static void SetLight(Color color, float intensity, Vector3 euler)
    {
        foreach (var l in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
        {
            if (l.type != LightType.Directional) continue;
            l.color = color;
            l.intensity = intensity;
            l.transform.rotation = Quaternion.Euler(euler);
        }
    }

    static void Rain()
    {
        var dog = Object.FindFirstObjectByType<DogMovement>();
        if (dog == null) return;
        var go = new GameObject("Rain");
        go.transform.SetParent(dog.transform, false);
        go.transform.localPosition = new Vector3(0, 12, 0);
        go.transform.rotation = Quaternion.Euler(90, 0, 0);
        var ps = go.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = 1.2f;
        main.startSpeed = 18f;
        main.startSize = 0.05f;
        main.maxParticles = 4000;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.startColor = new Color(0.75f, 0.82f, 0.95f, 0.6f);
        var emission = ps.emission;
        emission.rateOverTime = 900f;
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(40, 40, 1);
        var r = go.GetComponent<ParticleSystemRenderer>();
        r.renderMode = ParticleSystemRenderMode.Stretch;
        r.lengthScale = 6f;
        r.sharedMaterial = rainMat;
    }

    static void UpdateBuildList()
    {
        var list = new List<EditorBuildSettingsScene> { new EditorBuildSettingsScene(Folder + "/Main Menu.unity", true) };
        foreach (var n in SceneNames) list.Add(new EditorBuildSettingsScene(Folder + "/" + n + ".unity", true));
        foreach (var s in EditorBuildSettings.scenes)
            if (!list.Exists(x => x.path == s.path)) list.Add(new EditorBuildSettingsScene(s.path, false));
        EditorBuildSettings.scenes = list.ToArray();
    }

    // ---------------------------------------------------------------- materials

    static void MakeMaterials()
    {
        ground = Mat("Ground", new Color(0.55f, 0.62f, 0.45f));
        street = Mat("Street", new Color(0.30f, 0.30f, 0.32f));
        path = Mat("Path", new Color(0.72f, 0.68f, 0.60f));
        backdrop = Mat("Backdrop", new Color(0.85f, 0.80f, 0.70f));
        kanal = Mat("Kanal", new Color(0.22f, 0.20f, 0.14f));
        water = Mat("Water", new Color(0.35f, 0.55f, 0.70f));
        rice = Mat("Rice", new Color(0.62f, 0.75f, 0.35f));
        grass = Mat("Grass", new Color(0.45f, 0.65f, 0.35f));
        wood = Mat("Wood", new Color(0.55f, 0.40f, 0.25f));
        roof = Mat("Roof", new Color(0.45f, 0.30f, 0.20f));
        teal = Mat("Landmark", new Color(0.18f, 0.44f, 0.45f));
        church = Mat("Church", new Color(0.92f, 0.90f, 0.85f));
        tower = Mat("BellTower", new Color(0.49f, 0.36f, 0.77f));
        houseA = Mat("House_A", new Color(0.90f, 0.55f, 0.45f));
        houseB = Mat("House_B", new Color(0.55f, 0.70f, 0.85f));
        houseC = Mat("House_C", new Color(0.95f, 0.85f, 0.55f));
        counter = Mat("Counter", new Color(0.55f, 0.35f, 0.20f));
        truck = Mat("Truck", new Color(0.30f, 0.29f, 0.27f));
        red = Mat("Red", new Color(0.76f, 0.25f, 0.18f));
        court = Mat("Court", new Color(0.80f, 0.72f, 0.58f));
        trunk = Mat("Trunk", new Color(0.40f, 0.28f, 0.18f));
        leaf = Mat("Leaves", new Color(0.35f, 0.55f, 0.30f));
        person = Mat("Person", new Color(0.95f, 0.75f, 0.55f));
        milo = Mat("Milo", new Color(0.55f, 0.40f, 0.25f));
        brutus = Mat("Brutus", new Color(0.15f, 0.15f, 0.15f));
        pip = Mat("Pip", new Color(0.95f, 0.90f, 0.80f));
        window = Mat("Window", new Color(1f, 0.9f, 0.55f), true);
        glow = Mat("MemoryGlow", new Color(1f, 0.85f, 0.45f), true);
        flags = new[] { Mat("Flag_Red", new Color(0.85f, 0.2f, 0.2f)), Mat("Flag_Yellow", new Color(0.95f, 0.8f, 0.2f)),
                        Mat("Flag_Blue", new Color(0.2f, 0.4f, 0.85f)), Mat("Flag_Green", new Color(0.3f, 0.7f, 0.3f)) };
        rainMat = Mat("Rain", new Color(0.75f, 0.82f, 0.95f), false, "Universal Render Pipeline/Particles/Unlit");
    }

    static Material Mat(string name, Color c, bool emissive = false, string shaderName = "Universal Render Pipeline/Lit")
    {
        if (!AssetDatabase.IsValidFolder(MatFolder)) AssetDatabase.CreateFolder("Assets/Arts", "Greybox");
        string p = MatFolder + "/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(p);
        if (m == null)
        {
            var shader = Shader.Find(shaderName);
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            m = new Material(shader);
            AssetDatabase.CreateAsset(m, p);
        }
        m.SetColor("_BaseColor", c);
        if (emissive)
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", c * 2f);
            m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
        EditorUtility.SetDirty(m);
        return m;
    }
}
