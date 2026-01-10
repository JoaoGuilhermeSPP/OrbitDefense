using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class Enemys : MonoBehaviour
{
   
    [SerializeField] Transform Target;
    NavMeshAgent Agent;
   public EnemyLife stats;
    Vector3 targetScale  = Vector3.zero;
    bool scales = false;
    public GameObject explosion;
    void Start()
    {
        stats = GetComponent<EnemyLife>();
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");
        if(mundo != null)
        {
            Target = mundo.transform;
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        Agent.SetDestination(Target.position);
        if (Target != null)
        {
            Vector3 direction  = (Target.position - transform.position);
            float angles = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
            transform.rotation =Quaternion.Euler(0, 0, angles - 90f);
        }
        if (scales == true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 1);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mundo"))
        {
            scales = true;
            Destroy(gameObject, 5f);
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            GameObject explosionInstace = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosionInstace, 2f);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<WordLife>().takeDamage(10);
        }
    }
}

