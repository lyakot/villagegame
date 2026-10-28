
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalCalendar : MonoBehaviour
{
    [SerializeField] private Transform daysContainer;
    [SerializeField] private Button dayButtonPrefab;
    [SerializeField] private Transform weekDaysContainer;

    [Header("Цвета календаря")]
    [SerializeField] private Color normalDayColor = Color.white;
    [SerializeField]
    private Color currentDayColor =
        new Color(1f, 0.8f, 0.4f);
    [SerializeField]
    private Color selectedDayColor =
    new Color(0.4f, 0.8f, 1f);

    private const int TotalDays = 14;

    private static readonly string[] WeekDays =
    {
    "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"
};
    private readonly List<Button> dayButtons = new List<Button>();
    private int lastHighlightedDay = -1;
    private int selectedDay = -1;


    private void Start()
    {
        CreateCalendar();
        UpdateCurrentDayHighlight();

        SelectDay(GameTime.Instance.CurrentDay);
    }

    private void Update()
    {
        int currentDay = GameTime.Instance.CurrentDay;

        if (currentDay != lastHighlightedDay)
        {
            UpdateCurrentDayHighlight();
        }
    }


    private void CreateCalendar()
    {
        // Заголовки дней недели
        foreach (string weekDay in WeekDays)
        {
            GameObject header = new GameObject(
                "WeekDay_" + weekDay,
                typeof(RectTransform),
                typeof(TextMeshProUGUI)
            );

            header.transform.SetParent(weekDaysContainer, false);

            RectTransform headerRect = header.GetComponent<RectTransform>();
            headerRect.sizeDelta = new Vector2(50f, 20f);

            TextMeshProUGUI text = header.GetComponent<TextMeshProUGUI>();
            text.text = weekDay;
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 10;
            text.color = Color.black;
        }

        // Кнопки игровых дней
        for (int day = 1; day <= TotalDays; day++)
        {
            Button button = Instantiate(dayButtonPrefab, daysContainer);

            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.text = day.ToString();

            dayButtons.Add(button);

            int dayNumber = day;
            button.onClick.AddListener(() => SelectDay(dayNumber));
        }
    }

    private void UpdateCurrentDayHighlight()
    {
        int currentDay = GameTime.Instance.CurrentDay;

        for (int i = 0; i < dayButtons.Count; i++)
        {
            Button button = dayButtons[i];
            int dayNumber = i + 1;

            // Будущие дни недоступны
            button.interactable = dayNumber <= currentDay;

            if (button.targetGraphic != null)
            {
                button.targetGraphic.color =
                    dayNumber == currentDay
                        ? currentDayColor
                        : normalDayColor;
            }
        }

        lastHighlightedDay = currentDay;
    }

    private void SelectDay(int day)
    {
        selectedDay = day;

        for (int i = 0; i < dayButtons.Count; i++)
        {
            Button button = dayButtons[i];
            int dayNumber = i + 1;

            if (button.targetGraphic != null)
            {
                if (dayNumber == GameTime.Instance.CurrentDay)
                {
                    button.targetGraphic.color = currentDayColor;
                }
                else if (dayNumber == selectedDay)
                {
                    button.targetGraphic.color = selectedDayColor;
                }
                else
                {
                    button.targetGraphic.color = normalDayColor;
                }
            }
        }

        Debug.Log("Выбран день: " + selectedDay);

        DayProgressManager.Instance.ShowDayStatistics(selectedDay);
    }
}