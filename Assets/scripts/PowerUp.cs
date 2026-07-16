using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PowerUp : MonoBehaviour
{
    public static PowerUp instance;

    [Header("UI")]
    public Image PowerUp1, PowerUp2, PowerUp3;

    // ================= SPEED BUFF =================
    [Header("Speed Buff")]
    public float speedMultiplier = 2f;
    public GameObject effectShoot;
    public float speedTime = 30f;

    private bool speedActive;
    private Coroutine speedCoroutine;

    // ================= BULLET BUFF =================
    [Header("Bullet Buff")]
    public GameObject bulletDouble;
    public GameObject bulleteffect;
    public float bulletTime = 30f;
    public float offSetX = 0.3f;

    private bool bulletActive;
    private Coroutine bulletCoroutine;

    // ================= SHIELD BUFF =================
    [Header("Shield Buff")]
    public GameObject prefShield;
    public float shieldTime = 30f;

    private GameObject shieldInstance;
    private bool shieldActive;
    private Coroutine shieldCoroutine;

    // ================= HEALTH =================
    [Header("Health")]
    public GameObject healtheffect;

    // Array para suportar ambos sistemas de vida
    public WordLife worldLife;
    public LifePlayer playerLife;

    // ================= ORIGINAL VALUES =================
    private float originalSpeed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Guarda velocidade original do tiro
        if (PlayerMove.instance != null)
            originalSpeed = PlayerMove.instance.bulletForce;

   
      
    }

    private void OnDestroy()
    {
        // Para todas as corrotinas ativas
        StopAllCoroutines();

        if (instance == this)
            instance = null;
    }

 
    public void SpeedBuff()
    {
        if (PlayerMove.instance == null) return;

        // Se já estiver ativo apenas reseta o tempo
        if (speedActive)
        {
            if (speedCoroutine != null)
                StopCoroutine(speedCoroutine);

            speedCoroutine = StartCoroutine(StopSpeed());
            return;
        }

        speedActive = true;

        // Aplica apenas UMA vez
        PlayerMove.instance.bulletForce = originalSpeed * speedMultiplier;

        if (effectShoot != null)
            effectShoot.SetActive(true);

        speedCoroutine = StartCoroutine(StopSpeed());
    }

    IEnumerator StopSpeed()
    {
        float t = 0f;

        if (PowerUp2 != null)
            PowerUp2.fillAmount = 0f;

        while (t < speedTime)
        {
            t += Time.deltaTime;

            if (PowerUp2 != null)
                PowerUp2.fillAmount = t / speedTime;

            yield return null;
        }

        // Volta velocidade original
        if (PlayerMove.instance != null)
            PlayerMove.instance.bulletForce = originalSpeed;

        speedActive = false;

        if (effectShoot != null)
            effectShoot.SetActive(false);

        if (PowerUp2 != null)
            PowerUp2.fillAmount = 0f;
    }

    // =========================================================
    // BULLET BUFF
    // =========================================================
    public void BuffBullet()
    {
        // Se já estiver ativo apenas reseta o tempo
        if (bulletActive)
        {
            if (bulletCoroutine != null)
                StopCoroutine(bulletCoroutine);

            bulletCoroutine = StartCoroutine(StopBullet());
            return;
        }

        bulletActive = true;

        if (bulleteffect != null)
            bulleteffect.SetActive(true);

        bulletCoroutine = StartCoroutine(StopBullet());
    }

    public bool IsBulletBuffActive()
    {
        return bulletActive;
    }

    IEnumerator StopBullet()
    {
        float t = 0f;

        if (PowerUp3 != null)
            PowerUp3.fillAmount = 0f;

        while (t < bulletTime)
        {
            t += Time.deltaTime;

            if (PowerUp3 != null)
                PowerUp3.fillAmount = t / bulletTime;

            yield return null;
        }

        bulletActive = false;

        if (bulleteffect != null)
            bulleteffect.SetActive(false);

        if (PowerUp3 != null)
            PowerUp3.fillAmount = 0f;
    }

    // =========================================================
    // SHIELD BUFF
    // =========================================================
    public void ShieldBuff()
    {
        // Se já estiver ativo apenas reseta o tempo
        if (shieldActive)
        {
            if (shieldCoroutine != null)
                StopCoroutine(shieldCoroutine);

            shieldCoroutine = StartCoroutine(StopShield());
            return;
        }

        shieldActive = true;

        if (prefShield != null)
        {
            shieldInstance = Instantiate(
                prefShield,
                transform.position,
                transform.rotation,
                transform
            );
        }

        shieldCoroutine = StartCoroutine(StopShield());
    }

    IEnumerator StopShield()
    {
        float t = 0f;

        if (PowerUp1 != null)
            PowerUp1.fillAmount = 0f;

        while (t < shieldTime)
        {
            t += Time.deltaTime;

            if (PowerUp1 != null)
                PowerUp1.fillAmount = t / shieldTime;

            yield return null;
        }

        if (shieldInstance != null)
            Destroy(shieldInstance);

        shieldActive = false;

        if (PowerUp1 != null)
            PowerUp1.fillAmount = 0f;
    }

    // =========================================================
    // HEAL - CORRIGIDO!
    // =========================================================
    public void Healer()
    {
        Debug.Log("Healer() chamado");

        bool healed = false;

        // Tenta curar o LifePlayer primeiro (jogador)
        if (playerLife != null)
        {
            Debug.Log("Curando LifePlayer");
            playerLife.Heal(30);
            healed = true;

            // Efeito na posição do player
            if (healtheffect != null && playerLife.transform != null)
            {
                GameObject fx = Instantiate(
                    healtheffect,
                    playerLife.transform.position,
                    Quaternion.identity
                );
                fx.transform.SetParent(null);
                Destroy(fx, 2f);
            }
        }

        // Também cura o WordLife se existir (vida do mundo)
        if (worldLife != null)
        {
            Debug.Log("Curando WordLife");
            worldLife.Heal(30);
            healed = true;

            // Efeito na posição do mundo
            if (healtheffect != null && worldLife.transform != null && playerLife == null)
            {
                GameObject fx = Instantiate(
                    healtheffect,
                    worldLife.transform.position,
                    Quaternion.identity
                );
                fx.transform.SetParent(null);
                Destroy(fx, 2f);
            }
        }

        if (!healed)
        {
            Debug.LogWarning("Healer: Nenhum sistema de vida encontrado!");
        }
    }
}