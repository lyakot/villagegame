using UnityEngine;
using UnityEngine.EventSystems;

public class BackpackButton : MonoBehaviour, IPointerClickHandler
{
    public PlayerBackpack playerBackpack;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Клик по рюкзаку: " + eventData.button);
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            playerBackpack.OpenBackpack();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            playerBackpack.RemoveBackpack();
        }
    }
}
