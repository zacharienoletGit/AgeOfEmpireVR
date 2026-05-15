using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Canvas canvas;
    public Text titleText;
    public Text statusText;
    public Text healthText;
    public Text woodText;
    public Text waveText;
    public Text hintText;
    public Button startButton;
    public Button buildButton;
    public Button restartButton;

    GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;

        if (canvas == null)
            CreateWorldUI();

        HookButtons();
        Refresh();
    }

    public void Configure(GameManager gameManager)
    {
        manager = gameManager;
    }

    public void Refresh()
    {
        if (manager == null)
            manager = GameManager.Instance;

        if (manager == null)
            return;

        if (titleText != null)
            titleText.text = "Empire Miniature VR";

        if (statusText != null)
            statusText.text = GetStatusText();

        if (healthText != null)
        {
            int health = manager.townHall != null ? manager.townHall.currentHealth : 0;
            int maxHealth = manager.townHall != null ? manager.townHall.maxHealth : 0;
            healthText.text = "TownHall HP: " + health + " / " + maxHealth;
        }

        if (woodText != null)
            woodText.text = "Wood: " + manager.wood + "   Score: " + manager.score;

        if (waveText != null)
        {
            int wave = manager.waveManager != null ? manager.waveManager.CurrentWave : 0;
            waveText.text = "Wave: " + wave + "   Enemies: " + manager.EnemiesAlive;
        }

        if (hintText != null)
            hintText.text = GetHintText();

        if (startButton != null)
            startButton.gameObject.SetActive(manager.CurrentState != GameManager.GameState.Playing);

        if (buildButton != null)
            buildButton.interactable = manager.IsPlaying;

        if (restartButton != null)
            restartButton.gameObject.SetActive(manager.CurrentState == GameManager.GameState.GameOver || manager.CurrentState == GameManager.GameState.Victory);
    }

    string GetStatusText()
    {
        if (manager == null)
            return "";

        if (manager.CurrentState == GameManager.GameState.MainMenu)
            return "Press Start pour commencer";

        if (manager.CurrentState == GameManager.GameState.GameOver)
            return "Game Over - le TownHall est detruit";

        if (manager.CurrentState == GameManager.GameState.Victory)
            return "Victoire - toute les vagues sont fini";

        if (manager.buildSystem != null && manager.buildSystem.IsBuildMode)
            return "Build mode: click sur le sol pour placer une tower";

        return "Defend le TownHall";
    }

    string GetHintText()
    {
        return "Mouse/trigger: select + move   B: build   R: restart   Space: start";
    }

    void HookButtons()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayButton();
                GameManager.Instance?.StartGame();
            });
        }

        if (buildButton != null)
        {
            buildButton.onClick.RemoveAllListeners();
            buildButton.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayButton();
                GameManager.Instance?.ToggleBuildMode();
            });
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayButton();
                GameManager.Instance?.RestartGame();
            });
        }
    }

    void CreateWorldUI()
    {
        EnsureEventSystem();

        GameObject canvasObject = new GameObject("Student_World_UI");
        canvasObject.transform.position = new Vector3(0f, 1.75f, 0.85f);
        canvasObject.transform.rotation = Quaternion.identity;
        canvasObject.transform.localScale = Vector3.one * 0.0025f;

        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.AddComponent<GraphicRaycaster>();

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(520f, 360f);

        GameObject panel = CreateImage("Panel", canvasObject.transform, new Color(0.08f, 0.08f, 0.08f, 0.72f));
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        titleText = CreateText("Title", panel.transform, new Vector2(0f, 130f), 30, TextAnchor.MiddleCenter);
        statusText = CreateText("Status", panel.transform, new Vector2(0f, 88f), 18, TextAnchor.MiddleCenter);
        healthText = CreateText("Health", panel.transform, new Vector2(0f, 45f), 18, TextAnchor.MiddleCenter);
        woodText = CreateText("Wood", panel.transform, new Vector2(0f, 15f), 18, TextAnchor.MiddleCenter);
        waveText = CreateText("Wave", panel.transform, new Vector2(0f, -15f), 18, TextAnchor.MiddleCenter);
        hintText = CreateText("Hint", panel.transform, new Vector2(0f, -140f), 14, TextAnchor.MiddleCenter);

        startButton = CreateButton("StartButton", panel.transform, "START", new Vector2(-135f, -72f));
        buildButton = CreateButton("BuildButton", panel.transform, "BUILD", new Vector2(0f, -72f));
        restartButton = CreateButton("RestartButton", panel.transform, "RESTART", new Vector2(135f, -72f));
    }

    void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() != null)
            return;

        GameObject eventObject = new GameObject("EventSystem");
        eventObject.AddComponent<EventSystem>();
        eventObject.AddComponent<InputSystemUIInputModule>();
    }

    GameObject CreateImage(string objectName, Transform parent, Color color)
    {
        GameObject imageObject = new GameObject(objectName);
        imageObject.transform.SetParent(parent, false);
        Image image = imageObject.AddComponent<Image>();
        image.color = color;
        return imageObject;
    }

    Text CreateText(string objectName, Transform parent, Vector2 anchoredPosition, int size, TextAnchor anchor)
    {
        GameObject textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);

        Text text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.alignment = anchor;
        text.color = Color.white;

        RectTransform rect = text.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(460f, 34f);
        rect.anchoredPosition = anchoredPosition;

        return text;
    }

    Button CreateButton(string objectName, Transform parent, string label, Vector2 anchoredPosition)
    {
        GameObject buttonObject = CreateImage(objectName, parent, new Color(0.25f, 0.35f, 0.25f, 0.95f));
        Button button = buttonObject.AddComponent<Button>();

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(110f, 42f);
        rect.anchoredPosition = anchoredPosition;

        Text labelText = CreateText("Text", buttonObject.transform, Vector2.zero, 18, TextAnchor.MiddleCenter);
        labelText.text = label;
        labelText.rectTransform.sizeDelta = rect.sizeDelta;

        return button;
    }
}
