using DG.Tweening;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    public EnemyScript enemySettigns;

    public int Life;
    public int speed;
    public int Damage;

    private SpriteRenderer Sr;
    private Color OrigColor;
    public GameObject explosion;

    private Tween damageTween; // Guarda referência do Tween

    void Awake()
    {
        Life = enemySettigns.Life;
        speed = enemySettigns.speed;
        Damage = enemySettigns.Damage;
        Sr = gameObject.GetComponent<SpriteRenderer>();
        OrigColor = Sr.color;
    }

    private void OnDestroy()
    {
        // MATA o Tween antes de destruir o objeto!
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            damageTween = null;
        }

        // Restaura a cor original (segurança)
        if (Sr != null)
            Sr.color = OrigColor;
    }

    public void takeDamage(int damage)
    {
        Life -= damage;

        // Mata tween anterior se existir
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            Sr.color = OrigColor;
        }

        // Cria nova animação e guarda referência
        damageTween = Sr.DOColor(Color.red, 0.2f)
            .SetLoops(7, LoopType.Yoyo)
            .OnComplete(() =>
            {
                if (Sr != null)
                    Sr.color = Color.white;
            });

        if (Life <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Mata o Tween ANTES de destruir
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            damageTween = null;
        }

        // Efeito de explosão
        if (explosion != null)
        {
            GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            explosionInstance.transform.SetParent(null); // Garante independência
            Destroy(explosionInstance, 2f);
        }

        // Som
        if (SFXManager.current != null)
            SFXManager.current.PlayMusic(SFXManager.current.splosion);

        // Missões
        if (MissionManager.instance != null)
        {
            MissionManager.instance.AddMissionProgress(Mission.Score, 10);
            MissionManager.instance.AddMissionProgress(Mission.KillEnemies, 1);
        }
        else
        {
            Debug.LogWarning("MissionManager não encontrado ao destruir EnemyLife");
        }

        // Score
        if (gameScore.instance != null)
            gameScore.instance.ScoreCollect();

        // Destroi o objeto
        Destroy(gameObject);
    }
}