// Editor tool: builds the Barangay demo level into GamePlay.unity from a copy of Dev Test.
// Menu: Homebound > Build GamePlay Level. Runs only in the Unity Editor, never in the game.
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildBarangayGreybox
{
    const string SourceScene = "Assets/Scenes/Dev Test.unity";
    const string TargetScene = "Assets/Scenes/GamePlay.unity";
    const string OldBarangay = "Assets/Scenes/Barangay.unity";
    const string MatFolder = "Assets/Arts/Greybox";

    [MenuItem("Homebound/Build GamePlay Level")]
    public static void Build()
    {
        if (!EditorUtility.DisplayDialog("Build GamePlay level",
            "This replaces Assets/Scenes/GamePlay.unity with the Barangay demo level, built from a copy of Dev Test. Dev Test is not changed.",
            "Build", "Cancel")) return;
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(TargetScene) != null) AssetDatabase.DeleteAsset(TargetScene);
        if (AssetDatabase.LoadAssetAtPath<SceneAsset>(OldBarangay) != null) AssetDatabase.DeleteAsset(OldBarangay);

        AssetDatabase.CopyAsset(SourceScene, TargetScene);
        var scene = EditorSceneManager.OpenScene(TargetScene, OpenSceneMode.Single);

        // Remove the old test floor, hide the stray test cube.
        var oldFloor = GameObject.Find("Floor");
        if (oldFloor != null) Object.DestroyImmediate(oldFloor);
        var oldCube = GameObject.Find("Cube");
        if (oldCube != null && oldCube.transform.parent == null) oldCube.SetActive(false);

        // Colors
        Material ground = Mat("Ground", new Color(0.55f, 0.62f, 0.45f));
        Material street = Mat("Street", new Color(0.30f, 0.30f, 0.32f));
        Material kanal = Mat("Kanal", new Color(0.22f, 0.20f, 0.14f));
        Material wall = Mat("Backdrop", new Color(0.85f, 0.80f, 0.70f));
        Material houseA = Mat("House_A", new Color(0.90f, 0.55f, 0.45f));
        Material houseB = Mat("House_B", new Color(0.55f, 0.70f, 0.85f));
        Material houseC = Mat("House_C", new Color(0.95f, 0.85f, 0.55f));
        Material store = Mat("SariSari", new Color(0.95f, 0.75f, 0.20f));
        Material counter = Mat("Counter", new Color(0.55f, 0.35f, 0.20f));
        Material[] houses = { houseA, houseB, houseC };

        var level = new GameObject("Level").transform;

        // Ground and backdrop walls (40 x 30 m)
        Box("Ground", level, new Vector3(0, -0.5f, 0), new Vector3(40, 1, 30), ground);
        var walls = new GameObject("Walls").transform; walls.SetParent(level, false);
        Box("Wall_N", walls, new Vector3(0, 2, 15), new Vector3(40, 4, 0.5f), wall);
        Box("Wall_S", walls, new Vector3(0, 2, -15), new Vector3(40, 4, 0.5f), wall);
        Box("Wall_E", walls, new Vector3(20, 2, 0), new Vector3(0.5f, 4, 30), wall);
        Box("Wall_W", walls, new Vector3(-20, 2, 0), new Vector3(0.5f, 4, 30), wall);

        // Street (west-east), eskinita (turns north to Home), kanal along the south edge
        var roads = new GameObject("Roads").transform; roads.SetParent(level, false);
        Box("Street", roads, new Vector3(-1, 0.01f, 0), new Vector3(38, 0.02f, 5), street, false);
        Box("Eskinita", roads, new Vector3(16, 0.01f, 8.75f), new Vector3(4, 0.02f, 12.5f), street, false);
        Box("Kanal", roads, new Vector3(-1, 0.015f, -3), new Vector3(38, 0.03f, 0.8f), kanal, false);

        // Houses: north row, south row, and one closing the east side of the eskinita
        var houseRoot = new GameObject("Houses").transform; houseRoot.SetParent(level, false);
        float[] northX = { -16, -9, 5 };
        float[] northH = { 5, 4, 6 };
        for (int i = 0; i < northX.Length; i++)
            Box("House_N" + (i + 1), houseRoot, new Vector3(northX[i], northH[i] / 2f, 7), new Vector3(6, northH[i], 7), houses[i % 3]);
        Box("House_N4", houseRoot, new Vector3(10.5f, 2.5f, 7), new Vector3(5, 5, 7), houses[1]);
        Box("House_E", houseRoot, new Vector3(19.2f, 2.5f, 8.75f), new Vector3(1.6f, 5, 12.5f), houses[2]);
        float[] southX = { -16, -9, -2, 5, 12 };
        float[] southH = { 4, 5, 4.5f, 6, 4 };
        for (int i = 0; i < southX.Length; i++)
            Box("House_S" + (i + 1), houseRoot, new Vector3(southX[i], southH[i] / 2f, -7), new Vector3(6, southH[i], 7), houses[(i + 1) % 3]);
        Box("House_S6", houseRoot, new Vector3(17.5f, 2.5f, -7), new Vector3(4, 5, 7), houses[0]);

        // Sari-sari store with a counter facing the street
        var storeRoot = new GameObject("SariSari Store").transform; storeRoot.SetParent(level, false);
        Box("Store", storeRoot, new Vector3(-2, 1.75f, 6), new Vector3(6, 3.5f, 5), store);
        Box("Counter", storeRoot, new Vector3(-2, 0.45f, 3.1f), new Vector3(5, 0.9f, 0.6f), counter);

        // Move gameplay objects into place (keeping their own heights)
        var dog = Object.FindFirstObjectByType<DogMovement>();
        if (dog != null) { Place(dog.transform, -16, 0); dog.transform.rotation = Quaternion.Euler(0, 90, 0); }

        var pip = Object.FindFirstObjectByType<StrayDog>();
        if (pip != null) { Place(pip.transform, 3, 2); pip.transform.rotation = Quaternion.Euler(0, 180, 0); }

        var home = Object.FindFirstObjectByType<HomeGoal>();
        if (home != null) Place(home.transform, 16, 12);

        int safe = 0, bad = 0;
        foreach (var food in Object.FindObjectsByType<FoodItem>(FindObjectsSortMode.None))
        {
            if (food.IsSafe()) { Place(food.transform, -3 + safe * 1.2f, 2.0f); safe++; }
            else { Place(food.transform, 6 + bad * 2f, -3); bad++; }
        }

        // Scent trail points: spread evenly along the route start -> street -> eskinita
        var sniff = Object.FindFirstObjectByType<DogSniff>();
        if (sniff != null && sniff.trailPoints != null)
        {
            var route = new List<Vector2> { new Vector2(-13, 0), new Vector2(16, 0), new Vector2(16, 9) };
            int n = sniff.trailPoints.Length;
            for (int i = 0; i < n; i++)
            {
                if (sniff.trailPoints[i] == null) continue;
                float t = n == 1 ? 0 : i / (float)(n - 1);
                Vector2 p = AlongRoute(route, t);
                Place(sniff.trailPoints[i], p.x, p.y);
            }
        }

        // Put Barangay first in Build Profiles so it is the scene that runs in a build
        var list = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        list.RemoveAll(s => s.path == TargetScene || s.path == OldBarangay);
        list.Insert(0, new EditorBuildSettingsScene(TargetScene, true));
        EditorBuildSettings.scenes = list.ToArray();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("GamePlay level built: " + safe + " safe food, " + bad + " bad food placed. Saved to " + TargetScene);
    }

    static void Place(Transform t, float x, float z)
    {
        t.position = new Vector3(x, t.position.y, z);
    }

    static Vector2 AlongRoute(List<Vector2> pts, float t)
    {
        float total = 0;
        for (int i = 1; i < pts.Count; i++) total += Vector2.Distance(pts[i - 1], pts[i]);
        float d = t * total;
        for (int i = 1; i < pts.Count; i++)
        {
            float seg = Vector2.Distance(pts[i - 1], pts[i]);
            if (d <= seg) return Vector2.Lerp(pts[i - 1], pts[i], seg == 0 ? 0 : d / seg);
            d -= seg;
        }
        return pts[pts.Count - 1];
    }

    static GameObject Box(string name, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool collider = true)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        if (!collider) Object.DestroyImmediate(go.GetComponent<Collider>());
        go.isStatic = true;
        return go;
    }

    static Material Mat(string name, Color c)
    {
        if (!AssetDatabase.IsValidFolder(MatFolder)) AssetDatabase.CreateFolder("Assets/Arts", "Greybox");
        string path = MatFolder + "/" + name + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (m == null)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            m = new Material(shader);
            AssetDatabase.CreateAsset(m, path);
        }
        m.SetColor("_BaseColor", c);
        EditorUtility.SetDirty(m);
        return m;
    }
}
