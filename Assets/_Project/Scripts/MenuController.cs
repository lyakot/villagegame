using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [Header("UI элементы")]
    public GameObject menuPanel;

    // Переменная для отслеживания состояния паузы
    private bool isPaused = false;

    void Start()
    {
        if (menuPanel != null)
            menuPanel.SetActive(false);

        // Гарантируем, что при старте время идет нормально
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Проверяем нажатие на Esc
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Метод для включения паузы
    public void PauseGame()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true); // Показываем меню
            Time.timeScale = 0f;       // Останавливаем время в игре
            isPaused = true;
        }
    }

    // Метод для снятия с паузы (можно также повесить на кнопку "Продолжить")
    public void ResumeGame()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false); // Прячем меню
            Time.timeScale = 1f;        // Возвращаем нормальную скорость времени
            isPaused = false;
        }
    }

    public void ExitGame()
    {
        Debug.Log("Игра закрывается...");
        Application.Quit();
    }
}
