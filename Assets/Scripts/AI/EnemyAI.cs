using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public BaseHealth targetBase;
    public float moveSpeed = 0.25f;
    public float stopDistance = 0.22f;
    public float attackDelay = 1.1f;

    UnitCombat combat;
    float attackTimer;
    bool registered;

    void Start()
    {
        combat = GetComponent<UnitCombat>();

        if (targetBase == null)
            targetBase = FindObjectOfType<BaseHealth>();

        if (!registered)
        {
            GameManager.Instance?.RegisterEnemy(this);
            registered = true;
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            return;

        if (combat == null || !combat.IsAlive || targetBase == null || targetBase.IsDead)
            return;

        attackTimer -= Time.deltaTime;

        Vector3 targetPosition = targetBase.transform.position;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > stopDistance)
        {
            Vector3 flatTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, flatTarget, moveSpeed * Time.deltaTime);
            Vector3 direction = flatTarget - transform.position;

            if (direction.sqrMagnitude > 0.0001f)
                transform.forward = direction.normalized;
        }
        else if (attackTimer <= 0f)
        {
            targetBase.TakeDamage(combat.damage);
            attackTimer = attackDelay;
        }
    }
}
