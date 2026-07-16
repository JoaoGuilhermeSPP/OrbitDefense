using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WordLife : MonoBehaviour
{
    [Header("Vida")]
    public float MaxHealth = 100f;
    public float Life;

    [Header("UI")]
    public Image BarUI;

    [Header("Visual")]
    public SpriteRenderer Sr;
    public Sprite DerrotaA;
    public Color originalColor;

    [Header("Game Over")]
    public GameObject painelLoser;
    public GameObject ButtonsLoser;

    public static WordLife instance;

    private Tween damageTween;
    private Tween healTween;
    private bool isDead = false;

    private void Awake()
    {
        instance = this;

        Time.timeScale = 1f;
        Life = MaxHealth;

        if (painelLoser != null)
            painelLoser.SetActive(false);

        if (ButtonsLoser != null)
            ButtonsLoser.SetActive(false);

        if (Sr != null)
            originalColor = Sr.color;

        UpdateUI();
    }

    private void OnDestroy()
    {
        KillAllTweens();

        if (instance == this)
            instance = null;
    }

    private void KillAllTweens()
    {
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            damageTween = null;
        }

        if (healTween != null && healTween.IsActive())
        {
            healTween.Kill();
            healTween = null;
        }

        if (Sr != null)
            Sr.color = Color.white;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        Life = Mathf.Clamp(Life - damage, 0f, MaxHealth);

        UpdateUI();

        // Cancela tween anterior
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();

            if (Sr != null)
                Sr.color = originalColor;
        }

        // Animação de dano
        if (Sr != null)
        {
            damageTween = Sr.DOColor(Color.red, 0.01f)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    if (Sr != null)
                        Sr.color = originalColor;
                });
        }

        if (Life <= 0f && !isDead)
            Derrota();
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        Life = Mathf.Clamp(Life + amount, 0f, MaxHealth);

        if (SFXManager.current != null)
            SFXManager.current.PlayMusic(SFXManager.current.Life);

        UpdateUI();

        // Cancela tween de cura anterior
        if (healTween != null && healTween.IsActive())
        {
            healTween.Kill();

            if (Sr != null)
                Sr.color = originalColor;
        }

        // Cancela tween de dano
        if (damageTween != null && damageTween.IsActive())
        {
            damageTween.Kill();
            damageTween = null;
        }

        // Animação de cura
        if (Sr != null)
        {
            healTween = Sr.DOColor(Color.green, 0.5f)
                .SetLoops(6, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    if (Sr != null)
                        Sr.color = Color.white;
                });
        }
    }

    private void UpdateUI()
    {
        if (BarUI != null)
            BarUI.fillAmount = Life / MaxHealth;
    }

    public void Derrota()
    {
        if (isDead)
            return;

        isDead = true;

        KillAllTweens();

        // Sprite de derrota
        if (Sr != null)
        {
            Sr.sprite = DerrotaA;
            Sr.color = Color.white;
        }

        // Remove escudo
        if (PlayerMove.instance != null &&
            PlayerMove.instance.Shield != null)
        {
            PlayerMove.instance.Shield.SetActive(false);
        }

        // Game Over
        if (gameScore.instance != null)
            gameScore.instance.GameOver();

        StartCoroutine(SequenciaDerrota());
    }

    private IEnumerator SequenciaDerrota()
    {
        // Pequena pausa
        yield return new WaitForSeconds(1f);

        // Congela o jogo
        Time.timeScale = 0f;

        // Espera em tempo real
        yield return new WaitForSecondsRealtime(3f);

        // Exibe tela de derrota
        if (painelLoser != null)
            painelLoser.SetActive(true);

        if (ButtonsLoser != null)
            ButtonsLoser.SetActive(true);
    }
}