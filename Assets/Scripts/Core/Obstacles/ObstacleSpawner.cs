using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Obstacles {
    public class ObstacleSpawner : MonoBehaviour {
        [SerializeField] private ObstacleBehavior _prefab;
        [SerializeField] private FloatReference _objectSpawnDistance;

        private void Start() {
            transform.position = new Vector3(0, 0, _objectSpawnDistance.Value);
        }
        
        public void Spawn(bool finishLine) {
            var obstacleBehavior = Instantiate(_prefab, transform, false);
            obstacleBehavior.IsFinishLine = finishLine;
        }

        public float GetObstacleSpawnDistance() {
            return _objectSpawnDistance.Value;
        }

        public void DestroyAllObstacles() {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}