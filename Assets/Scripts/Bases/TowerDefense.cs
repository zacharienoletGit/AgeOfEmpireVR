using UnityEngine;

public class TowerDefense : MonoBehaviour
{
    public float range = 0.9f;
    public int damage = 10;
    public float fireDelay = 0.7f;

    float fireTimer;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            return;

        fireTimer -= Time.deltaTime;

        UnitCombat target = FindClosestEnemy();
        if (target == null || fireTimer > 0f)
            return;

        target.TakeDamage(damage);
        fireTimer = fireDelay;
    }

    UnitCombat FindClosestEnemy()
    {
        UnitCombat[] combats = FindObjectsOfType<UnitCombat>();
        UnitCombat closest = null;
        float closestDistance = range;

        for (int i = 0; i < combats.Length; i++)
        {
            if (combats[i] == null || !combats[i].IsAlive || combats[i].team != UnitCombat.Team.Enemy)
                continue;

            float distance = Vector3.Distance(transform.position, combats[i].transform.position);
            if (distance <= closestDistance)
            {
                closest = combats[i];
                closestDistance = distance;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
