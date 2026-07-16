using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private WordLife worldTarget;

    public float initialFallSpeed = 8f;
    public float bounceSpeed = 10f;
    public float lifeTime = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // procura automaticamente o alvo na cena
        worldTarget = FindFirstObjectByType<WordLife>();

        rb.linearVelocity = Vector2.down * initialFallSpeed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<WordLife>()?.TakeDamage(8);
            return;
        }

        if ( collision.gameObject.layer == 8)
        {
            return;
        }
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<LifePlayer>().TakeDamage(1);
        }
        if (collision.gameObject.layer == 10)
        RicochetToTarget();
    }

    void RicochetToTarget()
    {
        if (worldTarget == null) return;

        Vector2 direction =
            (worldTarget.transform.position + transform.position).normalized;

        // sempre descendo
        direction.x = Mathf.Abs(direction.x);

        rb.linearVelocity = direction * bounceSpeed;
    }
}