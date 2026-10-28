using UnityEngine;

public class BackpackUI : MonoBehaviour
{
    [SerializeField] private InventorySlot[] slots;


    private void OnEnable()
    {
        Debug.Log("BACKPACK OnEnable");

        if (InventoryManager.Instance != null)
        {
            Debug.Log("BACKPACK подписывается на OnInventoryChanged");

            InventoryManager.Instance.OnInventoryChanged += Refresh;
            Refresh();
        }
        else
        {
            Debug.Log("BACKPACK: InventoryManager.Instance == null");
        }
    }


    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }


    public void Refresh()
    {
        Debug.Log("BACKPACK REFRESH");

        for (int i = 0; i < slots.Length; i++)
        {
            ItemData item = InventoryManager.Instance.GetItem(i);
            int amount = InventoryManager.Instance.GetAmount(i);

            slots[i].SetItem(item, amount);
        }
    }
}