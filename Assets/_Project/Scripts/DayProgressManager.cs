using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class DayProgressManager : MonoBehaviour
{
    // Структура для сохранения истории прошлых дней
    [System.Serializable]
    public struct DaySummary
    {
        public int dayNumber;
        public int fishCount;
        public int mushroomCount;
        public int berryCount;
        public int appleCount;
    }

    [Header("Настройки Отпуска")]
    public int currentDay = 1;
    public int totalVacationDays = 14;

    [Header("Статистика за ТЕКУЩИЙ день")]
    private int _fishCaughtToday = 0;
    private int _mushroomsGatheredToday = 0;
    private int _berriesGatheredToday = 0;
    private int _applesGatheredToday = 0;

    [Header("UI Дневника")]
    public GameObject journalUIContainer; // Объект самого блокнота на Canvas
    public TextMeshProUGUI leftPageText;  // Текст для левой страницы (Заметки)
    public TextMeshProUGUI rightPageText; // Текст для правой страницы (Трофеи)

    // Список, куда мы будем записывать итоги каждого прожитого дня
    private List<DaySummary> _vacationHistory = new List<DaySummary>();
    public List<DaySummary> VacationHistory => _vacationHistory;


    public bool TryGetDaySummary(int dayNumber, out DaySummary summary)
    {
        foreach (DaySummary day in _vacationHistory)
        {
            if (day.dayNumber == dayNumber)
            {
                summary = day;
                return true;
            }
        }

        summary = default;
        return false;
    }


    public void ShowDayStatistics(int dayNumber)
    {
        if (rightPageText == null) return;

        string rightPage = $"<b>Улов и сборы за день {dayNumber}:</b>\n\n";

        // Если выбран текущий день — берём ещё не завершённые счётчики
        if (dayNumber == currentDay)
        {
            rightPage += $"Поймано рыб: <b>{_fishCaughtToday}</b> шт.\n";
            rightPage += $"Найдено грибов: <b>{_mushroomsGatheredToday}</b> шт.\n";
            rightPage += $"Собрано ягод: <b>{_berriesGatheredToday}</b> шт.\n";
            rightPage += $"Собрано яблок: <b>{_applesGatheredToday}</b> шт.\n";
        }
        else if (TryGetDaySummary(dayNumber, out DaySummary summary))
        {
            rightPage += $"Поймано рыб: <b>{summary.fishCount}</b> шт.\n";
            rightPage += $"Найдено грибов: <b>{summary.mushroomCount}</b> шт.\n";
            rightPage += $"Собрано ягод: <b>{summary.berryCount}</b> шт.\n";
            rightPage += $"Собрано яблок: <b>{summary.appleCount}</b> шт.\n";
        }
        else
        {
            rightPage += "За этот день пока нет записей.";
        }

        rightPageText.text = rightPage;
    }

    public void LoadHistory(List<DaySummarySaveData> savedHistory)
    {
        _vacationHistory.Clear();

        foreach (DaySummarySaveData savedSummary in savedHistory)
        {
            DaySummary summary = new DaySummary
            {
                dayNumber = savedSummary.dayNumber,
                fishCount = savedSummary.fishCount,
                mushroomCount = savedSummary.mushroomCount,
                berryCount = savedSummary.berryCount,
                appleCount = savedSummary.appleCount
            };

            _vacationHistory.Add(summary);
        }

        currentDay = GameTime.Instance.CurrentDay;

        Debug.Log($"📖 Загружена история дневника: {_vacationHistory.Count} дн.");
    }
    private bool _isJournalOpen = false;

    public static DayProgressManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay += EndCurrentDay;
        }

        if (journalUIContainer != null)
            journalUIContainer.SetActive(false);

        UpdateJournalText();
    }

    void Update()
    {
        // Открытие/закрытие дневника на кнопку J
        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            ToggleJournal();
        }
    }


    public void ToggleJournal()
    {
        _isJournalOpen = !_isJournalOpen;
        if (journalUIContainer != null) journalUIContainer.SetActive(_isJournalOpen);

        if (_isJournalOpen)
        {
            UpdateJournalText();
            // По желанию: здесь можно блокировать движение игрока через PixelMovement, пока открыт дневник
        }
    }

    // Методы для добавления находок (будут вызываться из других скриптов)
    public void AddFish()
    {
        _fishCaughtToday++;
        Debug.Log($"[Дневник] Поймана рыба! Всего за день: {_fishCaughtToday}");
    }
    public void AddMushroom() => _mushroomsGatheredToday++;
    public void AddBerry() => _berriesGatheredToday++;
    public void AddApple() => _applesGatheredToday++;

    // Метод для генерации уютного текста в блокноте
    private void UpdateJournalText()
    {
        if (leftPageText == null || rightPageText == null) return;

        // --- ЛЕВАЯ СТРАНИЦА (Заметки) ---
        string leftPage = $"<b>День {currentDay}-й из {totalVacationDays}</b>\n";
        leftPage += "<i>Заметки на полях:</i>\n\n";

        // Динамический текст в зависимости от успехов
        if (_fishCaughtToday == 0 &&
            _mushroomsGatheredToday == 0 &&
            _berriesGatheredToday == 0)
        {
            leftPage += "Сегодня ленивый день. Просто наслаждаюсь тишиной и свежим воздухом...";
        }
        else if (_fishCaughtToday > 3)
        {
            leftPage += "Руки так и чешутся закинуть удочку снова! Рыба сегодня идёт как бешеная.";
        }
        else if (_mushroomsGatheredToday > 0 ||
                 _berriesGatheredToday > 0)
        {
            leftPage += "Славная вышла прогулка по лесу. Набрала полные карманы лесных даров.";
        }
        else
        {
            leftPage += "День проходит не зря, отпуск ощущается замечательно!";
        }

        // --- ПРАВАЯ СТРАНИЦА (Статистика) ---
        string rightPage = $"<b>Улов и сборы за день {currentDay}:</b>\n\n";
        rightPage += $"Поймано рыб: <b>{_fishCaughtToday}</b> шт.\n";
        rightPage += $"Найдено грибов: <b>{_mushroomsGatheredToday}</b> шт.\n";
        rightPage += $"Собрано ягод: <b>{_berriesGatheredToday}</b> шт.\n";
        rightPage += $"Собрано яблок: <b>{_applesGatheredToday}</b> шт.\n";

        leftPageText.text = leftPage;
        rightPageText.text = rightPage;
    }

    // Метод для завершения дня (вызывать, например, при взаимодействии с кроватью)
    public void EndCurrentDay()
    {
        DaySummary summary = new DaySummary
        {
            dayNumber = currentDay,
            fishCount = _fishCaughtToday,
            mushroomCount = _mushroomsGatheredToday,
            berryCount = _berriesGatheredToday,
            appleCount = _applesGatheredToday
        };

        _vacationHistory.Add(summary);

        _fishCaughtToday = 0;
        _mushroomsGatheredToday = 0;
        _berriesGatheredToday = 0;

        currentDay = GameTime.Instance.CurrentDay;

        UpdateJournalText();

        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.SaveGame();
        }

        Debug.Log($"📖 День {summary.dayNumber} записан в дневник.");
    }

    private void OnDestroy()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnNewDay -= EndCurrentDay;
        }
    }
}
