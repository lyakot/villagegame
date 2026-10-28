using UnityEngine;

public class PlayerBackpack : MonoBehaviour
{

    public bool HasBackpack { get; private set; }
    public GameObject backpackPrefab;
    public GameObject backpackButton;
    
    private Animator animator;

    [SerializeField] private InventoryUI inventoryUI;
    private void Start()
    {
        animator = GetComponent<Animator>();

        if (backpackButton != null)
            backpackButton.SetActive(false);
    }

    public void PickUpBackpack()
    {
        HasBackpack = true;
    }

    public void EquipBackpack()
    {
        HasBackpack = true;

        animator.SetBool("hasBackpack", true);

        if (backpackButton != null)
            backpackButton.SetActive(true);
    }
    public void RemoveBackpack()
    {
        Debug.Log("Попытка снять рюкзак. HasBackpack = " + HasBackpack);

        if (!HasBackpack)
        {
            Debug.Log("Рюкзак не надет");
            return;
        }

        HasBackpack = false;

        animator.SetBool("hasBackpack", false);

        Debug.Log("Рюкзак снят");

        if (backpackButton != null)
        {
            backpackButton.SetActive(false);
            Debug.Log("Кнопка рюкзака скрыта");
        }

        Instantiate(
            backpackPrefab,
            transform.position + Vector3.right,
            Quaternion.identity);

        Debug.Log("Рюкзак создан на земле");
    }

    public void OpenBackpack()
    {
        if (!animator.GetBool("hasBackpack"))
            return;

        Debug.Log("Открыть рюкзак");

        // Позже здесь будет:
        // backpackInventoryUI.ToggleInventory();
    }
}