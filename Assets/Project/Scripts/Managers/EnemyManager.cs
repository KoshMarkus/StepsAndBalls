using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform dynamicObjectsTransform;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private List<GameObject> enemies;
        
        [Header("Horizontal enemy spawn position range")]
        
        [SerializeField] private float minSpawnPositionX;
        [SerializeField] private float maxSpawnPositionX;
        [SerializeField] private Vector3 spawnPosition;

        [Header("Enemy spawn speed")]
        
        [SerializeField] private float spawnThreshold;
        [SerializeField] private float spawnProgress;
        [SerializeField] private float spawnThresholdLesseningStep;
        [SerializeField] private float minSpawnThreshold;
        private float spawnThresholdAtStart;

        [Header("Enemy movement speed")]
        
        [SerializeField] private float enemyMoveThreshold;
        [SerializeField] private float enemyMoveThresholdLesseningStep;
        [SerializeField] private float minEnemyMoveThreshold;
        private float enemyMoveThresholdAtStart;

        [SerializeField] private bool gameStarted;

        private void Start()
        {
            enemyMoveThresholdAtStart = enemyMoveThreshold;
            spawnThresholdAtStart = spawnThreshold;
            
            spawnProgress = 0;
        }

        private void Update()
        {
            if (gameStarted)
            {
                spawnProgress += Time.deltaTime;

                if (spawnProgress >= spawnThreshold)
                {
                    SpawnEnemy();
                    spawnProgress = 0;
                } 
            }
        }

        private void SpawnEnemy()
        {
            var newSpawnPosition = new Vector3(Random.Range(minSpawnPositionX, maxSpawnPositionX), spawnPosition.y, spawnPosition.z);

            var newEnemy = Instantiate(enemyPrefab, newSpawnPosition, Quaternion.identity, dynamicObjectsTransform);
            newEnemy.GetComponent<Enemy>().SetMoveThreshold(minEnemyMoveThreshold, enemyMoveThreshold);
            
            enemies.Add(newEnemy);
        }
        
        private void ClearOutAllEnemies()
        {
            foreach (var enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        
        public void MoveSpawnPositionFurther()
        {
            var newSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y + 1, spawnPosition.z + 1);
            spawnPosition = newSpawnPosition;
        }

        //For UnityEvents
        
        //GameManager > onGameStarted
        
        public void GameStarted()
        {
            gameStarted = true;
        }
        
        //GameManager > onGameOver
        
        public void GameOver()
        {
            gameStarted = false;
            ClearOutAllEnemies();

            spawnThreshold = spawnThresholdAtStart;
            spawnProgress = 0;
            
            enemyMoveThreshold = enemyMoveThresholdAtStart;

        }
        
        //GameManager > onMilestioneReach
        
        public void LessenTheSpawnThreshold()
        {
            spawnThreshold -= spawnThresholdLesseningStep;
            
            if (spawnThreshold < minSpawnThreshold)
            {
                spawnThreshold = minSpawnThreshold;
            }
        }

        public void LessenEnemyMoveThreshold()
        {
            enemyMoveThreshold -= enemyMoveThresholdLesseningStep;

            if (enemyMoveThreshold < minEnemyMoveThreshold)
            {
                enemyMoveThreshold = minEnemyMoveThreshold;
            }
        }
    }
}