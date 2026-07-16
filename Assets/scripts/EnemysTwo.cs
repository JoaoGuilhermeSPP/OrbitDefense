using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemysTwo : MonoBehaviour
{
    public bool atacando;
    NavMeshAgent agent;
    public EnemyLife stats;
    public Transform target;

    public GameObject BulletPrefab;
    public Transform point;
    public float force = 20f;
    public float fireRate = 0.5f;
    private float FireTime = 0.2f;

    private bool isDead = false;
    private bool hasStartedAttack = false;
    private Coroutine fireCoroutine;

    void Start()
    {
        atacando = false;
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

        // Só seta destino se estiver no NavMesh
        if (agent.isOnNavMesh && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
        }

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position);
            float angles = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angles + 90f);
        }

        if (atacando)
        {
            transform.RotateAround(target.transform.position, Vector3.back, 10 * Time.deltaTime);
        }
    }

    IEnumerator FireRoutine()
    {
        while (atacando && !isDead)
        {
            yield return new WaitForSeconds(fireRate);

            if (!atacando || isDead || BulletPrefab == null || point == null)
                yield break;

            if (Time.time >= FireTime)
            {
                GameObject bullet = Instantiate(BulletPrefab, point.position, point.rotation);

                if (SFXManager.current != null)
                    SFXManager.current.PlayMusic(SFXManager.current.bulletEnemys);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(point.up * -force, ForceMode2D.Impulse);
                }

                FireTime = Time.time + fireRate;
                Destroy(bullet, 2f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Mundo") && !hasStartedAttack)
        {
            hasStartedAttack = true;
            atacando = true;

            if (agent != null && agent.isOnNavMesh)
                agent.isStopped = true;

            // Inicia UMA ÚNICA corrotina de disparo
            fireCoroutine = StartCoroutine(FireRoutine());

            // Autodestruição após 8 segundos
            Destroy(gameObject, 8f);
        }
    }

    private void OnDestroy()
    {
        isDead = true;
        atacando = false;

        // Para a corrotina de disparo
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