using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float retreatSpeed = 1.5f;
    public float strafeSpeed = 1.2f;

    public float minDistance = 1.2f;
    public float stopDistance = 1.8f;
    public float attackDistance = 2.6f;

    [Header("Damage")]
    public int lightDamage = 8;
    public int heavyDamage = 18;
    public int specialDamage = 40;

    public float lightKnockbackForce = 3f;
    public float heavyKnockbackForce = 5f;
    public float specialKnockbackForce = 8f;

    [Header("Attack Settings")]
    public float attackCooldown = 1.1f;
    public float heavyChance = 0.35f;

    [Header("Retreat")]
    public float retreatAfterAttackTime = 0.25f;
    private bool isRetreating = false;
    private float retreatTimer = 0f;

    [Header("Spirit")]
    public Slider spiritBar;
    public int maxSpirit = 100;
    private int currentSpirit = 0;

    private float lastAttackTime;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        UpdateSpiritBar();
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();

        float distance = Vector3.Distance(transform.position, player.position);
        bool isMoving = false;

        if (isRetreating)
        {
            RetreatFromPlayer();
            isMoving = true;

            retreatTimer -= Time.deltaTime;

            if (retreatTimer <= 0)
                isRetreating = false;

            SetRunAnimation(isMoving);
            return;
        }

        if (distance > attackDistance)
        {
            MoveTowardPlayer();
            isMoving = true;
        }
        else
        {
            TryAttack();

            if (distance < minDistance)
            {
                RetreatFromPlayer();
                isMoving = true;
            }
            else
            {
                StrafeAroundPlayer();
                isMoving = true;
            }
        }

        SetRunAnimation(isMoving);
    }

    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction == Vector3.zero) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * 10f
        );
    }

    void MoveTowardPlayer()
    {
        Vector3 targetPosition = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    void RetreatFromPlayer()
    {
        Vector3 directionAway = transform.position - player.position;
        directionAway.y = 0;

        if (directionAway == Vector3.zero) return;

        transform.position += directionAway.normalized * retreatSpeed * Time.deltaTime;
    }

    void StrafeAroundPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer == Vector3.zero) return;

        Vector3 sideDirection = Vector3.Cross(Vector3.up, directionToPlayer.normalized);
        transform.position += sideDirection * strafeSpeed * Time.deltaTime;
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        if (currentSpirit >= maxSpirit)
        {
            SpecialAttack();
        }
        else
        {
            if (Random.value < heavyChance)
                HeavyAttack();
            else
                LightAttack();
        }

        lastAttackTime = Time.time;

        isRetreating = true;
        retreatTimer = retreatAfterAttackTime;
    }

    void LightAttack()
    {
        if (animator != null)
            animator.SetTrigger("hit");

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(lightDamage);
            ApplyPlayerKnockback(lightKnockbackForce);
            AddSpirit(20);
            Debug.Log("Enemy Light Attack");
        }
    }

    void HeavyAttack()
    {
        if (animator != null)
            animator.SetTrigger("Kick");

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(heavyDamage);
            ApplyPlayerKnockback(heavyKnockbackForce);
            AddSpirit(35);
            Debug.Log("Enemy Heavy Kick");
        }
    }

    void SpecialAttack()
    {
        if (animator != null)
            animator.SetTrigger("hit");

        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(specialDamage);
            ApplyPlayerKnockback(specialKnockbackForce);

            currentSpirit = 0;
            UpdateSpiritBar();

            Debug.Log("Enemy Special Attack");
        }
    }

    void ApplyPlayerKnockback(float force)
    {
        PlayerKnockback knockback = player.GetComponent<PlayerKnockback>();

        if (knockback != null)
            knockback.ApplyKnockback(transform.position, force);
    }

    void AddSpirit(int amount)
    {
        currentSpirit += amount;

        if (currentSpirit > maxSpirit)
            currentSpirit = maxSpirit;

        UpdateSpiritBar();
    }

    void UpdateSpiritBar()
    {
        if (spiritBar != null)
        {
            spiritBar.maxValue = maxSpirit;
            spiritBar.value = currentSpirit;
        }
    }

    void SetRunAnimation(bool value)
    {
        if (animator != null)
            animator.SetBool("isrunning", value);
    }
}