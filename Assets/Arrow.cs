using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage;
    private float speed = 12f;

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    void Update()
    {
        // Движение стрелы вперед
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie"))
        {
            other.GetComponent<ZombieHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
