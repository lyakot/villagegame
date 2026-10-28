using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    private bool isLoading;
    private string savePath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        savePath = Path.Combine(Application.persistentDataPath, "save.json");

        Debug.Log($"Путь сохранения: {savePath}");
    }

    private void Start()
    {
        isLoading = true;

        LoadGame();

        isLoading = false;

        InventoryManager.Instance.OnInventoryChanged += SaveGame;
    }
    public void SaveGame()
    {
        if (isLoading)
            return;

        SaveData saveData = new SaveData();

        saveData.currentDay = GameTime.Instance.CurrentDay;
        saveData.currentSeason = (int)GameTime.Instance.CurrentSeason;
        saveData.currentHour = GameTime.Instance.CurrentHour;

        foreach (DayProgressManager.DaySummary summary in DayProgressManager.Instance.VacationHistory)
        {
            DaySummarySaveData savedSummary = new DaySummarySaveData
            {
                dayNumber = summary.dayNumber,
                fishCount = summary.fishCount,
                mushroomCount = summary.mushroomCount,
                berryCount = summary.berryCount,
                appleCount = summary.appleCount
            };

            saveData.dayHistory.Add(savedSummary);
        }

        for (int i = 0; i < 24; i++)
        {
            ItemData item = InventoryManager.Instance.GetItem(i);

            if (item == null)
                continue;

            int amount = InventoryManager.Instance.GetAmount(i);

            InventorySaveData inventoryItem = new InventorySaveData
            {
                itemId = item.itemId,
                amount = amount
            };

            saveData.inventory.Add(inventoryItem);
        }

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);

        Debug.Log("Игра сохранена!");
        Debug.Log(json);
    }

    private ItemData FindItemById(string itemId)
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("Items");

        foreach (ItemData item in allItems)
        {
            if (item.itemId == itemId)
            {
                return item;
            }
        }

        Debug.LogWarning($"ItemData не найден: {itemId}");
        return null;
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("Файл сохранения не найден.");
            return;
        }

        string json = File.ReadAllText(savePath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        GameTime.Instance.LoadTime(
        saveData.currentDay,
        saveData.currentSeason,
        saveData.currentHour
        );

        if (DayProgressManager.Instance != null && saveData.dayHistory != null)
        {
            DayProgressManager.Instance.LoadHistory(saveData.dayHistory);
        }

        foreach (InventorySaveData savedItem in saveData.inventory)
        {
            ItemData item = FindItemById(savedItem.itemId);

            if (item != null)
            {
                InventoryManager.Instance.LoadItem(item, savedItem.amount);
            }
        }

        Debug.Log("Игра загружена!");
        Debug.Log(json);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            SaveGame();
        }

        if (Keyboard.current.f9Key.wasPressedThisFrame)
        {
            LoadGame();
        }
    }
}