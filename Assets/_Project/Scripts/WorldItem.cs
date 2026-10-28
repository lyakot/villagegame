using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData item;
    [SerializeField] private HotbarController hotbar;
    [SerializeField] private PlayerToolController playerToolController;
    public void Pickup()
    {
        bool added = InventoryManager.Instance.AddItem(item);

        if (added)
        {
            if (item.itemType == ItemType.Tool)
            {
                hotbar.AddItem(item);
                playerToolController.IgnoreNextClick();
            }

            if (item.itemId == "blueberry")
            {
                DayProgressManager.Instance.AddBerry();
            }

            if (item.itemId == "apple")
            {
                DayProgressManager.Instance.AddApple();
            }
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}