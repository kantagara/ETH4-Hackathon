using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private EnemyController[] enemies;
    [FormerlySerializedAs("SpawnPoints")] [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float spawnTime = 2f;

    public List<EnemyController> SpawnedEnemies = new ();
    private List<Action> handlers;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            var enemyPrefab = enemies[Random.Range(0, enemies.Length)];
            var enemyInstance = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            SpawnedEnemies.Add(enemyInstance);
            enemyInstance.Damageable.OnDeath += () =>
            {
                SpawnedEnemies.Remove(enemyInstance);
            };
            yield return new WaitForSeconds(spawnTime * animationCurve.Evaluate(Time.timeSinceLevelLoad));

        }
    }

   
}