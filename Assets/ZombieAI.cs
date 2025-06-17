using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public int damagePerSecond = 10;
    public float attackRate = 1f;

    private Transform baseTransform;
    private float nextAttackTime;
    private bool isAttackingBase = false;

    void Start()
    {
        baseTransform = GameObject.FindGameObjectWithTag("Base ").transform;
    }

    void Update()
    {
        if (!isAttackingBase)
        {
            MoveToBase();
        }
    }

    void MoveToBase()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            baseTransform.position,
            moveSpeed * Time.deltaTime
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base "))
        {
            isAttackingBase = true;
            InvokeRepeating("AttackBase", 0f, attackRate);
        }
    }

    void AttackBase()
    {
        BaseHealth baseHealth = baseTransform.GetComponent<BaseHealth>();
        if (baseHealth != null)
        {
            baseHealth.TakeDamage(damagePerSecond);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            isAttackingBase = false;
            CancelInvoke("AttackBase");
        }
    }
}