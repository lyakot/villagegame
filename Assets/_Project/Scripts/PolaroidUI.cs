using UnityEngine;
using TMPro;

public class PolaroidUI : MonoBehaviour
{
    [SerializeField] private RectTransform closingContainer;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timeText;

    private float fullHeight;
    private bool isClosed;

    private void Start()
    {
        GameTime.Instance.OnNewDay += OnNewDay;

        fullHeight = closingContainer.rect.height;

        dayText.text = $"День {GameTime.Instance.CurrentDay}";
        UpdateTimeText();
    }

    private void Update()
    {
        UpdateTimeText();

        if (isClosed)
            return;

        float progress = GameTime.Instance.GetDayProgress();

        if (progress >= 1f)
        {
            progress = 1f;

            Vector2 finalSize = closingContainer.sizeDelta;
            finalSize.y = fullHeight;
            closingContainer.sizeDelta = finalSize;

            isClosed = true;
            return;
        }

        Vector2 size = closingContainer.sizeDelta;
        size.y = fullHeight * progress;
        closingContainer.sizeDelta = size;
    }

    private void UpdateTimeText()
    {
        float currentHour = GameTime.Instance.CurrentHour;

        int hours = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hours) * 60f);

        timeText.text = $"{hours:00}:{minutes:00}";
    }

    private void OnNewDay()
    {
        isClosed = false;

        dayText.text = $"День {GameTime.Instance.CurrentDay}";
        UpdateTimeText();

        Debug.Log($"📷 Новый снимок: день {GameTime.Instance.CurrentDay}");
    }

    private void OnDestroy()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay -= OnNewDay;
        }
    }
}