using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float knockbackTime = 0.15f;

    private Vector3 direction;
    private float speed;
    private float timer;

    void Update()
    {
        if (timer > 0)
        {
            transform.position += direction * speed * Time.deltaTime;
            timer -= Time.deltaTime;
        }
    }

    public void ApplyKnockback(Vector3 attackerPosition, float force)
    {
        direction = transform.position - attackerPosition;
        direction.y = 0;
        direction.Normalize();

        speed = force;
        timer = knockbackTime;
    }
}