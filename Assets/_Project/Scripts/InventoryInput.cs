using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (inventoryUI.gameObject.activeSelf)
                inventoryUI.CloseInventory();
            else
                inventoryUI.OpenInventory();
        }
    }
}