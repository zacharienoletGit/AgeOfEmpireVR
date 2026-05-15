using UnityEngine;

public class SelectableUnit : MonoBehaviour
{
    public float moveSpeed = 0.45f;
    public float stopDistance = 0.05f;

    Vector3 moveTarget;
    bool hasMoveTarget;
    UnitCombat attackTarget;
    UnitCombat combat;
    GameObject selectedRing;

    void Awake()
    {
        combat = GetComponent<UnitCombat>();
        CreateSelectedRing();
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            return;

        if (combat == null || !combat.IsAlive)
            return;

        if (attackTarget != null && attackTarget.IsAlive)
        {
            float distance = Vector3.Distance(transform.position, attackTarget.transform.position);
            if (distance > combat.attackRange * 0.9f)
            {
                MoveStep(attackTarget.transform.position);
            }
            else
            {
                combat.Attack(attackTarget);
            }

            return;
        }

        if (hasMoveTarget)
        {
            MoveStep(moveTarget);

            if (Vector3.Distance(transform.position, moveTarget) <= stopDistance)
                hasMoveTarget = false;
        }
    }

    public void SetSelected(bool selected)
    {
        if (selectedRing != null)
            selectedRing.SetActive(selected);
    }

    public void MoveTo(Vector3 point)
    {
        attackTarget = null;
        moveTarget = new Vector3(point.x, transform.position.y, point.z);
        hasMoveTarget = true;
    }

    public void AttackTarget(UnitCombat target)
    {
        if (target == null || target.team == UnitCombat.Team.Player)
            return;

        attackTarget = target;
        hasMoveTarget = false;
    }

    void MoveStep(Vector3 point)
    {
        Vector3 flatTarget = new Vector3(point.x, transform.position.y, point.z);
        transform.position = Vector3.MoveTowards(transform.position, flatTarget, moveSpeed * Time.deltaTime);
        Vector3 direction = flatTarget - transform.position;

        if (direction.sqrMagnitude > 0.0001f)
            transform.forward = direction.normalized;
    }

    void CreateSelectedRing()
    {
        selectedRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        selectedRing.name = "SelectedRing";
        selectedRing.transform.SetParent(transform);
        selectedRing.transform.localPosition = new Vector3(0f, -0.48f, 0f);
        selectedRing.transform.localScale = new Vector3(1.35f, 0.03f, 1.35f);

        Collider ringCollider = selectedRing.GetComponent<Collider>();
        if (ringCollider != null)
            ringCollider.enabled = false;

        StudentSceneBootstrap.ApplyMaterial(selectedRing, Color.yellow);
        selectedRing.SetActive(false);
    }
}
