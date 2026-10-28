using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{

    [Header("Slot")]
    [SerializeField] private Image slotImage;

    [Header("Item Icon")]
    [SerializeField] private Image itemIcon;

    [Header("Item Amount")]
    [SerializeField] private TMP_Text amountText;

    [Header("Selection")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite selectedSprite;


    public bool Selected { get; private set; }


    private ItemData item;
    private int amount;


    public void SetItem(ItemData newItem, int newAmount = 1)
    {
        item = newItem;
        amount = newAmount;

        if (item != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;

            if (amountText != null)
            {
                amountText.text = amount > 1 ? amount.ToString() : "";
            }
        }
        else
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            amount = 0;

            if (amountText != null)
            {
                amountText.text = "";
            }
        }
    }

    public Action<ItemData> OnItemClicked;

    public void Click()
    {
        if (item != null)
        {
            Debug.Log($"🐟 Клик по рыбе: {item.itemName}");
            OnItemClicked?.Invoke(item);
        }
    }

    public ItemData GetItem()
    {
        return item;
    }

    public int GetAmount()
    {
        return amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void RemoveAmount(int value)
    {
        amount -= value;

        if (amount <= 0)
        {
            SetItem(null);
        }
    }

    public void Select()
    {
        Selected = true;
        slotImage.sprite = selectedSprite;
    }


    public void Deselect()
    {
        Selected = false;
        slotImage.sprite = normalSprite;
    }
}