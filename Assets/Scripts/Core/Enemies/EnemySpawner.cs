using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core.Level;
using Scripts.Core.Player;
using Scripts.Misc;
using Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Enemies {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private List<Enemy> _possibleEnemies;
        [SerializeField] private Transform _playerTrans;
        [SerializeField] private FloatReference _validSpawnRadius;
        [SerializeField] private LevelManager _levelManager;

        private List<Enemy> _spawnedEnemies = new();
        private int lastEnemySpawnIndex = -1;

        public void Spawn(int num) {
            IEnumerator DelayedSpawn() {
                yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));

                // Choose an enemy that is different than the last one spawned
                int randomIndex = Random.Range(1, _possibleEnemies.Count) + lastEnemySpawnIndex;
                randomIndex %= _possibleEnemies.Count;
                Enemy enemyToSpawn = _possibleEnemies[randomIndex];
                lastEnemySpawnIndex = randomIndex;

                var playerPosition = _playerTrans.position;
                var newEnemy = Instantiate(enemyToSpawn, transform);
                _spawnedEnemies.Add(newEnemy);
            
                Vector3 spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
                while (true) {
                    float distanceToPlayer = Vector3.Distance(playerPosition, spawnPosition);
                    if (distanceToPlayer > 4) break;
                    spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
                }

                newEnemy.transform.position = spawnPosition;
                newEnemy.transform.DOScale(Vector3.one, .25f).From(Vector3.zero);
            }

            for (int i = 0; i < num; i++)
                StartCoroutine(DelayedSpawn());
        }

        private void DestroyAllEnemies() {
            for (int i = _spawnedEnemies.Count - 1; i >= 0; i--) {
                var enemy = _spawnedEnemies[i];
                _spawnedEnemies.RemoveAt(i);
                if(enemy == null) continue;
                enemy.ForceKill();
            }
        }
        
        private void Awake() {
            Movement.OnPlayerDeath += DestroyAllEnemies;
            _levelManager.OnWin += DestroyAllEnemies;
        }

        private void OnDestroy() {
            Movement.OnPlayerDeath -= DestroyAllEnemies;
            _levelManager.OnWin -= DestroyAllEnemies;
        }
    }
}