using System;
using Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Credits {
    public class CreditSpawner : MonoBehaviour {
        [SerializeField] private CreditBoxBehavior _prefab;
        [SerializeField] private FloatReference _objectSpawnDistance;
        [SerializeField] private FloatReference _validSpawnRadius;
        [SerializeField] private IntReference _creditsThisLevel;

        private void Awake() {
            GameManager.OnGameStateChange += GameStateChangeHandler;
        }

        private void GameStateChangeHandler(GameState state) {
            if (state == GameState.Game) {
                _creditsThisLevel.SetValue(0);
            }
        }

        public void Spawn() {
            Vector3 spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
            spawnPosition.z = _objectSpawnDistance.Value;
            
            var creditBox = Instantiate(_prefab, transform);
            creditBox.transform.position = spawnPosition;
        }

        public void DestroyAllCredits() {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}