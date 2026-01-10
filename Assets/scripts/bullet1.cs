using UnityEngine;

public class bullet1 : MonoBehaviour
{
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mundo"))
            return;
      
        Destroy(gameObject, 0.4f);
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
           
           collision.gameObject.GetComponent<WordLife>().takeDamage(1);
        }
    }
}
