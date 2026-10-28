using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotbarSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;

    private ItemData item;

    [SerializeField] private HotbarController hotbarController;
    [SerializeField] private int slotIndex;

    public bool IsEmpty()
    {
        return item == null;
    }

    public void SetItem(ItemData newItem)
    {
        Debug.Log("Рисуем предмет в хотбаре: " + newItem.itemName);

        item = newItem;

        if (item != null)
        {
            Debug.Log("Иконка: " + item.icon);

            icon.sprite = item.icon;
            icon.enabled = true;
        }
    }


    public ItemData GetItem()
    {
        return item;
    }


    public void Clear()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        hotbarController.SelectSlot(slotIndex);
    }
}