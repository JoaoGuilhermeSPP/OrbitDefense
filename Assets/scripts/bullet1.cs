using DG.Tweening;
using UnityEngine;

public class bullet1 : MonoBehaviour
{
 

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mundo"))
        {
            collision.transform.DOScale(9.5f, 0.5f).SetLoops(2, LoopType.Yoyo);
            return;
        }
        Destroy(gameObject, 0.4f);
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
    }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {

            collision.gameObject.GetComponent<WordLife>().TakeDamage(0.4f);

            Destroy(gameObject);
        }
        if(collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<LifePlayer>().TakeDamage(1);
        }
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8) 
        {
            
            Destroy(gameObject);
        }

    }
}
