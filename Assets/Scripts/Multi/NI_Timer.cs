using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PurrNet;

public class NI_Timer : NetworkIdentity
{
    [Header("Font")]
    public TMP_FontAsset tmpFont = null; // Assign your TMP font asset in Inspector
    [Header("Parameters")]
    public SyncVar<float> timeRemaining = new(120);
    public bool timerIsRunning = false;

    public TMP_Text timeText;

    void Start()
    {
        if(tmpFont == null)
            tmpFont = Resources.Load<TMP_FontAsset>("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset");
        createCanvas();
    }

    [ServerOnly]
    void Update()
    {
        if (timerIsRunning)
        {
            timeRemaining.value -= Time.deltaTime;
            DisplayTime(timeRemaining);

            if (timeRemaining.value <= 0f)
            {
                timeRemaining.value = 0f; // clamp à 0
                stopTimer();
            }
        }
    }

    /// <summary>
    /// Fonction qui lance le timer
    /// </summary>
    /// <param name="timeRemaining"></param>
    public void startTimer(float timeRemaining)
    {
        timerIsRunning = true;
        this.timeRemaining.value = timeRemaining;
    }

    [ObserversRpc]
    public void stopTimer()
    {
        timerIsRunning = false; // On passe � false le bool�en qui permet d'entrer dans la boucle
        timeText.text = ""; // On reset le texte
    }

    public void launchTimer()
    {
        timerIsRunning = true;
    }

    [ObserversRpc]
    public void DisplayTime(float timeToDisplay)
    {    
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if(!timeText)
            return;
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public void createCanvas()
    {
        GameObject canvasGO = new GameObject("RuntimeCanvas");
        canvasGO.AddComponent<NetworkIdentity>();

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler canvasScaler = canvasGO.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        canvasScaler.scaleFactor = 1;
        canvasScaler.referencePixelsPerUnit = 100;

        canvasGO.AddComponent<GraphicRaycaster>();

        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(canvasGO.transform);

        timeText = textGO.AddComponent<TextMeshProUGUI>();
        timeText.text = "";
        timeText.alignment = TextAlignmentOptions.Top;
        timeText.font = tmpFont;
        timeText.color = Color.white;

        // Set text position and size
        RectTransform rect = timeText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f); // Anchored to middle of top edge
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = new Vector2(200, 50);
        rect.anchoredPosition = new Vector2(0, -50);
    }
}
