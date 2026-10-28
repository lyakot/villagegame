using UnityEngine;

public class InventorySlotManager : MonoBehaviour
{
    private InventorySlot currentSlot;

    public void SelectSlot(InventorySlot slot)
    {
        if (currentSlot != null)
        {
            currentSlot.Deselect();
        }

        currentSlot = slot;
        currentSlot.Select();
    }
}