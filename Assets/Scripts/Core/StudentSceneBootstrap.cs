using System.Collections.Generic;
using UnityEngine;

public class StudentSceneBootstrap : MonoBehaviour
{
    // This creates a playable prototype if the scene is still mostly empty.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CreatePrototypeIfNeeded()
    {
        if (FindObjectOfType<GameManager>() != null)
            return;

        GameObject root = new GameObject("StudentGameRoot");

        AudioManager audioManager = root.AddComponent<AudioManager>();
        GameManager gameManager = root.AddComponent<GameManager>();
        WaveManager waveManager = root.AddComponent<WaveManager>();
        BuildSystem buildSystem = root.AddComponent<BuildSystem>();
        GameUI gameUI = root.AddComponent<GameUI>();
        VRPointerController pointer = root.AddComponent<VRPointerController>();

        Camera sceneCamera = Camera.main;
        if (sceneCamera == null)
            sceneCamera = CreateFallbackCamera();

        pointer.rayCamera = sceneCamera;

        CreateLights();
        CreateMiniMap();

        BaseHealth townHall = CreateTownHall();
        CreatePlayerUnit("PlayerUnit_1", new Vector3(-0.55f, 0.96f, 1.9f));
        CreatePlayerUnit("PlayerUnit_2", new Vector3(-0.3f, 0.96f, 2.15f));
        Transform[] spawns = CreateSpawnPoints();
        CreateSmallProps();

        gameManager.Configure(waveManager, gameUI, townHall, buildSystem);
        waveManager.Configure(gameManager, townHall, spawns);
        buildSystem.Configure(gameManager);
        gameUI.Configure(gameManager);

        // This variable is just here so Unity keeps the component creation clear.
        if (audioManager == null)
            Debug.Log("Audio manager missing but project still works.");
    }

    static Camera CreateFallbackCamera()
    {
        GameObject cameraObject = new GameObject("Fallback Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 1.65f, -1.3f);
        cameraObject.transform.LookAt(new Vector3(0f, 0.85f, 2.1f));
        return cameraObject.AddComponent<Camera>();
    }

    static void CreateLights()
    {
        if (FindObjectOfType<Light>() != null)
            return;

        GameObject lightObject = new GameObject("Student Sun");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.5f;
        lightObject.transform.rotation = Quaternion.Euler(45f, -35f, 0f);
    }

    static void CreateMiniMap()
    {
        if (FindObjectOfType<MapGround>() != null)
            return;

        GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cube);
        table.name = "WoodTable";
        table.transform.position = new Vector3(0f, 0.65f, 2.15f);
        table.transform.localScale = new Vector3(3.2f, 0.18f, 2.25f);
        ApplyMaterial(table, new Color(0.32f, 0.22f, 0.12f));

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "MapGround";
        ground.transform.position = new Vector3(0f, 0.78f, 2.15f);
        ground.transform.localScale = new Vector3(2.9f, 0.05f, 1.9f);
        ground.AddComponent<MapGround>();
        ApplyMaterial(ground, new Color(0.22f, 0.48f, 0.22f));

        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
        road.name = "EnemyRoad";
        road.transform.position = new Vector3(0f, 0.815f, 2.15f);
        road.transform.localScale = new Vector3(0.35f, 0.025f, 1.75f);
        road.AddComponent<MapGround>();
        ApplyMaterial(road, new Color(0.55f, 0.43f, 0.28f));
    }

    static BaseHealth CreateTownHall()
    {
        GameObject townHall = new GameObject("TownHall");
        townHall.transform.position = new Vector3(0f, 0.98f, 1.55f);

        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "TownHallBody";
        body.transform.SetParent(townHall.transform);
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(0.42f, 0.32f, 0.42f);
        ApplyMaterial(body, new Color(0.72f, 0.55f, 0.34f));

        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.name = "TownHallRoof";
        roof.transform.SetParent(townHall.transform);
        roof.transform.localPosition = new Vector3(0f, 0.24f, 0f);
        roof.transform.localScale = new Vector3(0.5f, 0.16f, 0.5f);
        roof.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        ApplyMaterial(roof, new Color(0.55f, 0.12f, 0.08f));

        return townHall.AddComponent<BaseHealth>();
    }

    static void CreatePlayerUnit(string objectName, Vector3 position)
    {
        GameObject unit = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        unit.name = objectName;
        unit.transform.position = position;
        unit.transform.localScale = new Vector3(0.16f, 0.2f, 0.16f);
        ApplyMaterial(unit, new Color(0.1f, 0.33f, 0.78f));

        UnitCombat combat = unit.AddComponent<UnitCombat>();
        combat.team = UnitCombat.Team.Player;
        combat.maxHealth = 45;
        combat.currentHealth = 45;
        combat.damage = 10;
        combat.attackRange = 0.35f;

        unit.AddComponent<SelectableUnit>();
    }

    static Transform[] CreateSpawnPoints()
    {
        List<Transform> points = new List<Transform>();
        Vector3[] positions =
        {
            new Vector3(-0.95f, 0.96f, 2.95f),
            new Vector3(0f, 0.96f, 3.05f),
            new Vector3(0.95f, 0.96f, 2.95f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject spawn = new GameObject("EnemySpawn_" + (i + 1));
            spawn.transform.position = positions[i];
            points.Add(spawn.transform);
        }

        return points.ToArray();
    }

    static void CreateSmallProps()
    {
        for (int i = 0; i < 10; i++)
        {
            float x = Random.Range(-1.25f, 1.25f);
            float z = Random.Range(1.45f, 2.85f);

            if (Mathf.Abs(x) < 0.25f)
                x += 0.55f;

            GameObject tree = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tree.name = "TinyTree";
            tree.transform.position = new Vector3(x, 0.91f, z);
            tree.transform.localScale = new Vector3(0.08f, 0.18f, 0.08f);
            ApplyMaterial(tree, new Color(0.12f, 0.36f, 0.12f));

            Collider treeCollider = tree.GetComponent<Collider>();
            if (treeCollider != null)
                treeCollider.enabled = false;
        }
    }

    public static void ApplyMaterial(GameObject target, Color color)
    {
        if (target == null)
            return;

        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null)
            return;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.color = color;
        renderer.sharedMaterial = material;
    }
}
