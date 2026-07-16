using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTre : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    public EnemyLife stats;
    public GameObject explosion;

    private bool atacando;
    private bool scales;
    private bool startedAttack;
    private bool isDead;
    private Vector3 targetScale = Vector3.one;
    private Coroutine fireCoroutine;

    void Start()
    {
        stats = GetComponent<EnemyLife>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.autoBraking = true;
        }

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");
        if (mundo != null)
        {
            target = mundo.transform;
        }
    }

    void Update()
    {
        if (isDead || target == null || agent == null) return;

        if (agent.isOnNavMesh && agent.isActiveAndEnabled && !agent.isStopped)
        {
            agent.SetDestination(target.position);
        }

        Move();

        if (atacando)
        {
            transform.RotateAround(target.position, Vector3.back, 25f * Time.deltaTime);
        }

        if (scales)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);
        }
    }

    private void Move()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    IEnumerator FireRoutine()
    {
        // Espera inicial de 5 segundos
        yield return new WaitForSeconds(5f);

        atacando = false;

        if (agent != null && agent.isOnNavMesh)
            agent.isStopped = false;

        scales = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Mundo") && !startedAttack)
        {
            startedAttack = true;
            atacando = true;

            if (agent != null && agent.isOnNavMesh)
                agent.isStopped = true;

            fireCoroutine = StartCoroutine(FireRoutine());

            Destroy(gameObject, 10f);
        }

        if (collision.CompareTag("Shield"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Mundo"))
        {
            WordLife life = collision.gameObject.GetComponent<WordLife>();
            if (life != null)
            {
                collision.transform.DOScale(9.5f, 0.5f)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => {
                        if (collision.transform != null)
                            collision.transform.localScale = Vector3.one * 9.5f;
                    });

                life.TakeDamage(10);
            }
        }

        if (collision.gameObject.layer == 7)
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
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstance.transform.SetParent(null);
            Destroy(explosionInstance, 2f);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        isDead = true;

        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }

        StopAllCoroutines();

        agent = null;
        target = null;
        stats = null;
    }
}