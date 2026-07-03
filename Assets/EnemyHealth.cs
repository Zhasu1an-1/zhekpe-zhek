using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 2000;
    private int currentHealth;

    public Slider healthBar;

    private Renderer enemyRenderer;
    private Color originalColor;

    public float HealthPercent
    {
        get { return (float)currentHealth / maxHealth; }
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (healthBar != null)
            healthBar.value = currentHealth;

        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator HitFlash()
    {
        enemyRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material.color = originalColor;
    }

    void Die()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        if (gameManager != null)
            gameManager.ShowWin();

        Destroy(gameObject);
    }
}