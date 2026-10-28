using UnityEngine;
using UnityEngine.InputSystem; // Подключаем новую систему ввода

[RequireComponent(typeof(Rigidbody2D))]
public class PixelMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public bool isLocked = false;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        // ЕСЛИ ПЕРСОНАЖ ЗАБЛОКИРОВАН (например, рыбачит)
        if (isLocked)
        {
            movement = Vector2.zero; // Полностью обнуляем движение

            // Сбрасываем параметры скорости в аниматоре, чтобы он встал в Idle
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
            return; // Выходим из метода, клавиатуру дальше не слушаем
        }

        // Проверяем, подключена ли клавиатура
        if (Keyboard.current != null)
        {
            float moveX = 0f;
            float moveY = 0f;

            // Опрашиваем WASD или стрелочки напрямую из кода
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1f;

            // Запрещаем движение по диагонали (Приоритет горизонтали)
            if (moveX != 0f && moveY != 0f)
            {
                moveY = 0f;
            }

            movement = new Vector2(moveX, moveY);
        }

        // Отправляем чистые значения (-1, 0, 1) в ваш Аниматор
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("LastMoveX", movement.x);
            animator.SetFloat("LastMoveY", movement.y);
        }
    }

    void FixedUpdate()
    {
        // Физическое перемещение на основе переданных в Аниматор данных
        rb.linearVelocity = movement * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Столкнулся с: " + collision.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Зашёл в триггер: " + other.gameObject.name);
    }

    public enum LookDirection { Down, Up, Left, Right }

    public LookDirection GetLookDirection()
    {
        float x = animator.GetFloat("LastMoveX");
        float y = animator.GetFloat("LastMoveY");

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            return x > 0 ? LookDirection.Right : LookDirection.Left;
        }
        else
        {
            return y > 0 ? LookDirection.Up : LookDirection.Down;
        }
    }
}
