using DG.Tweening;
using TMPro;
using UnityEngine;

public class gameScore : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    private int Score;
    public static gameScore instance;

    void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public int _Score()
    {
        return Score;
    }

    public void GameOver()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.SaveScore(Score);
        }
        else
        {
            Debug.LogWarning("SaveManager não encontrado ao salvar score");
        }
    }

    public void ScoreCollect()
    {
        Score = Score + 10;

        if (ScoreText != null)
        {
            ScoreText.text = Score.ToString();
            ScoreText.transform.DOScale(2f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        SpeedSpawn();
    }

    public void SpeedSpawn()
    {
        if (SpawnEnemys.instance == null) return;

        if (Score >= 1000)
        {
            SpawnEnemys.instance.spawnInterval = 0.7f;
            Debug.Log("Dificuldade: Máxima (0.7s)");
        }
        else if (Score >= 750)
        {
            SpawnEnemys.instance.spawnInterval = 1.5f;
            Debug.Log("Dificuldade: Alta (1.5s)");
        }
        else if (Score >= 500)
        {
            SpawnEnemys.instance.spawnInterval = 2f;
            Debug.Log("Dificuldade: Média (2s)");
        }
        else if (Score >= 250)
        {
            SpawnEnemys.instance.spawnInterval = 2.4f;
            Debug.Log("Dificuldade: Baixa (2.4s)");
        }
        else
        {
            SpawnEnemys.instance.spawnInterval = 3f;
        }
    }
}