using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [Header("Movement / Attack")]
    public float moveSpeed = 2f;
    public int damagePerHit = 5;
    public float attackRate = 1f;

    private Transform baseTransform;
    private BaseHealth baseHealth;
    private bool isAttacking = false;

    void Start()
    {
        // возможно база ещё не создана — будем искать в Update, но пробуем найти сейчас
        FindBase();
    }

    void Update()
    {
        // пока раунд не начат — стоим на месте
        if (GameManager.instance == null || !GameManager.instance.roundStarted) return;

        // найди базу, если она не найдена
        if (baseTransform == null)
        {
            FindBase();
            if (baseTransform == null) return;
        }

        // если не атакуем — идём к базе
        if (!isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     baseTransform.position,
                                                     moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Base ")) return;

        baseHealth = collision.gameObject.GetComponent<BaseHealth>();
        baseTransform = collision.transform;

        // начинаем атаку только если раунд уже начат
        if (GameManager.instance != null && GameManager.instance.roundStarted)
            StartAttack();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Base ")) return;

        // если зомби оказался у базы раньше, а игрок потом нажал Start,
        // OnCollisionStay сработает и запустит атаку
        if (!isAttacking && GameManager.instance != null && GameManager.instance.roundStarted)
            StartAttack();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Base ")) return;
        isAttacking = false;
        CancelInvoke(nameof(AttackBase));
    }

    void StartAttack()
    {
        if (isAttacking || baseHealth == null) return;
        isAttacking = true;
        InvokeRepeating(nameof(AttackBase), 0f, attackRate);
        Debug.Log(name + ": started attacking base");
    }

    void AttackBase()
    {
        if (baseHealth == null)
        {
            CancelInvoke(nameof(AttackBase));
            isAttacking = false;
            return;
        }

        baseHealth.TakeDamage(damagePerHit);
        Debug.Log(name + $": attacked base for {damagePerHit}");
    }

    void FindBase()
    {
        GameObject b = GameObject.FindGameObjectWithTag("Base ");
        if (b != null)
        {
            baseTransform = b.transform;
            baseHealth = b.GetComponent<BaseHealth>();
        }
    }
}
