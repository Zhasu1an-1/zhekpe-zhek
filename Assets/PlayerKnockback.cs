using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackTime = 0.15f;

    private CharacterController controller;
    private Vector3 direction;
    private float speed;
    private float timer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (timer > 0)
        {
            controller.Move(direction * speed * Time.deltaTime);
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