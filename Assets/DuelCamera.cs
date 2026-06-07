using UnityEngine;

public class DuelCamera : MonoBehaviour
{
    public Transform player;
    public Transform enemy;

    public float distance = 10f;
    public float height = 6f;
    public float sideOffset = 0f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (player == null || enemy == null) return;

        Vector3 middlePoint = (player.position + enemy.position) / 2f;

        Vector3 fightDirection = enemy.position - player.position;
        fightDirection.y = 0;

        if (fightDirection == Vector3.zero) return;

        Vector3 cameraDirection = -fightDirection.normalized;

        Vector3 desiredPosition =
            middlePoint +
            cameraDirection * distance +
            Vector3.up * height +
            Vector3.right * sideOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(middlePoint + Vector3.up * 1f);
    }
}