using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public float lightAttackRange = 2f;
    public float heavyAttackRange = 2.5f;
    public float specialAttackRange = 3.5f;

    public int combo1Damage = 15;
    public int combo2Damage = 20;
    public int combo3Damage = 35;

    public int heavyDamage = 50;
    public int specialDamage = 100;

    public float comboResetTime = 1.2f;

    public float lightKnockbackForce = 4f;
    public float heavyKnockbackForce = 7f;
    public float specialKnockbackForce = 10f;

    public float specialHitDelay = 1.4f;

    public GameObject hitEffectPrefab;

    private int comboStep = 0;
    private float lastComboTime;

    public Slider spiritBar;
    public int maxSpirit = 100;
    private int currentSpirit = 0;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        UpdateSpiritBar();
    }

    void Update()
    {
        if (Time.time - lastComboTime > comboResetTime)
            comboStep = 0;

        if (Input.GetKeyDown(KeyCode.F))
            ComboAttack();

        if (Input.GetKeyDown(KeyCode.R))
            KickAttack();

        if (Input.GetKeyDown(KeyCode.E))
            SpecialAttack();
    }

    void ComboAttack()
    {
        if (animator != null)
            animator.SetTrigger("LightAttack");

        comboStep++;
        lastComboTime = Time.time;

        if (comboStep > 3)
            comboStep = 1;

        int damage = combo1Damage;
        float knockbackForce = lightKnockbackForce;

        if (comboStep == 1)
        {
            damage = combo1Damage;
            knockbackForce = 3f;
            Debug.Log("Combo 1");
        }
        else if (comboStep == 2)
        {
            damage = combo2Damage;
            knockbackForce = 4f;
            Debug.Log("Combo 2");
        }
        else if (comboStep == 3)
        {
            damage = combo3Damage;
            knockbackForce = 6f;
            Debug.Log("Combo 3");
        }

        bool hit = Attack(lightAttackRange, damage, knockbackForce);

        if (hit)
            AddSpirit(20);
    }

    void KickAttack()
    {
        if (animator != null)
            animator.SetTrigger("Kick");

        bool hit = Attack(heavyAttackRange, heavyDamage, heavyKnockbackForce);

        if (hit)
            AddSpirit(30);

        Debug.Log("Kick Attack");
    }

    void SpecialAttack()
    {
        if (currentSpirit < maxSpirit)
        {
            Debug.Log("Spirit is not full");
            return;
        }

        if (animator != null)
            animator.SetTrigger("PowerUp");

        Invoke(nameof(DoSpecialDamage), specialHitDelay);

        currentSpirit = 0;
        UpdateSpiritBar();

        Debug.Log("Power Up Started");
    }

    void DoSpecialDamage()
    {
        Attack(specialAttackRange, specialDamage, specialKnockbackForce);
        Debug.Log("Special Hit Damage");
    }

    bool Attack(float range, int damage, float knockbackForce)
    {
        bool hitEnemy = false;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth health = enemy.GetComponentInParent<EnemyHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);

                EnemyKnockback knockback = enemy.GetComponentInParent<EnemyKnockback>();

                if (knockback != null)
                    knockback.ApplyKnockback(transform.position, knockbackForce);

                SpawnHitEffect(enemy.transform.position);

                hitEnemy = true;
            }
        }

        return hitEnemy;
    }

    void SpawnHitEffect(Vector3 position)
    {
        if (hitEffectPrefab != null)
        {
            Vector3 spawnPosition = position + Vector3.up * 1f;
            GameObject effect = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
            Destroy(effect, 1f);
        }
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
}