using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class FishingMinigame : MonoBehaviour
{
    [Header("Глобальные менеджеры")]
    public DayProgressManager dayProgressManager;

    [Header("Ссылка на игрока")]
    public PixelMovement playerMovement;

    [Header("Контейнеры Интерфейса")]
    [Tooltip("Общий родительский объект для слайдера, рыбки и текста статуса рыбалки")]
    public GameObject fishingUIContainer;
    [Tooltip("Внешний текст-подсказка, который виден всегда во внешнем мире")]
    public TextMeshProUGUI worldPromptText;

    [Header("UI Элементы внутри контейнера")]
    public Slider tensionSlider;
    public Image fillImage;
    public RectTransform fishSprite;
    public TextMeshProUGUI statusText;

    [Header("Настройки Удочки")]
    public float pullSpeed = 90f;
    public float gravitySpeed = 70f;

    [Header("Активная роль Рыбы")]
    public float fishSpeed = 60f;
    public float fishEnergy = 100f;
    public float lineDurability = 100f;

    private bool _isFishing = false;
    private float _playerValue = 20f;
    private float _fishValue = 50f;

    private float _fishTarget;
    private float _fishTimer;
    private float _currentLineHealth;
    private float _maxFishEnergy;

    private float _catchZone = 10f;

    private Coroutine _fadeCoroutine;
    [SerializeField] private ItemData okun;
    [SerializeField] private ItemData pike;
    [SerializeField] private FishCatchUI fishCatchUI;
    void Start()
    {
        _maxFishEnergy = fishEnergy;

        // Прячем интерфейс мини-игры при запуске
        if (fishingUIContainer != null) fishingUIContainer.SetActive(false);

        // Показываем внешнюю надпись игроку
        ShowTemporaryPrompt("Нажми [E], чтобы закинуть удочку", 2f);
    }

    private void ShowTemporaryPrompt(string message, float visibleTime)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadePrompt(message, visibleTime));
    }
    private System.Collections.IEnumerator FadePrompt(string message, float visibleTime)
    {
        worldPromptText.gameObject.SetActive(true);
        worldPromptText.text = message;

        Color color = worldPromptText.color;
        color.a = 1f;
        worldPromptText.color = color;

        // Ждём
        yield return new WaitForSeconds(visibleTime);

        // Плавно исчезаем
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            worldPromptText.color = color;

            yield return null;
        }

        worldPromptText.gameObject.SetActive(false);
    }
    public void StartFishing()
    {
        _isFishing = true;
        _playerValue = 20f;
        _fishValue = 50f;
        fishEnergy = _maxFishEnergy;
        _currentLineHealth = lineDurability;

        // Блокируем персонажа
        if (playerMovement != null) playerMovement.isLocked = true;

        // ВКЛЮЧАЕМ интерфейс рыбалки и ПРЯЧЕМ внешнюю надпись
        if (fishingUIContainer != null) fishingUIContainer.SetActive(true);
        UpdatePromptText("", false);

        if (tensionSlider != null)
        {
            tensionSlider.minValue = 0f;
            tensionSlider.maxValue = 100f;
        }

        if (fillImage != null) fillImage.color = new Color(0.2f, 0.8f, 0.2f, 0.5f);
        UpdateStatusText("Удерживай полосу на рыбке!", Color.white);
    }

    void Update()
    {
        if (!_isFishing)
        {
            // Слушаем кнопку E во внешнем мире
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartFishing();
            }
            return;
        }

        // 1. ВЛИЯНИЕ ИГРОКА
        bool isPressingSpace = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        if (isPressingSpace) _playerValue += pullSpeed * Time.deltaTime;
        else _playerValue -= gravitySpeed * Time.deltaTime;

        _playerValue = Mathf.Clamp(_playerValue, 0f, 100f);
        tensionSlider.value = _playerValue;

        // 2. ДВИЖЕНИЕ РЫБКИ
        _fishTimer -= Time.deltaTime;
        if (_fishTimer <= 0f)
        {
            _fishTimer = Random.Range(0.6f, 1.4f);
            _fishTarget = Random.Range(15f, 85f);
        }

        _fishValue = Mathf.MoveTowards(_fishValue, _fishTarget, fishSpeed * Time.deltaTime);
        UpdateFishPosition();

        // 3. ПРОВЕРКА СОВПАДЕНИЯ
        bool isCatching = Mathf.Abs(_playerValue - _fishValue) <= _catchZone;

        if (isCatching)
        {
            fishEnergy -= 20f * Time.deltaTime;
            _currentLineHealth = Mathf.Min(_currentLineHealth + Time.deltaTime * 5f, lineDurability);

            if (fillImage != null) fillImage.color = new Color(0.2f, 0.8f, 0.2f, 0.5f);

            if (fishEnergy < 30f) UpdateStatusText("Рыба устала! ДожимаЙ!", Color.yellow);
            else UpdateStatusText("Тяни! Рыба подсечена", Color.green);
        }
        else
        {
            _currentLineHealth -= 25f * Time.deltaTime;
            if (fillImage != null) fillImage.color = new Color(0.9f, 0.1f, 0.1f, 0.5f);

            if (_playerValue > _fishValue) UpdateStatusText("Слишком сильно! ОТПУСТИ!", new Color(1f, 0.3f, 0.3f));
            else UpdateStatusText("Слабое натяжение! ТЯНИ!", new Color(1f, 0.3f, 0.3f));

            if (_currentLineHealth < 30f) UpdateStatusText("ЛЕСКА СЕЙЧАС ПОРВЕТСЯ!!!", Color.red);
        }

        if (fishEnergy <= 0f) EndFishing(true);
        else if (_currentLineHealth <= 0f) EndFishing(false);
    }

    private void EndFishing(bool isWin)
    {
        _isFishing = false;

        // Разблокируем игрока
        if (playerMovement != null) playerMovement.isLocked = false;

        // ВЫКЛЮЧАЕМ интерфейс самой рыбалки
        if (fishingUIContainer != null) fishingUIContainer.SetActive(false);

        // Показываем внешнюю надпись с результатом и инструкцией
        if (isWin)
        {
            if (dayProgressManager != null)
            {
                dayProgressManager.AddFish();
            }

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem(pike);
            }

            if (fishCatchUI != null)
            {
                fishCatchUI.ShowFish(okun);
            }

            UpdatePromptText("ПОЙМАНО! Нажми [E] для нового заброса", true);
            Debug.Log("Поймали!");
        }
        else
        {
            UpdatePromptText("СОРВАЛАСЬ... Нажми [E], чтобы попробовать снова", true);
            Debug.Log("Сорвалась!");
        }
    }


    private void UpdateStatusText(string message, Color textColor)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = textColor;
        }
    }

    private void UpdatePromptText(string message, bool isVisible)
    {
        if (worldPromptText != null)
        {
            worldPromptText.gameObject.SetActive(isVisible);
            worldPromptText.text = message;
        }
    }

    private void UpdateFishPosition()
    {
        if (fishSprite == null || tensionSlider == null) return;
        RectTransform sliderRect = tensionSlider.GetComponent<RectTransform>();
        bool isVertical = tensionSlider.direction == Slider.Direction.BottomToTop || tensionSlider.direction == Slider.Direction.TopToBottom;

        if (isVertical)
        {
            float height = sliderRect.rect.height;
            float localY = (_fishValue / 100f) * height - (height / 2f);
            fishSprite.anchoredPosition = new Vector2(0f, localY);
        }
        else
        {
            float width = sliderRect.rect.width;
            float localX = (_fishValue / 100f) * width - (width / 2f);
            fishSprite.anchoredPosition = new Vector2(localX, 0f);
        }
    }
}
