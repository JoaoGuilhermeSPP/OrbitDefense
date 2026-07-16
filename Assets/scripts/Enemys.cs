using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Enemys : MonoBehaviour
{
    [SerializeField] Transform Target;
    NavMeshAgent Agent;
    public EnemyLife stats;
    Vector3 targetScale = Vector3.zero;
    bool scales = false;
    public GameObject explosion;

    private bool isDead = false;
    private bool hasCollided = false;

    void Start()
    {
        stats = GetComponent<EnemyLife>();
        Agent = GetComponent<NavMeshAgent>();

        // Configuração do NavMeshAgent
        if (Agent != null)
        {
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
            Agent.autoBraking = true;
        }

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");
        if (mundo != null)
        {
            Target = mundo.transform;
        }
    }

    void Update()
    {
        if (isDead || Target == null || Agent == null) return;

        // Só seta destino se estiver no NavMesh
        if (Agent.isOnNavMesh && Agent.isActiveAndEnabled)
        {
            Agent.SetDestination(Target.position);
        }

        if (Target != null)
        {
            Vector3 direction = (Target.position - transform.position);
            float angles = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angles - 90f);
        }

        if (scales)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Mundo") && !hasCollided)
        {
            hasCollided = true;
            scales = true;
            Destroy(gameObject, 5f);
        }

        if (collision.CompareTag("Shield"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // Limita DOTween para evitar acúmulo
        if (collision.gameObject.layer == 6) // Mundo
        {
            WordLife worldLife = collision.gameObject.GetComponent<WordLife>();
            if (worldLife != null)
            {
                // Só aplica DOTween uma vez por colisão
                collision.transform.DOScale(7f, 0.5f)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => {
                        if (collision.transform != null)
                            collision.transform.localScale = Vector3.one * 7f;
                    });

                worldLife.TakeDamage(10);
            }
        }

        if (collision.gameObject.layer == 7) // Player
        {
            LifePlayer playerLife = collision.gameObject.GetComponent<LifePlayer>();
            if (playerLife != null)
                playerLife.TakeDamage(1);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (explosion != null)
        {
            GameObject explosionInstace = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstace.transform.SetParent(null);
            Destroy(explosionInstace, 2f);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Limpa referências
        Agent = null;
        Target = null;
        stats = null;
    }
}