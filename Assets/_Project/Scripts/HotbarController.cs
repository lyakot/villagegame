using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform[] slots;      // Сюда перетащим Slot_1, Slot_2... из иерархии
    public Transform selector;     // Сюда перетащим объект Selector

    public HotbarSlot[] hotbarSlots;

    private int _currentSlotIndex = 0;

    void Start()
    {
        // При старте привязываем рамку к первому слоту
        UpdateSelectorPosition();
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectSlot(0);
            if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectSlot(1);
            if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectSlot(2);
            if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectSlot(3);
            if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectSlot(4);
        }
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        _currentSlotIndex = index;

        Debug.Log("Выбран слот: " + _currentSlotIndex);

        UpdateSelectorPosition();
    }


    private void UpdateSelectorPosition()
    {
        if (selector != null && slots.Length > 0)
        {
            // Меняем родителя для рамки выделения, и она сама прыгает на новый слот
            selector.SetParent(slots[_currentSlotIndex], false);

            // Сбрасываем локальные координаты в ноль, чтобы она встала ровно по центру слота
            selector.SetAsFirstSibling();   // Теперь Selector будет под Icon
            selector.localPosition = Vector3.zero;
        }
    }

    public bool AddItem(ItemData item)
    {
        Debug.Log($"Добавляем предмет {item.itemName}");

        foreach (var slot in hotbarSlots)
        {
            if (slot.IsEmpty())
            {
                Debug.Log("Найден свободный слот");

                slot.SetItem(item);
                return true;
            }
        }

        Debug.Log("Хотбар заполнен!");
        return false;
    }

    public HotbarSlot GetSelectedSlot()
    {
        return hotbarSlots[_currentSlotIndex];
    }

    public ItemData GetSelectedItem()
    {
        return hotbarSlots[_currentSlotIndex].GetItem();
    }
}
