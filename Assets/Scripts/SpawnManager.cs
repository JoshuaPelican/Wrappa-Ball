using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Credits")]
    [SerializeField] float CreditTimer = 1;
    [SerializeField] int StartingCredits = 10;

    [Header("Enemies")]
    [SerializeField] EnemyData[] Enemies;

    public EnemyData AFKKiller;


    [Header("Waves")]
    [SerializeField] Vector2 WaveTimerRange = new Vector2(5f, 10f);
    [SerializeField] int MaxEnemyCount = 10;

    [SerializeField] GameObject SpawnWarning;
    [SerializeField] float StartingSpawnTimer = 3f;
    [SerializeField] float SpawnDelay = 1.5f;
    [SerializeField] AudioClip[] SpawnClips;

    int currentCredits;

    float creditTimer;
    float spawnTimer;

    List<int> weightedEnemyPool;
    EnemyData selectedEnemy;
    int currentEnemyCount => GameObject.FindGameObjectsWithTag("Enemy").Length;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        creditTimer -= Time.deltaTime; //Credits

        if (creditTimer <= 0)
        {
            GainCredits(Mathf.RoundToInt(1 * (1 + (GameManager.GameTime / 6000))));
        }


        spawnTimer -= Time.deltaTime; //Spawning

        if(spawnTimer <= 0)
        {
            TrySpawnWave();
        }
    }

    void Init()
    {
        currentCredits = StartingCredits;

        spawnTimer = StartingSpawnTimer;

        weightedEnemyPool = new List<int>(); //Initialize weighted enemy pool using weights

        for (int i = 0; i < Enemies.Length; i++)
        {
            for (int j = 0; j < Enemies[i].Weight; j++)
            {
                weightedEnemyPool.Add(i);
            }
        }
    }

    void GainCredits(int credits)
    {
        currentCredits += credits;

        creditTimer = CreditTimer;
    }


    void TrySpawnWave()
    {
        //Check current enemy count
        if (currentEnemyCount >= MaxEnemyCount)
        {
            FailSpawn(1);
            return;
        }

        //Select random enemy, using weights
        if(selectedEnemy == null)
            selectedEnemy = Enemies[weightedEnemyPool[Random.Range(0, weightedEnemyPool.Count)]];

        if (GameManager.GameTime < selectedEnemy.TimeRequired)
        {
            FailSpawn(0.1f);
            return;
        }

        //Try Spawning it
        if (currentCredits < selectedEnemy.CreditCost)
        {
            FailSpawn(1);
            return;
        }

        bool spawnElite = false;
        if (currentCredits > selectedEnemy.CreditCost * 3 && GameManager.GameTime > (selectedEnemy.TimeRequired + 30) * 2)
            spawnElite = true;

        if (currentCredits > (spawnElite ? selectedEnemy.CreditCost * 18 :selectedEnemy.CreditCost * 6))
        {
            FailSpawn(0.75f);
            return;
        }

        StartCoroutine(SpawnEnemy(selectedEnemy, spawnElite));

        //Reset spawn timer, but with less time
        spawnTimer = Random.Range(WaveTimerRange.x, WaveTimerRange.y) * 0.1f;
    }

    void FailSpawn(float timerMultiplier)
    {
        //Reset spawn timer
        spawnTimer = Random.Range(WaveTimerRange.x, WaveTimerRange.y) * timerMultiplier;
        selectedEnemy = null;
    }

    IEnumerator SpawnEnemy(EnemyData enemy, bool isElite)
    {
        int i = 0;
        Vector2 randomScreenPosition = CameraUtility.RandomCameraPosition(enemy.EnemyPrefab.transform.localScale * 1.5f);
        while (Physics2D.OverlapCircle(randomScreenPosition, enemy.EnemyPrefab.transform.localScale.magnitude) && i < 100)
        {
            randomScreenPosition = CameraUtility.RandomCameraPosition(enemy.EnemyPrefab.transform.localScale * 1.5f);
            i++;
        }

        if (i >= 100)
        {
            Debug.LogError("Valid Spawn Not Found!");
            yield break;
        }

        AudioManager.PlayAudioRandom(SpawnClips, 0.5f, 0.75f);

        Destroy(Instantiate(SpawnWarning, randomScreenPosition, Quaternion.identity), SpawnDelay);

        yield return new WaitForSeconds(SpawnDelay);

        GameObject newEnemy = Instantiate(enemy.EnemyPrefab, randomScreenPosition, Quaternion.identity);
        Health enemyHealth = newEnemy.GetComponent<Health>();

        enemyHealth.SetEnemyData(enemy, isElite);

        if (isElite)
        {
            currentCredits -= enemy.CreditCost * 2;
            newEnemy.transform.localScale *= 2f;
            enemyHealth.DoubleHealth();
        }

        currentCredits -= enemy.CreditCost;
    }

    public void SpawnAFKKiller()
    {
        Vector2 worldBounds = CameraUtility.CameraWorldBounds(Vector2.one * -2);
        Vector2 randomOutsideScreenPosition = CameraUtility.RandomCameraPosition(Vector2.zero);


        if(Mathf.Abs(randomOutsideScreenPosition.x) > Mathf.Abs(randomOutsideScreenPosition.y))
        {
            if (randomOutsideScreenPosition.x >= 0)
                randomOutsideScreenPosition.x += worldBounds.x;
            else
                randomOutsideScreenPosition.x -= worldBounds.x;
        }
        else
        {
            if (randomOutsideScreenPosition.y >= 0)
                randomOutsideScreenPosition.y += worldBounds.y;
            else
                randomOutsideScreenPosition.y -= worldBounds.y;
        }

        Instantiate(AFKKiller.EnemyPrefab, randomOutsideScreenPosition, Quaternion.identity);
    }
}
