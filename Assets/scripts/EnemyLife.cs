using NUnit.Framework.Internal;
using System.Collections;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public EnemyScript enemySettigns;
    public int Life;
    public int speed;
    public int Damage;
    SpriteRenderer Sr;
    Color OrigColor;
    public GameObject explosion;
    
    
    void Start()
    {
        Life = enemySettigns.Life;
        speed = enemySettigns.speed;
        Damage = enemySettigns.Damage;
        Sr  = gameObject.GetComponent<SpriteRenderer>();
        OrigColor = Sr.color;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public  void takeDamage(int damage)
    {
        Life -= damage;

        StartCoroutine(Effect());

        if (Life <= 0)
        {
            Die();
        }

    }
    void Die()
    {
        GameObject explosionInstace = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionInstace, 2f);
        Destroy(gameObject);
    }
   public IEnumerator Effect()
    {
        Sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        Sr.color = OrigColor;

    }
    
}
