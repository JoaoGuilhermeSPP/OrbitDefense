using UnityEngine;


public class BulletBossTree : MonoBehaviour
{
    [Header("Target")]
    public string targetTag = "Mundo";

    private Transform target;

    [Header("Movimento")]
    public float velocidade = 8f;
    public float velocidadeRotacao = 200f;

    [Header("Desvio")]
    public float offsetRotacao;

    [Header("Efeito")]
    public GameObject explosioneffec;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ProcurarTarget();
        Destroy(gameObject, 10f);
    }

    private void ProcurarTarget()
    {
        GameObject alvo =
            GameObject.FindGameObjectWithTag(targetTag);

        if (alvo != null)
        {
            target = alvo.transform;
        }
        else
        {
            Debug.LogWarning(
                "Target não encontrado com a tag: " +
                targetTag
            );
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            ProcurarTarget();

            if (target == null)
                return;
        }

        Vector2 direcao =
            ((Vector2)target.position - rb.position).normalized;

        // Rotação aleatória opcional
        float angulo = offsetRotacao * Mathf.Deg2Rad;

        Vector2 direcaoFinal = new Vector2(
            direcao.x * Mathf.Cos(angulo) -
            direcao.y * Mathf.Sin(angulo),

            direcao.x * Mathf.Sin(angulo) +
            direcao.y * Mathf.Cos(angulo)
        );

        float rotacaoAmount =
            Vector3.Cross(
                direcaoFinal,
                transform.right
            ).z;

        rb.angularVelocity =
            -rotacaoAmount * velocidadeRotacao;

        rb.linearVelocity =
            transform.right * velocidade;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignora layers
        if (collision.gameObject.layer == 8 ||
            collision.gameObject.layer == 10
        )
        {
            return;
        }

        // Dano
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject
                .GetComponent<WordLife>()
                ?.TakeDamage(2);
            Explodir();
        }
        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<LifePlayer>()?.TakeDamage(1);
            Explodir();
        }

    }

    private void Explodir()
    {
        if (explosioneffec != null)
        {
            GameObject effect = Instantiate(
                explosioneffec,
                transform.position,
                Quaternion.identity
            );
            SFXManager.current.PlayMusic(SFXManager.current.ExplosionB);
            Destroy(effect, 1.5f);
        }

        Destroy(gameObject);
    }
}