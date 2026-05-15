using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 150;
    public int currentHealth = 150;

    public bool IsDead
    {
        get { return currentHealth <= 0; }
    }

    void Start()
    {
        if (currentHealth <= 0)
            currentHealth = maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        GameManager.Instance?.UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        GameManager.Instance?.UpdateUI();

        if (currentHealth <= 0)
            GameManager.Instance?.GameOver();
    }
}
