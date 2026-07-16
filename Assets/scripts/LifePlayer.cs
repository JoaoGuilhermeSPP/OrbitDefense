using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifePlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image[] bars;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer sprite;
    public SpriteRenderer Sr;
    public Sprite DerrotaB;

    [Header("Configuração")]
    [SerializeField] private int maxLife = 10;
    [SerializeField] private float invulnerableTime = 1f;

    [Header("Derrota")]
    public GameObject painelLoser;
    public GameObject ButtonsLoser;

    private int currentLife;
    private bool isInvulnerable;
    private bool isDead;

    private Coroutine blinkCoroutine;
    private Coroutine invulnerabilityCoroutine;

    private Vector3 playerOriginalScale;
    private Vector3 srOriginalScale;
    private Color srOriginalColor;

    private void Awake()
    {
        Time.timeScale = 1f;

        currentLife = maxLife;
        isDead = false;
        isInvulnerable = false;

        playerOriginalScale = transform.localScale;

        if (Sr != null)
        {
            srOriginalScale = Sr.transform.localScale;
            srOriginalColor = Sr.color;
        }

        if (painelLoser != null)
            painelLoser.SetActive(false);

        if (ButtonsLoser != null)
            ButtonsLoser.SetActive(false);
    }

    private void Start()
    {
        UpdateLifeUI();
    }

    private void LateUpdate()
    {
        if (!isDead)
        {
            transform.localScale = playerOriginalScale;

            if (Sr != null)
            {
                Sr.transform.localScale = srOriginalScale;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isInvulnerable || damage <= 0)
            return;

        currentLife -= damage;

        if (currentLife < 0)
            currentLife = 0;

        UpdateLifeUI();

        if (currentLife <= 0)
        {
            Derrota();
            return;
        }

        if (invulnerabilityCoroutine != null)
            StopCoroutine(invulnerabilityCoroutine);

        invulnerabilityCoroutine =
            StartCoroutine(Invulnerability());
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(Mathf.CeilToInt(damage));
    }

    public void Heal(int amount)
    {
        if (isDead || amount <= 0)
            return;

        currentLife += amount;

        if (currentLife > maxLife)
            currentLife = maxLife;

        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        Color color = Color.green;

        if (currentLife <= 3)
            color = Color.red;
        else if (currentLife <= 5)
            color = Color.yellow;

        for (int i = 0; i < bars.Length; i++)
        {
            if (bars[i] == null)
                continue;

            bars[i].enabled = i < currentLife;

            Color c = bars[i].color;
            c.r = color.r;
            c.g = color.g;
            c.b = color.b;

            bars[i].color = c;
        }

        StopBlink();

        if (currentLife == 1 &&
            bars.Length > 0 &&
            bars[0] != null)
        {
            blinkCoroutine =
                StartCoroutine(BlinkBar(bars[0]));
        }
        else
        {
            foreach (Image bar in bars)
            {
                if (bar == null)
                    continue;

                Color c = bar.color;
                c.a = 1f;
                bar.color = c;
            }
        }
    }

    private IEnumerator BlinkBar(Image bar)
    {
        while (!isDead)
        {
            Color c = bar.color;
            c.a = 0.2f;
            bar.color = c;

            yield return new WaitForSeconds(0.2f);

            c.a = 1f;
            bar.color = c;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        int playerLayer =
            LayerMask.NameToLayer("Player");

        int enemyLayer =
            LayerMask.NameToLayer("Enemy");

        if (playerLayer != -1 &&
            enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(
                playerLayer,
                enemyLayer,
                true
            );
        }

        for (int i = 0; i < 5; i++)
        {
            if (isDead)
                yield break;

            if (sprite != null)
                sprite.enabled = false;

            yield return new WaitForSeconds(0.1f);

            if (sprite != null)
                sprite.enabled = true;

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(
            invulnerableTime
        );

        if (playerLayer != -1 &&
            enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(
                playerLayer,
                enemyLayer,
                false
            );
        }

        if (!isDead)
            isInvulnerable = false;

        invulnerabilityCoroutine = null;
    }

    private void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    public void Derrota()
    {
        if (isDead)
            return;

        isDead = true;
        isInvulnerable = true;

        StopBlink();

        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
            invulnerabilityCoroutine = null;
        }

        int playerLayer =
            LayerMask.NameToLayer("Player");

        int enemyLayer =
            LayerMask.NameToLayer("Enemy");

        if (playerLayer != -1 &&
            enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(
                playerLayer,
                enemyLayer,
                false
            );
        }

        if (sprite != null)
            sprite.enabled = true;

        Animator anim =
            GetComponent<Animator>();

        if (anim != null)
            anim.enabled = false;

        if (Sr != null)
        {
            Sr.sprite = DerrotaB;
            Sr.color = srOriginalColor;
            Sr.transform.localScale =
                srOriginalScale;
        }

        transform.localScale =
            playerOriginalScale;

        if (PlayerMove.instance != null &&
            PlayerMove.instance.Shield != null)
        {
            PlayerMove.instance.Shield
                .SetActive(false);
        }

        if (gameScore.instance != null)
        {
            gameScore.instance.GameOver();
        }

        StartCoroutine(
            SequenciaDerrota()
        );
    }

    private IEnumerator SequenciaDerrota()
    {
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3f);

        if (painelLoser != null)
            painelLoser.SetActive(true);

        if (ButtonsLoser != null)
            ButtonsLoser.SetActive(true);
    }

    private void OnDisable()
    {
        StopBlink();
    }

    private void OnDestroy()
    {
        StopBlink();

        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
        }
    }
}