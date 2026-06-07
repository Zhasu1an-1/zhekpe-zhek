using UnityEngine;

public class PlayerLookAtEnemy : MonoBehaviour
{
    public Transform enemy;
    public float rotationSpeed = 10f;

    void Update()
    {
        if (enemy == null) return;

        Vector3 direction = enemy.position - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}