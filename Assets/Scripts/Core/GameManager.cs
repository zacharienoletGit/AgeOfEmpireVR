using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver,
        Victory
    }

    public static GameManager Instance { get; private set; }

    [Header("Main references")]
    public WaveManager waveManager;
    public GameUI gameUI;
    public BaseHealth townHall;
    public BuildSystem buildSystem;

    [Header("Student values")]
    public int startWood = 80;
    public int wood;
    public int score;

    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    public SelectableUnit SelectedUnit { get; private set; }
    public int EnemiesAlive { get; private set; }

    bool allWavesFinished;

    public bool IsPlaying
    {
        get { return CurrentState == GameState.Playing; }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        FindMissingReferences();
        wood = startWood;
        UpdateUI();
    }

    public void Configure(WaveManager wave, GameUI ui, BaseHealth baseToProtect, BuildSystem builder)
    {
        waveManager = wave;
        gameUI = ui;
        townHall = baseToProtect;
        buildSystem = builder;
    }

    public void StartGame()
    {
        FindMissingReferences();

        CurrentState = GameState.Playing;
        wood = startWood;
        score = 0;
        EnemiesAlive = 0;
        allWavesFinished = false;
        SelectedUnit = null;

        if (townHall != null)
            townHall.ResetHealth();

        if (buildSystem != null)
            buildSystem.ClearPlacedTowers();

        if (waveManager != null)
            waveManager.BeginWaves();

        UpdateUI();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterEnemy(EnemyAI enemy)
    {
        if (!IsPlaying || enemy == null)
            return;

        EnemiesAlive++;
        UpdateUI();
    }

    public void EnemyKilled(UnitCombat enemy)
    {
        if (!IsPlaying)
            return;

        EnemiesAlive = Mathf.Max(0, EnemiesAlive - 1);
        score += 10;
        wood += 10;

        CheckVictory();
        UpdateUI();
    }

    public void WavesFinished()
    {
        allWavesFinished = true;
        CheckVictory();
        UpdateUI();
    }

    public void GameOver()
    {
        if (CurrentState == GameState.GameOver)
            return;

        CurrentState = GameState.GameOver;
        SelectedUnit = null;
        UpdateUI();
    }

    public void SelectUnit(SelectableUnit unit)
    {
        if (!IsPlaying)
            return;

        if (SelectedUnit != null)
            SelectedUnit.SetSelected(false);

        SelectedUnit = unit;

        if (SelectedUnit != null)
            SelectedUnit.SetSelected(true);

        UpdateUI();
    }

    public void MoveSelectedUnit(Vector3 point)
    {
        if (SelectedUnit == null || !IsPlaying)
            return;

        SelectedUnit.MoveTo(point);
    }

    public void AttackWithSelectedUnit(UnitCombat target)
    {
        if (SelectedUnit == null || target == null || !IsPlaying)
            return;

        SelectedUnit.AttackTarget(target);
    }

    public bool TrySpendWood(int amount)
    {
        if (wood < amount)
            return false;

        wood -= amount;
        UpdateUI();
        return true;
    }

    public void ToggleBuildMode()
    {
        if (!IsPlaying || buildSystem == null)
            return;

        buildSystem.ToggleBuildMode();
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (gameUI != null)
            gameUI.Refresh();
    }

    void FindMissingReferences()
    {
        if (waveManager == null)
            waveManager = FindObjectOfType<WaveManager>();

        if (gameUI == null)
            gameUI = FindObjectOfType<GameUI>();

        if (townHall == null)
            townHall = FindObjectOfType<BaseHealth>();

        if (buildSystem == null)
            buildSystem = FindObjectOfType<BuildSystem>();
    }

    void CheckVictory()
    {
        if (allWavesFinished && EnemiesAlive <= 0 && CurrentState == GameState.Playing)
            CurrentState = GameState.Victory;
    }
}
