using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Credits;
using Scripts.Core.Enemies;
using Scripts.Core.Obstacles;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Level{
    public class LevelManager : MonoBehaviour {
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private CreditSpawner _creditSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private FloatReference _playerSpeed;
        [SerializeField] private GameObject _player;

        private LevelData _levelData;
        
        private float _obstacleDistanceTraveled;
        private int _obstacleIndex;
        
        private float _creditDistanceTraveled;
        private int _creditIndex;
        
        private float _totalDistance;

        public float PercentThroughLevel { get; private set; }

        public void PlayTestLevel() {
            LevelData levelData = new() {
                StartingSpeed = 10,
                EndingSpeed = 20,
                Obstacles = CreateEvenlySpacedArray(0, 40, 50),
                Credits = CreateEvenlySpacedArray(20, 80, 24)
            };
            PlayLevel(levelData);
        }
        
        public void PlayLevel(LevelData levelData) {
            _levelData = levelData;
            _totalDistance = levelData.Obstacles[^1] + _obstacleSpawner.GetObstacleSpawnDistance();
            _playerSpeed.SetValue(levelData.StartingSpeed);
            
            _obstacleDistanceTraveled = 0;
            _obstacleIndex = 0;
            
            _creditIndex = 0;
            _creditDistanceTraveled = 0;
            
            _player.SetActive(true);
        }

        private void Update() {
            _obstacleDistanceTraveled += Time.deltaTime * _playerSpeed.Value;
            _creditDistanceTraveled += Time.deltaTime * _playerSpeed.Value;
            PercentThroughLevel = _obstacleDistanceTraveled / _totalDistance;
            
            EvaluateSpeed();
            CheckForObstacleSpawn();
            CheckForCreditSpawn();
        }

        private void CheckForObstacleSpawn() {
            if (_obstacleIndex >= _levelData.Obstacles.Length) return;
            
            float nextObstacleAt = _levelData.Obstacles[_obstacleIndex];

            bool traveledEnoughForNextObstacle = _obstacleDistanceTraveled > nextObstacleAt;
            if (traveledEnoughForNextObstacle) {
                bool isFinishLine = _obstacleIndex == _levelData.Obstacles.Length - 1;
                _obstacleSpawner.Spawn(isFinishLine);
                if (_obstacleIndex % 3 == 0) {
                    _enemySpawner.Spawn();
                }

                _obstacleIndex++;
            }
        }
        
        private void CheckForCreditSpawn() {
            if (_creditIndex >= _levelData.Credits.Length) return;
            
            float nextCreditAt = _levelData.Credits[_creditIndex];

            bool spawnCredit = _creditDistanceTraveled > nextCreditAt;
            if (spawnCredit) {
                _creditSpawner.Spawn();
                _creditIndex++;
            }
        }

        private void EvaluateSpeed() {
            float newSpeed = Mathf.Lerp(_levelData.StartingSpeed, _levelData.EndingSpeed, PercentThroughLevel);
            _playerSpeed.SetValue(newSpeed);
        }

        private void SuccessfulObstaclePassHandler(bool isFinishLine) {
            if (isFinishLine) {
                AfterPassFinish();
            }
        }

        private void AfterPassFinish() {
            Debug.Log("You win!");
            _obstacleSpawner.DestroyAllObstacles();
            GameManager.ChangeGameState(GameState.PreGame);
        }

        private float[] CreateEvenlySpacedArray(float startAt, float distance, int number) {
            float[] array = new float[number];
            for (int i = 0; i < array.Length; i++) {
                array[i] = startAt + distance * i;
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