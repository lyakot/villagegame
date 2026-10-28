using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToolController : MonoBehaviour
{
    [SerializeField] private HotbarController hotbar;
    [SerializeField] private float toolRange = 1f;
    [SerializeField] private LayerMask toolLayer;

    [SerializeField] private PixelMovement playerMovement;

    [SerializeField] private Transform hitPointUp;
    [SerializeField] private Transform hitPointDown;
    [SerializeField] private Transform hitPointLeft;
    [SerializeField] private Transform hitPointRight;

    [SerializeField] private Animator animator;

    private bool ignoreNextClick;
    private void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        if (ignoreNextClick)
        {
            ignoreNextClick = false;
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            UseTool();
        }
    }

    private void UseTool()
    {
        HotbarSlot slot = hotbar.GetSelectedSlot();

        ItemData item = slot.GetItem();

        if (item == null)
        {
            Debug.Log("Слот пуст");
            return;
        }

        Debug.Log("Выбранный предмет: " + item.itemName);

        if (item.itemType == ItemType.Tool)
        {
            UseToolItem(item);
        }
    }

    private void UseToolItem(ItemData tool)
    {
        switch (tool.itemId)
        {
            case "axe":
                UseAxe(tool);
                break;
        }
    }

    private void UseAxe(ItemData tool)
    {

        Debug.Log("🪓 Используем топор!");

        if (!HasTreeInRange())
        {
            Debug.Log("🌳 Рядом нет дерева");
            return;
        }

        PixelMovement.LookDirection direction = playerMovement.GetLookDirection();

        switch (direction)
        {
            case PixelMovement.LookDirection.Down:
                animator.SetInteger("ToolDirection", 0);
                break;

            case PixelMovement.LookDirection.Up:
                animator.SetInteger("ToolDirection", 1);
                break;

            case PixelMovement.LookDirection.Left:
                animator.SetInteger("ToolDirection", 2);
                break;

            case PixelMovement.LookDirection.Right:
                animator.SetInteger("ToolDirection", 3);
                break;
        }

        animator.SetTrigger("UseTool");
    }

    public void IgnoreNextClick()
    {
        ignoreNextClick = true;
    }
    private bool HasTreeInRange()
    {
        Transform hitPoint = GetCurrentHitPoint();

        Collider2D hit = Physics2D.OverlapCircle(
            hitPoint.position,
            toolRange,
            toolLayer);

        if (hit == null)
            return false;

        return hit.GetComponent<TreeHealth>() != null;
    }
    private void OnDrawGizmosSelected()
    {
        if (hitPointDown == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitPointDown.position, toolRange);
    }

    private void DealToolDamage()
    {
        Transform hitPoint = GetCurrentHitPoint();

        Collider2D hit = Physics2D.OverlapCircle(
            hitPoint.position,
            toolRange,
            toolLayer);

        if (hit == null)
        {
            Debug.Log("Рядом ничего нет");
            return;
        }

        TreeHealth tree = hit.GetComponent<TreeHealth>();

        if (tree != null)
        {
            tree.PlayHitAnimation();
            tree.TakeDamage(1);
        }
    }
    private Transform GetCurrentHitPoint()
    {
        switch (playerMovement.GetLookDirection())
        {
            case PixelMovement.LookDirection.Up:
                return hitPointUp;

            case PixelMovement.LookDirection.Down:
                return hitPointDown;

            case PixelMovement.LookDirection.Left:
                return hitPointLeft;

            case PixelMovement.LookDirection.Right:
                return hitPointRight;

            default:
                return hitPointDown;
        }
    }
}