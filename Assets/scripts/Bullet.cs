using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public GameObject explosion;

    private bool hasExploded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        if (collision.CompareTag("Mundo"))
            return;

        if (collision.CompareTag("Shield"))
            return;

        if (collision.CompareTag("bullet"))
            return;

        // ===== BUFF =====
        if (collision.CompareTag("Buff"))
        {
            Debug.Log($"Bullet atingiu Buff: {collision.gameObject.name}");

            if (PowerUpMecanic.instance != null)
            {
                PowerUpMecanic.instance.CollectBuff(collision.gameObject);

                if (SFXManager.current != null)
                    SFXManager.current.PlayMusic(SFXManager.current.Buff);
            }
            else
            {
                Debug.LogWarning("PowerUpMecanic.instance é NULL ao coletar buff!");
            }

            Explode();
            return;
        }
        // ===== ENEMY =====
        if (collision.CompareTag("Enemy"))
        {
            EnemyLife enemyLife = collision.GetComponent<EnemyLife>();
            if (enemyLife != null)
                enemyLife.takeDamage(10);
        }

        if (collision.CompareTag("Boss"))
        {
            BossLife bossLife = collision.GetComponent<BossLife>();
            if (bossLife != null)
                bossLife.takeDamage(10);
        }

        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosion != null)
        {
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstance.transform.SetParent(null); // Garante independência
            Destroy(explosionInstance, 1f);
        }

        Destroy(gameObject);
    }
}