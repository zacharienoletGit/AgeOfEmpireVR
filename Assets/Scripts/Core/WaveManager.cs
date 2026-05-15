using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave setup")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float firstWaveDelay = 2f;
    public float spawnDelay = 0.8f;
    public float timeBetweenWaves = 2.5f;
    public int totalWaves = 3;

    public int CurrentWave { get; private set; }

    readonly List<GameObject> spawnedEnemies = new List<GameObject>();
    GameManager manager;
    BaseHealth townHall;
    Coroutine waveRoutine;

    void Awake()
    {
        manager = GameManager.Instance;
    }

    void Start()
    {
        if (manager == null)
            manager = GameManager.Instance;

        if (townHall == null)
            townHall = FindObjectOfType<BaseHealth>();
    }

    public void Configure(GameManager gameManager, BaseHealth baseToAttack, Transform[] points)
    {
        manager = gameManager;
        townHall = baseToAttack;
        spawnPoints = points;
    }

    public void BeginWaves()
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);

        ClearEnemies();
        CurrentWave = 0;
        waveRoutine = StartCoroutine(WaveRoutine());
    }

    public void ClearEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] != null)
                Destroy(spawnedEnemies[i]);
        }

        spawnedEnemies.Clear();
    }

    IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(firstWaveDelay);

        for (int wave = 1; wave <= totalWaves; wave++)
        {
            if (manager == null || !manager.IsPlaying)
                yield break;

            CurrentWave = wave;
            manager.UpdateUI();

            int enemyCount = 2 + wave;

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy(wave);
                yield return new WaitForSeconds(spawnDelay);
            }

            while (manager != null && manager.IsPlaying && manager.EnemiesAlive > 0)
                yield return null;

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        if (manager != null)
            manager.WavesFinished();
    }

    void SpawnEnemy(int wave)
    {
        Transform spawnPoint = GetSpawnPoint();
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : new Vector3(0f, 0.95f, 3.3f);

        GameObject enemyObject;

        if (enemyPrefab != null)
        {
            enemyObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            enemyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemyObject.name = "Enemy_Student_" + wave;
            enemyObject.transform.position = spawnPosition;
            enemyObject.transform.localScale = new Vector3(0.18f, 0.22f, 0.18f);
            StudentSceneBootstrap.ApplyMaterial(enemyObject, new Color(0.75f, 0.18f, 0.12f));
        }

        UnitCombat combat = enemyObject.GetComponent<UnitCombat>();
        if (combat == null)
            combat = enemyObject.AddComponent<UnitCombat>();

        combat.team = UnitCombat.Team.Enemy;
        combat.maxHealth = 25 + wave * 8;
        combat.damage = 5 + wave;
        combat.attackRange = 0.18f;
        combat.ResetHealth();

        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();
        if (enemyAI == null)
            enemyAI = enemyObject.AddComponent<EnemyAI>();

        enemyAI.targetBase = townHall;
        enemyAI.moveSpeed = 0.22f + wave * 0.03f;

        spawnedEnemies.Add(enemyObject);
    }

    Transform GetSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return null;

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
