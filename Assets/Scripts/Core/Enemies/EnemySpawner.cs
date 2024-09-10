using System;
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

        public void Spawn() {
            Invoke(nameof(S), Random.Range(0, 1.0f));
        }

        private void S() {
            var playerPosition = _playerTrans.position;
            var newEnemy = Instantiate(_possibleEnemies.GetRandom(), transform);

            Vector3 spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
            while (true) {
                float distanceToPlayer = Vector3.Distance(playerPosition, spawnPosition);
                if (distanceToPlayer > 4) break;
                spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
            }

            newEnemy.transform.position = spawnPosition;
            newEnemy.transform.DOScale(Vector3.one, .25f).From(Vector3.zero);
        }

        private void DestroyAllEnemies() {
            foreach (var enemy in _spawnedEnemies) {
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