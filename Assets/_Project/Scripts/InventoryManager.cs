using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public System.Action OnInventoryChanged;
    [SerializeField] private HotbarController hotbar;
    [SerializeField] private int inventorySize = 24;

    private InventoryItem[] items;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        items = new InventoryItem[inventorySize];
    }


    public bool AddItem(ItemData item)
    {
        // Сначала ищем такой же предмет
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].item == item)
            {
                items[i].amount++;

                Debug.Log($"Добавлен {item.itemName}. Количество: {items[i].amount}");

                OnInventoryChanged?.Invoke();

                return true;
            }
        }

        // Если такого предмета нет — ищем пустую ячейку
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new InventoryItem(item, 1);

                Debug.Log($"Добавлен новый предмет: {item.itemName}");

                OnInventoryChanged?.Invoke();

                return true;
            }
        }

        Debug.Log("Инвентарь заполнен");
        return false;
    }


    public ItemData GetItem(int index)
    {
        if (index < 0 || index >= items.Length)
            return null;

        if (items[index] == null)
            return null;

        return items[index].item;
    }

    public int GetAmount(int index)
    {
        if (index < 0 || index >= items.Length)
            return 0;

        if (items[index] == null)
            return 0;

        return items[index].amount;
    }

    public void LoadItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return;

        // Если такой предмет уже есть — восстанавливаем его количество
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].item == item)
            {
                items[i].amount = amount;

                Debug.Log($"Загружен {item.itemName}. Количество: {amount}");

                OnInventoryChanged?.Invoke();

                return;
            }
        }

        // Если предмета ещё нет — создаём новый слот
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new InventoryItem(item, amount);

                Debug.Log($"Загружен новый предмет: {item.itemName}. Количество: {amount}");

                OnInventoryChanged?.Invoke();

                return;
            }
        }

        Debug.LogWarning($"Не удалось загрузить {item.itemName}: инвентарь заполнен.");
    }
}