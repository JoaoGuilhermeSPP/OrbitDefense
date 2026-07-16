using System.Collections;
using UnityEngine;

public class PowerUpMecanic : MonoBehaviour
{
    public static PowerUpMecanic instance;

    [Header("Spawn")]
    public GameObject buffPrefab;
    public float spawnTime = 25f;

    private GameObject currentBuff;
    private bool isSpawning;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (currentBuff == null && !isSpawning)
        {
            StartCoroutine(SpawnBuff());
        }
    }
    

    private IEnumerator SpawnBuff()
    {
        isSpawning = true;

        yield return new WaitForSeconds(spawnTime);

        currentBuff = Instantiate(
            buffPrefab,
            transform.position,
            Quaternion.identity
        );

        isSpawning = false;
    }


    public void RandomBuff()
    {
        if (PowerUp.instance == null) return;

        // Sorteia de 1 a 4 (o 5 não conta no int)
        int power = UnityEngine.Random.Range(1, 5);

        switch (power)
        {
            case 1: PowerUp.instance.BuffBullet(); break;
            case 2: PowerUp.instance.ShieldBuff(); break;
            case 3: PowerUp.instance.SpeedBuff(); break;
            case 4: PowerUp.instance.Healer(); break;
        }
        Debug.Log("Buff sorteado: " + power);
    }


  
    public void CollectBuff(GameObject buff)
    {
        RandomBuff();
        SFXManager.current.PlayMusic(SFXManager.current.Buff);
        Destroy(buff);
        currentBuff = null;
    }
}
