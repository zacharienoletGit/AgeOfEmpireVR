using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 35;
    public bool IsBuildMode { get; private set; }

    readonly List<GameObject> placedTowers = new List<GameObject>();
    GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;
    }

    public void Configure(GameManager gameManager)
    {
        manager = gameManager;
    }

    public void ToggleBuildMode()
    {
        IsBuildMode = !IsBuildMode;
    }

    public void CancelBuildMode()
    {
        IsBuildMode = false;
    }

    public bool TryPlaceTower(Vector3 position)
    {
        if (!IsBuildMode)
            return false;

        if (manager == null)
            manager = GameManager.Instance;

        if (manager == null || !manager.TrySpendWood(towerCost))
            return false;

        Vector3 towerPosition = new Vector3(position.x, position.y + 0.18f, position.z);
        GameObject towerObject;

        if (towerPrefab != null)
        {
            towerObject = Instantiate(towerPrefab, towerPosition, Quaternion.identity);
        }
        else
        {
            towerObject = CreateBasicTower(towerPosition);
        }

        placedTowers.Add(towerObject);
        IsBuildMode = false;
        AudioManager.Instance?.PlayBuild();
        manager.UpdateUI();
        return true;
    }

    public void ClearPlacedTowers()
    {
        for (int i = placedTowers.Count - 1; i >= 0; i--)
        {
            if (placedTowers[i] != null)
                Destroy(placedTowers[i]);
        }

        placedTowers.Clear();
        IsBuildMode = false;
    }

    GameObject CreateBasicTower(Vector3 position)
    {
        GameObject root = new GameObject("DefenseTower_Student");
        root.transform.position = position;

        GameObject basePart = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        basePart.name = "TowerBase";
        basePart.transform.SetParent(root.transform);
        basePart.transform.localPosition = Vector3.zero;
        basePart.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
        StudentSceneBootstrap.ApplyMaterial(basePart, new Color(0.45f, 0.38f, 0.28f));

        GameObject topPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
        topPart.name = "TowerTop";
        topPart.transform.SetParent(root.transform);
        topPart.transform.localPosition = new Vector3(0f, 0.22f, 0f);
        topPart.transform.localScale = new Vector3(0.28f, 0.12f, 0.28f);
        StudentSceneBootstrap.ApplyMaterial(topPart, new Color(0.65f, 0.55f, 0.38f));

        root.AddComponent<TowerDefense>();
        return root;
    }
}
