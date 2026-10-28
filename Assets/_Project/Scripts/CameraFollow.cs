using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // персонаж
    public float smoothSpeed = 5f;  // скорость сглаживания
    public Vector3 offset;          // смещение камеры

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        // Плавное догоняющее движение
        Vector3 smoothed = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothed;
    }
}
