using UnityEngine;

public class BackpackItem : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Радиус, в пределах которого игрок может подобрать рюкзак")]
    public float pickupDistance = 3f;

    private Transform playerTransform;
    private Animator playerAnimator;

    public Sprite backpackIcon;
    private HotbarController hotbar;

    void Start()
    {
        // Автоматически находим игрока по тегу "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        

        if (player != null)
        {
            playerTransform = player.transform;
            playerAnimator = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Внимание: На сцене не найден объект с тегом 'Player'!");
        }
    }

    // Этот метод Unity вызывает автоматически при клике мышкой по коллайдеру объекта
    private void OnMouseDown()
    {
        if (playerTransform == null)
            return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > pickupDistance)
        {
            Debug.Log("Рюкзак слишком далеко!");
            return;
        }

        PlayerBackpack backpack = playerTransform.GetComponent<PlayerBackpack>();

        if (backpack != null)
        {
            backpack.EquipBackpack();
            Destroy(gameObject);
        }
    }
}
