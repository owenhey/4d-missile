using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Credits {
    public class CreditSpawner : MonoBehaviour {
        [SerializeField] private CreditBoxBehavior _prefab;
        [SerializeField] private FloatReference _objectSpawnDistance;
        [SerializeField] private FloatReference _validSpawnRadius;

        public void Spawn() {
            Vector3 spawnPosition = Random.insideUnitCircle * _validSpawnRadius.Value;
            spawnPosition.z = _objectSpawnDistance.Value;
            
            var creditBox = Instantiate(_prefab);
            creditBox.transform.position = spawnPosition;
        }

        public void DestroyAllObstacles() {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}