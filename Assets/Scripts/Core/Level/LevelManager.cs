using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Enemies;
using Scripts.Core.Obstacles;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Level{
    public class LevelManager : MonoBehaviour {
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private FloatReference _playerSpeed;
        [SerializeField] private GameObject _player;

        private LevelData _levelData;
        private float _distanceTraveled;
        private float _totalDistance;
        private int _obstacleIndex;

        public float PercentThroughLevel { get; private set; }

        public void PlayTestLevel() {
            LevelData levelData = new() {
                StartingSpeed = 10,
                EndingSpeed = 20,
                Obstacles = CreateEvenlySpacedArray(40, 50)
            };
            PlayLevel(levelData);
        }
        
        public void PlayLevel(LevelData levelData) {
            _levelData = levelData;
            _distanceTraveled = 0;
            _playerSpeed.SetValue(levelData.StartingSpeed);
            _totalDistance = levelData.Obstacles[^1] + _obstacleSpawner.GetObstacleSpawnDistance();
            _obstacleIndex = 0;
            _player.SetActive(true);
        }

        private void Update() {
            _distanceTraveled += Time.deltaTime * _playerSpeed.Value;
            PercentThroughLevel = _distanceTraveled / _totalDistance;
            
            EvaluateSpeed();
            CheckForObstacleSpawn();
        }

        private void CheckForObstacleSpawn() {
            if (_obstacleIndex >= _levelData.Obstacles.Length) return;
            
            float nextObstacleAt = _levelData.Obstacles[_obstacleIndex];

            bool traveledEnoughForNextObstacle = _distanceTraveled > nextObstacleAt;
            if (traveledEnoughForNextObstacle) {
                bool isFinishLine = _obstacleIndex == _levelData.Obstacles.Length - 1;
                _obstacleSpawner.Spawn(isFinishLine);
                if (_obstacleIndex % 3 == 0) {
                    _enemySpawner.Spawn();
                }
                _obstacleIndex++;
            }
        }
        
        private void EvaluateSpeed() {
            float newSpeed = Mathf.Lerp(_levelData.StartingSpeed, _levelData.EndingSpeed, PercentThroughLevel);
            _playerSpeed.SetValue(newSpeed);
        }

        private void SuccessfulObstaclePassHandler(bool isFinishLine) {
            if (isFinishLine) {
                Debug.Log("You win!");
                _obstacleSpawner.DestroyAllObstacles();
                GameManager.ChangeGameState(GameState.PreGame);
            }
        }

        private float[] CreateEvenlySpacedArray(float distance, int number) {
            float[] array = new float[number];
            for (int i = 0; i < array.Length; i++) {
                array[i] = distance * i;
            }

            return array;
        }

        private void HandleGameStateChange(GameState state) {
            bool isLevel = state == GameState.Game;
            gameObject.SetActive(isLevel);
        }
        
        private void Awake() {
            ObstacleBehavior.OnObstaclePass += SuccessfulObstaclePassHandler;
            GameManager.OnGameStateChange += HandleGameStateChange;
        }

        private void OnDestroy() {
            ObstacleBehavior.OnObstaclePass -= SuccessfulObstaclePassHandler;
            GameManager.OnGameStateChange += HandleGameStateChange;
        }
    }
}