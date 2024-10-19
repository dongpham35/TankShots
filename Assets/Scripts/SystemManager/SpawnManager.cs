using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SpawnManager : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public List<Transform> spawnPoints = new List<Transform>();

    private const float MAX_DELAY_SPAWN = 5f;
    private const float MIN_DELAY_SPAWN = 1f;
    private const int SUM_ENEMY = 5;

    private float timerToSpawn;
    private int waySpawn;
    List<GameObject> enemies = new List<GameObject>();
    
    private void Awake()
    {
        waySpawn = 0;
        timerToSpawn = MAX_DELAY_SPAWN - waySpawn * 0.1f;
    }
    

    
    private void Update()
    {
        if(!isServer) return;
        if(timerToSpawn > 0) timerToSpawn -= Time.deltaTime;
        if (timerToSpawn <= 0 && PlayerController.GetTanks().Count >= 2)
        {
            SpawnTurnEnemy();
            timerToSpawn = MAX_DELAY_SPAWN - waySpawn * 0.1f;
            if(timerToSpawn <= MIN_DELAY_SPAWN) timerToSpawn = MIN_DELAY_SPAWN;
        }
    }
    


    [Server]
    private void SpawnTurnEnemy()
    {
        SpawnEnemy();
        waySpawn++;
    }

    [Server]
    private void SpawnEnemy()
    {
        for (int i = 0; i < SUM_ENEMY; i++)
        {
            int indexPoint = Random.Range(0, spawnPoints.Count-1);
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[indexPoint].position, Quaternion.identity);
            NetworkServer.Spawn(enemy);
            enemies.Add(enemy);
            TankAttack tankAttack = enemy.GetComponent<TankAttack>();
            TankMove tankMove = enemy.GetComponent<TankMove>();
            tankAttack.SetAllPlayer(PlayerController.GetTanks());
            tankMove.SetAllPlayer(PlayerController.GetTanks());
        }
    }
    
}
