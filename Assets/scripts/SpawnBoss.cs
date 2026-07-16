using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnBoss : MonoBehaviour
{
    public Transform[] Spawnoints;
    public GameObject[] BossPrefs;
    public float spawnInterval = 30f;
    public static SpawnBoss instance;
    void Start()
    {
        instance = this;
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            //Pontos
            int RdIndex = Random.Range(0, Spawnoints.Length);
            Transform Points = Spawnoints[RdIndex];
            //Inimigos
            int enemyIndex = Random.Range(0, BossPrefs.Length);
            GameObject enemyPref = BossPrefs[enemyIndex];

            Instantiate(enemyPref, Points.position, Points.rotation);
        }

    }
}
