using UnityEngine;

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public enum TimeOfDay
{
    Sunrise,
    Day,
    Sunset,
    Night
}
public class GameTime : MonoBehaviour
{
    public static GameTime Instance;

    [Header("Day")]
    [SerializeField] private int currentDay = 1;

    [Header("Season")]
    [SerializeField] private int daysPerSeason = 22;
    [SerializeField] private Season currentSeason = Season.Spring;
    public Season CurrentSeason => currentSeason;

    [Header("Time Of Day")]
    [SerializeField] private TimeOfDay currentTimeOfDay = TimeOfDay.Sunrise;

    public TimeOfDay CurrentTimeOfDay => currentTimeOfDay;

    [Header("Time")]
    [SerializeField] private float dayStartHour = 6f;
    [SerializeField] private float dayEndHour = 22f;

    [Header("Speed")]
    [SerializeField] private float realSecondsPerGameHour = 60f;

    private float currentHour;

    public int CurrentDay => currentDay;
    public float CurrentHour => currentHour;

    public System.Action OnNewDay;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentHour = dayStartHour;
        currentTimeOfDay = TimeOfDay.Sunrise;
    }

    private void Update()
    {
        currentHour += Time.deltaTime / realSecondsPerGameHour;

        if (currentHour >= 12f && currentTimeOfDay == TimeOfDay.Sunrise)
        {
            currentTimeOfDay = TimeOfDay.Day;

            Debug.Log($"☀️ Наступил день: {currentHour:F1}, TimeOfDay = {currentTimeOfDay}");
        }

        if (currentHour >= dayEndHour)
        {
            StartNewDay();
        }
    }

    private void StartNewDay()
    {
        currentDay++;

        if (currentDay > daysPerSeason)
        {
            currentDay = 1;
            ChangeSeason();
        }

        currentHour = dayStartHour;
        currentTimeOfDay = TimeOfDay.Sunrise;

        Debug.Log($"🌅 Начался новый день: {currentDay}");

        OnNewDay?.Invoke();
    }

    public float GetDayProgress()
    {
        return Mathf.InverseLerp(dayStartHour, dayEndHour, currentHour);
    }

    private void ChangeSeason()
    {
        currentSeason++;

        if ((int)currentSeason > (int)Season.Winter)
        {
            currentSeason = Season.Spring;
        }

        Debug.Log($"🌿 Наступил новый сезон: {currentSeason}");
    }

    public void LoadTime(int day, int season, float hour)
    {
        currentDay = day;
        currentSeason = (Season)season;
        currentHour = hour;

        if (currentHour >= 20f)
        {
            currentTimeOfDay = TimeOfDay.Sunset;
        }
        else if (currentHour >= 12f)
        {
            currentTimeOfDay = TimeOfDay.Day;
        }
        else
        {
            currentTimeOfDay = TimeOfDay.Sunrise;
        }

        Debug.Log(
            $"Время загружено: день {currentDay}, сезон {currentSeason}, время {currentHour:F2}, TimeOfDay = {currentTimeOfDay}"
        );
    }
}