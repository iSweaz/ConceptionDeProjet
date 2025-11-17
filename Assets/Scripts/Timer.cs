using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Font")]
    public TMP_FontAsset tmpFont; // Assign your TMP font asset in Inspector
    [Header("Parameters")]
    public float timeRemaining = 120;
    public bool timerIsRunning = true;

    TMP_Text timeText;

    void Start()
    {
        createCanvas();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void launchTimer()
    {
        timerIsRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    void createCanvas()
    {
        GameObject canvasGO = new GameObject("RuntimeCanvas");

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
