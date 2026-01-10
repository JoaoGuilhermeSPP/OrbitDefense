using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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
    public float FireTime = 0.2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        atacando = false;
        stats = GetComponent<EnemyLife>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");
        if (mundo != null)
        {
            target = mundo.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position);
            float angles = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angles + 90f);
        }
        if (atacando == true)
        {
            transform.RotateAround(target.transform.position, Vector3.back, 10 * Time.deltaTime);
            StartCoroutine(Fire(8f));
                
        }
    }
    IEnumerator Fire(float delay)
    {
      
        yield return new WaitForSeconds(delay);

      
        if (Time.time >= FireTime)
        {
            GameObject bullet = Instantiate(BulletPrefab, point.position, point.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(point.up * -force, ForceMode2D.Impulse);

            FireTime = Time.time + fireRate;
 
            Destroy(bullet, 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mundo"))
        {
            atacando = true;
            agent.isStopped = true;
        }
    }
}
