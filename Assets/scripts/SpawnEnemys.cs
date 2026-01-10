using System.Collections;
using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    public Transform[] Spawnoints;
    public GameObject[] EnemyPrefs;
    public float spawnInterval = 3f;
    void Start()
    {
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
            int enemyIndex = Random.Range(0, EnemyPrefs.Length);
            GameObject enemyPref = EnemyPrefs[enemyIndex];

            Instantiate(enemyPref, Points.position, Points.rotation);
        }

    }
}
