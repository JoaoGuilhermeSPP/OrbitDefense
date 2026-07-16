using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    public Transform[] Spawnoints;
    public GameObject[] EnemyPrefs;
    public float spawnInterval = 5f;
    public static SpawnEnemys instance;

    [Header("Limites WebGL")]
    public int maxEnemiesOnScreen = 15; // Limite máximo de inimigos

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;

    void Start()
    {
        instance = this;
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        activeEnemies.Clear();

        if (instance == this)
            instance = null;
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Limpa inimigos mortos da lista
            activeEnemies.RemoveAll(enemy => enemy == null);

            // Só spawna se não exceder o limite
            if (activeEnemies.Count >= maxEnemiesOnScreen)
            {
                Debug.Log($"Limite de inimigos atingido: {activeEnemies.Count}/{maxEnemiesOnScreen}");
                continue;
            }

            // Pontos
            int RdIndex = Random.Range(0, Spawnoints.Length);
            Transform Points = Spawnoints[RdIndex];

            // Inimigos
            int enemyIndex = Random.Range(0, EnemyPrefs.Length);
            GameObject enemyPref = EnemyPrefs[enemyIndex];

            if (enemyPref != null && Points != null)
            {
                GameObject newEnemy = Instantiate(enemyPref, Points.position, Points.rotation);
                activeEnemies.Add(newEnemy);
            }
        }
    }
}