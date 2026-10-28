using UnityEngine;

public class CatchUI : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private FishCatchUI fishCatchUI;
    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += Refresh;
            Refresh();

            foreach (InventorySlot slot in slots)
            {
                slot.OnItemClicked += ShowFishInfo;
            }
        }
    }


    private void ShowFishInfo(ItemData fish)
    {
        if (fishCatchUI != null)
        {
            fishCatchUI.ShowFish(fish);
        }
    }


    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
        }

        foreach (InventorySlot slot in slots)
        {
            slot.OnItemClicked -= ShowFishInfo;
        }
    }

    public void Refresh()
    {
        int catchSlotIndex = 0;

        // Сначала очищаем все слоты улова
        foreach (InventorySlot slot in slots)
        {
            slot.SetItem(null);
        }

        // Просматриваем обычный инвентарь
        for (int i = 0; i < 24; i++)
        {
            ItemData item = InventoryManager.Instance.GetItem(i);

            if (item == null)
                continue;

            // Берём только рыбу
            if (item.itemType != ItemType.Fish)
                continue;

            if (catchSlotIndex >= slots.Length)
                break;

            int amount = InventoryManager.Instance.GetAmount(i);

            slots[catchSlotIndex].SetItem(item, amount);
            catchSlotIndex++;
        }
    }
}