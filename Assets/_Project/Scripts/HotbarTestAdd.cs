using UnityEngine;

public class HotbarTestAdd : MonoBehaviour
{
    [SerializeField] private ItemData item;
    [SerializeField] private HotbarController hotbar;


    private void Start()
    {
        hotbar.AddItem(item);

        Debug.Log("Топор добавлен в хотбар");
    }
}