using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleport : MonoBehaviour
{
    [Header("Teleport")]
    public string tagPontosTeleport = "TeleportPoint";

    private List<Transform> pontosDePulo = new();

    public float tempoVisivel = 3f;
    public float duracaoFade = 0.5f;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform pontoDeTiro;
    public Transform target;

    public int quantidadeDeMisseis = 3;
    public float intervaloEntreDisparos = 0.15f;

    [Header("Visual")]
    public float escalaBoss = 8f;

    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    private Vector3 escalaOriginal;
    private Vector3 posicaoOriginalFirePoint;

    private Transform pontoAtual;

    private Tween flutuacaoTween;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer não encontrado!");
            return;
        }

        escalaOriginal = Vector3.one * escalaBoss;

        transform.localScale = escalaOriginal;

        corOriginal = spriteRenderer.color;

        BuscarPontosTeleport();

        if (pontosDePulo.Count == 0)
        {
            Debug.LogError("Nenhum ponto de teleport encontrado!");
            return;
        }

        pontoAtual = pontosDePulo[Random.Range(0, pontosDePulo.Count)];

        transform.position = pontoAtual.position;

        if (pontoDeTiro != null)
        {
            posicaoOriginalFirePoint = pontoDeTiro.localPosition;
        }

        GameObject mundo = GameObject.FindGameObjectWithTag("Mundo");

        if (mundo != null)
        {
            target = mundo.transform;
        }

        AtualizarPontoDeTiro();

        StartCoroutine(CicloBoss());
    }

    private void BuscarPontosTeleport()
    {
        pontosDePulo.Clear();

        GameObject[] pontos = GameObject.FindGameObjectsWithTag(tagPontosTeleport);

        foreach (GameObject ponto in pontos)
        {
            pontosDePulo.Add(ponto.transform);
        }
    }

    IEnumerator CicloBoss()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempoVisivel);

            PararFlutuacao();

            // Fade OUT
            yield return spriteRenderer
                .DOFade(0f, duracaoFade)
                .WaitForCompletion();

            Teleportar();

            // Fade IN
            yield return spriteRenderer
                .DOFade(corOriginal.a, duracaoFade)
                .WaitForCompletion();

            IniciarFlutuacao();

            // Dispara os mísseis após teleportar
            StartCoroutine(DispararMisseis());
        }
    }

    private void Teleportar()
    {
        Transform novoPonto;

        do
        {
            novoPonto = pontosDePulo[
                Random.Range(0, pontosDePulo.Count)
            ];

        } while (
            novoPonto == pontoAtual &&
            pontosDePulo.Count > 1
        );

        pontoAtual = novoPonto;

        transform.position = pontoAtual.position;

        transform.localScale = escalaOriginal;

        spriteRenderer.color = corOriginal;

        AtualizarPontoDeTiro();
    }

    IEnumerator DispararMisseis()
    {
        for (int i = 0; i < quantidadeDeMisseis; i++)
        {
            CriarMissil();

            yield return new WaitForSeconds(intervaloEntreDisparos);
        }
    }

    private void CriarMissil()
    {
        if (
            bulletPrefab == null ||
            pontoDeTiro == null ||
            target == null
        )
        {
            return;
        }

        AtualizarPontoDeTiro();

        GameObject bullet = Instantiate(
            bulletPrefab,
            pontoDeTiro.position,
            Quaternion.identity
        );
        SFXManager.current.PlayMusic(SFXManager.current.BulletB);
        BulletBossTree missile =
            bullet.GetComponent<BulletBossTree>();

        if (missile != null)
        {
           

            missile.offsetRotacao =
                Random.Range(-25f, 25f);
        }
    }

    private void AtualizarPontoDeTiro()
    {
        if (target == null || pontoDeTiro == null)
            return;

        float direcao = target.position.x - transform.position.x;

        float lado = direcao > 0 ? 1f : -1f;

        Vector3 novaPosicao = posicaoOriginalFirePoint;

        novaPosicao.x =
            Mathf.Abs(posicaoOriginalFirePoint.x) * lado;

        pontoDeTiro.localPosition = novaPosicao;
    }

    private void IniciarFlutuacao()
    {
        flutuacaoTween?.Kill();

        flutuacaoTween = transform
            .DOMoveY(transform.position.y + 0.15f, 1.2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void PararFlutuacao()
    {
        flutuacaoTween?.Kill();
    }

    void Update()
    {
        transform.localScale = escalaOriginal;
    }

    void OnDestroy()
    {
        flutuacaoTween?.Kill();
    }
}