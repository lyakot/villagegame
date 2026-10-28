using UnityEngine;

public enum ItemType
{
    Resource,
    Tool,
    Consumable,
    Fish
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemId;
    public string itemName;
    public string description;
    public Sprite icon;

    public ItemType itemType;

    [Header("Tool Settings")]
    public int damage = 1;
}