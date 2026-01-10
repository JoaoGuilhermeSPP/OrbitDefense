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

    private Vector3 targetScale = Vector3.one;

    void Start()
    {
        stats = GetComponent<EnemyLife>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false; // IMPORTANTE PARA 2D

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");
        if (mundo != null)
        {
            target = mundo.transform;
        }
    }

    void Update()
    {
       
        if (target == null)
        {
            agent.isStopped = true;
            return;
        }

        agent.SetDestination(target.position);
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

    IEnumerator Fire(float delay)
    {
        yield return new WaitForSeconds(delay);

        atacando = false;
        agent.isStopped = false;
        scales = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mundo"))
        {
            atacando = true;
            agent.isStopped = true;

            //  Dispara UMA ÚNICA coroutine
            StartCoroutine(Fire(5f));

            // Autodestruição segura
            Destroy(gameObject, 10f);
        }

        if (collision.CompareTag("Shield"))
        {
            GameObject explosionInstance =
                Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(explosionInstance, 2f);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mundo"))
        {
            WordLife life = collision.gameObject.GetComponent<WordLife>();
            if (life != null)
            {
                life.takeDamage(10);
            }
        }
    }
}
