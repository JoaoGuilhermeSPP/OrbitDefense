using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    private EnemyLife EnemyLife;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mundo"))
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyLife>().takeDamage(10);
        }

        GameObject explosionInstace = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(explosionInstace,1f);

    }
   
}
