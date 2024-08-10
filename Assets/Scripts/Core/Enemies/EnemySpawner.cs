using DG.Tweening;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Enemies {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private Enemy _basicEnemy;
        [SerializeField] private Transform _playerTrans;
        [SerializeField] private FloatReference _validSpawnRadius;
        
        public void Spawn() {
            Invoke("S", Random.Range(0, 1.0f));
        }

        private void S() {
            var playerPosition = _playerTrans.position;
            var newEnemy = Instantiate(_basicEnemy, transform);

            Vector3 spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
            while (true) {
                float distanceToPlayer = Vector3.Distance(playerPosition, spawnPosition);
                if (distanceToPlayer > 4) break;
                spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
            }

            newEnemy.transform.position = spawnPosition;
            newEnemy.transform.DOScale(Vector3.one, .25f).From(Vector3.zero);
        }
    }
}