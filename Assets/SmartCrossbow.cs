using UnityEngine;

public class SmartCrossbow : MonoBehaviour
{
    [Header("Настройки")]
    public float rotationSpeed = 200f;
    public float attackRange = 3f;
    public float attackRate = 1f;
    public int damage = 5;
    public GameObject arrowPrefab;
    public Transform firePoint;

    private Transform target;
    private float nextAttackTime;
    private bool hasTargetInRange = false;

    void Update()
    {
        FindNearestZombie();

        if (hasTargetInRange)
        {
            RotateTowardsTarget();

            if (Time.time >= nextAttackTime)
            {
                Shoot();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void FindNearestZombie()
    {
        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        target = null;
        float closestDistance = Mathf.Infinity;
        hasTargetInRange = false;

        foreach (Collider2D zombie in zombies)
        {
            if (!zombie.CompareTag("Zombie")) continue;

            float distance = Vector2.Distance(transform.position, zombie.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = zombie.transform;
                hasTargetInRange = true;
            }
        }
    }

    void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void Shoot()
    {
        if (arrowPrefab == null || firePoint == null) return;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        if (arrowComponent != null)
        {
            arrowComponent.SetDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}





















