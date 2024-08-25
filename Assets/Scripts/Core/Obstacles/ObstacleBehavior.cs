using System;
using Scripts.Core.Obstacles.Hiding;
using Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;
using Scripts.Core.Player;

namespace Scripts.Core.Obstacles {
    public class ObstacleBehavior : MonoBehaviour {
        private Transform _trans;

        [SerializeField] private GameObject[] _colliderOptions;

        private ObstacleHider[] _hiders;

        [SerializeField] private bool _hitPlayer = false;

        [SerializeField] private FloatReference _obstacleRotationSpeed;

        public static Action<bool> OnObstaclePass;

        public bool IsFinishLine;

        private int _randomRotateDirection;
        private Movement _playerMovement;
        
        private void Awake() {
            _trans = transform;
            _playerMovement = GameObject.FindFirstObjectByType<Movement>();
            _randomRotateDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        private void Start() {
            int ranChosen = Random.Range(0, _colliderOptions.Length);
            for (int i = 0; i < _colliderOptions.Length; i++) {
                _colliderOptions[i].gameObject.SetActive(i == ranChosen);
            }

            _hiders = _trans.GetComponentsInChildren<ObstacleHider>();
            
            // randomize rotation
            _trans.Rotate(0, 0, Random.Range(0, 1000));
        }
        
        public void Update() {
            _trans.Rotate(0, 0, _obstacleRotationSpeed.Value * Time.deltaTime * _randomRotateDirection);
        }

        public void CollideWithPlayer() {
            _hitPlayer = true;
            _playerMovement.TakeDamage(20);
        }

        public void OnPassedPlayer() {
            OnObstaclePass?.Invoke(IsFinishLine);
            if (_hitPlayer) return;
            if (_hiders == null) return;
            
            foreach (var hider in _hiders) {
                hider.Hide();
            }
        }
    }
}