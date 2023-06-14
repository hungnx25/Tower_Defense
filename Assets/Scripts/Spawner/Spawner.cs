using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SpawModes
{
    Fixed, 
    Random
}

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SpawModes spawMode = SpawModes.Fixed;  
    [SerializeField] private int enemyCount = 10;
    //delay giua cac dot enemy xuat hien
    [SerializeField] private float delayBtwWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns;

    [Header("Random delay")]
    [SerializeField] private float minRandomDelay;
    [SerializeField] private float maxRandomDelay;

    private float spawnTimer;
    private int enemySpawned;
    private int _enemyRemaining;

    private WayPoint _waypoint;
    private ObjectPooler _pooler;
    // Start is called before the first frame update
    void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
        _waypoint = GetComponent<WayPoint>();

        _enemyRemaining = enemyCount;
    }    

    // Update is called once per frame
    void Update()
    {   
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0)
        {
            spawnTimer = GetSpawnDelay();
            if(enemySpawned < enemyCount)
            {
                enemySpawned++;
                SpawnEnemy();
            }
        }
    }

    //tao moi enemy
    private void SpawnEnemy()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.WayPoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;
        newInstance.SetActive(true);
    }

    //delay giua 2 enemy xuat hien
    private float GetSpawnDelay()
    {
        float delay = 0f;
        if(spawMode == SpawModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }

        return delay;
    }

    //get random time
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay);
        return randomTimer;
    }

    //bat dau wave moi sau delay time
    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        _enemyRemaining = enemyCount;
        spawnTimer = 0f;
        enemySpawned = 0;
    }

    //ghi lai so luong enemy con lai de bat dau wave moi
    private void RecordEnemy(Enemy enemy)
    {
        _enemyRemaining--;
        if(_enemyRemaining <= 0)
        {
            StartCoroutine(NextWave());
        }
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
    }
}
