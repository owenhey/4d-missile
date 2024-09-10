using System;
using DG.Tweening;
using Scripts.Core.Credits;
using Scripts.Core.Enemies;
using Scripts.Core.Obstacles;
using Scripts.Core.Player;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Level{
    public class LevelManager : MonoBehaviour {
        [SerializeField] private ObstacleSpawner _obstacleSpawner;
        [SerializeField] private CreditSpawner _creditSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private FloatReference _playerSpeed;
        [SerializeField] private GameObject _player;
        [SerializeField] private IntReference _currentLevel;
        [SerializeField] private FloatReference _endAnimationTime;
        
        private LevelData _levelData;

        private SpawnAfterDistance<ObstacleSpawnable> _obstacleSpawnBehav;
        private SpawnAfterDistance<FloatSpawnable> _creditsSpawnBehav;
        private SpawnAfterDistance<FloatSpawnable> _enemiesSpawnBehav;

        private float _distanceTraveled;
        private float _totalDistance;
        private bool _died; // Keeps track of whether the player has died

        public Action OnWin;

        public float PercentThroughLevel { get; private set; }

        public void PlayCurrentLevel() {
            var levelData = LevelFactory.GenerateLevel(_currentLevel.Value);
            PlayLevel(levelData);
        }
        
        private void PlayLevel(LevelData levelData) {
            _levelData = levelData;
            _totalDistance = levelData.Obstacles[^1].Value + _obstacleSpawner.GetObstacleSpawnDistance();
            _distanceTraveled = 0;
            _playerSpeed.SetValue(levelData.StartingSpeed);
            _died = false;
            
            // Setup spawners
            _obstacleSpawnBehav = new (levelData.Obstacles, SpawnObstacle);
            _creditsSpawnBehav = new (levelData.Credits, SpawnCredits);
            _enemiesSpawnBehav = new (levelData.Enemies, SpawnEnemy);
            
            _player.SetActive(true);
        }
        
        private void Update() {
            if (_died) return;
            
            float _disThisFrame = _playerSpeed.Value * Time.deltaTime;
            
            _distanceTraveled += _disThisFrame;
            PercentThroughLevel = _distanceTraveled / _totalDistance;
            
            _obstacleSpawnBehav.Advance(_disThisFrame);
            _creditsSpawnBehav.Advance(_disThisFrame);
            _enemiesSpawnBehav.Advance(_disThisFrame);
            
            UpdateSpeed();
        }

        private void SpawnObstacle(ObstacleSpawnable spawnable) {
            _obstacleSpawner.Spawn(spawnable.IsFinishLine);
        }
        
        private void SpawnCredits(FloatSpawnable spawnable) {
            _creditSpawner.Spawn();
        }

        private void SpawnEnemy(FloatSpawnable spawnable) {
            _enemySpawner.Spawn();
        }

        private void UpdateSpeed() {
            float newSpeed = Mathf.Lerp(_levelData.StartingSpeed, _levelData.EndingSpeed, PercentThroughLevel);
            _playerSpeed.SetValue(newSpeed);
        }

        private void ObstaclePassHandler(bool isFinishLine) {
            bool isValidWin = !_died && isFinishLine;
            if (isValidWin) {
                AfterPassFinish();
            }
        }

        private void AfterPassFinish() {
            OnWin?.Invoke();
            _obstacleSpawner.DestroyAllObstacles();
            _currentLevel.Add(1);
            
            Invoke(nameof(AdvanceFromLevel), _endAnimationTime.Value);
        }

        private void AdvanceFromLevel() {
            GameManager.ChangeGameState(GameState.PreGame);
        }

        private void HandleGameStateChange(GameState state) {
            bool isLevel = state == GameState.Game;
            gameObject.SetActive(isLevel);
        }
        
        private void HandlePlayerDeath() {
            _died = true;
            DOTween.To(()=>_playerSpeed.Value, x => _playerSpeed.SetValue(x), 0, 1.0f);
        }
        
        private void Awake() {
            ObstacleBehavior.OnObstaclePass += ObstaclePassHandler;
            GameManager.OnGameStateChange += HandleGameStateChange;
            Movement.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDestroy() {
            ObstacleBehavior.OnObstaclePass -= ObstaclePassHandler;
            GameManager.OnGameStateChange -= HandleGameStateChange;
            Movement.OnPlayerDeath -= HandlePlayerDeath;
        }
    }
}