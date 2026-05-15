using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    public Team team = Team.Player;
    public int maxHealth = 40;
    public int currentHealth = 40;
    public int damage = 8;
    public float attackRange = 0.35f;
    public float attackCooldown = 0.75f;

    float attackTimer;

    public bool IsAlive
    {
        get { return currentHealth > 0; }
    }

    void Start()
    {
        if (currentHealth <= 0)
            currentHealth = maxHealth;
    }

    void Update()
    {
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public bool CanAttack(UnitCombat target)
    {
        if (target == null || !target.IsAlive || !IsAlive)
            return false;

        if (target.team == team)
            return false;

        return Vector3.Distance(transform.position, target.transform.position) <= attackRange;
    }

    public void Attack(UnitCombat target)
    {
        if (attackTimer > 0f || !CanAttack(target))
            return;

        target.TakeDamage(damage);
        attackTimer = attackCooldown;
        AudioManager.Instance?.PlayHit();
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive)
            return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (team == Team.Enemy)
        {
            GameManager.Instance?.EnemyKilled(this);
            AudioManager.Instance?.PlayDeath();
        }

        Destroy(gameObject);
    }
}
