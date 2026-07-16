using DG.Tweening;
using UnityEngine;

public class BossLife : MonoBehaviour
{
    public EnemyScript bossStatus;
    public int Life;
    public int speed;
    public int Damage;

    [Header("Recompensa ao derrotar o chefe")]
    public float healAmount = 20f; // Cura o planeta

    private SpriteRenderer Sr;
    private Color OrigColor;

    public GameObject explosion;

    private Tween damageTween;

    void Awake()
    {
        Life = bossStatus.Life;
        speed = bossStatus.speed;
        Damage = bossStatus.Damage;

        Sr = GetComponent<SpriteRenderer>();

        if (Sr != null)
            OrigColor = Sr.color;
    }

    private void OnDestroy()
    {
        // Mata o Tween antes de destruir
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            damageTween = null;
        }

        if (Sr != null)
            Sr.color = OrigColor;
    }

    public void takeDamage(int damage)
    {
        Life -= damage;

        // Mata tween anterior
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();

            if (Sr != null)
                Sr.color = OrigColor;
        }

        // Nova animação de dano
        if (Sr != null)
        {
            damageTween = Sr.DOColor(Color.red, 0.2f)
                .SetLoops(7, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    if (Sr != null)
                        Sr.color = OrigColor;
                });
        }

        // Morreu?
        if (Life <= 0)
        {
            Die();

            // Outros bônus opcionais
            if (PowerUp.instance != null)
                PowerUp.instance.Healer();
        }
    }

    void Die()
    {
        // Mata Tween antes de destruir
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            damageTween = null;
        }

        // Cura o planeta
        if (WordLife.instance != null)
        {
            WordLife.instance.Heal(healAmount);
        }

        // Explosão
        if (explosion != null)
        {
            GameObject explosionInstance =
                Instantiate(explosion, transform.position, Quaternion.identity);

            explosionInstance.transform.SetParent(null);

            Destroy(explosionInstance, 2f);
        }

        // Som
        if (SFXManager.current != null)
        {
            SFXManager.current.PlayMusic(SFXManager.current.ExplosionB);
        }

        // Missões
        if (MissionManager.instance != null)
        {
            MissionManager.instance.AddMissionProgress(Mission.Score, 10);
            MissionManager.instance.AddMissionProgress(Mission.KillBoss, 1);
        }
        else
        {
            Debug.LogWarning("MissionManager não encontrado ao destruir BossLife");
        }

        // Score (apenas uma vez)
        if (gameScore.instance != null)
        {
            gameScore.instance.ScoreCollect();
        }

        // Destrói o chefe
        Destroy(gameObject);
    }
}