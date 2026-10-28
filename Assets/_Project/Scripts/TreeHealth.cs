using UnityEngine;

public class TreeHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [Header("Drops")]
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject applePrefab;

    [SerializeField] private int minAppleDrop = 0;
    [SerializeField] private int maxAppleDrop = 3;

    [SerializeField] private Animator animator;
    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log($"🌳 Дерево получило {damage} урона. Осталось HP: {health}");

        if (health <= 0)
        {
            DestroyTree();
        }
    }

    private void DestroyTree()
    {
        Debug.Log("🌲 Дерево срублено!");

        Instantiate(woodPrefab, transform.position, Quaternion.identity);

        int appleCount = Random.Range(minAppleDrop, maxAppleDrop + 1);

        for (int i = 0; i < appleCount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.2f, 0.5f),
                0
            );

            Instantiate(
                applePrefab,
                transform.position + offset,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
    public void PlayHitAnimation()
    {
        animator.SetTrigger("Hit");
    }
}