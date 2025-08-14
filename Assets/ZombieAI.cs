using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int damagePerHit = 10;
    public float attackRate = 1f;

    private BaseHealth targetBase;
    private bool isAttacking = false;

    void Update()
    {
        if (!isAttacking)
        {
            if (targetBase == null)
            {
                GameObject b = GameObject.FindGameObjectWithTag("Base ");
                if (b != null) targetBase = b.GetComponent<BaseHealth>();
            }
            if (targetBase != null)
                transform.position = Vector2.MoveTowards(transform.position, targetBase.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base "))
        {
            Debug.Log("ZombieAI: collided with Base");
            targetBase = collision.gameObject.GetComponent<BaseHealth>();
            if (targetBase != null)
            {
                isAttacking = true;
                InvokeRepeating(nameof(AttackBase), 0f, attackRate);
            }
        }
    }

    void AttackBase()
    {
        if (targetBase == null)
        {
            CancelInvoke(nameof(AttackBase));
            isAttacking = false;
            return;
        }
        Debug.Log("ZombieAI: AttackBase()");
        targetBase.TakeDamage(damagePerHit);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            isAttacking = false;
            CancelInvoke(nameof(AttackBase));
        }
    }
}