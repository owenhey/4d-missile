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
        [SerializeField] private IntReference _playerLives;
        [SerializeField] private IntReference _credits;
        
        private SpawnAfterDistance<ObstacleSpawnable> _obstacleSpawnBehav;
        private SpawnAfterDistance<FloatSpawnable> _creditsSpawnBehav;
        private SpawnAfterDistance<EnemySpawnable> _enemiesSpawnBehav;

        private float _distanceTraveled;
        private float _totalDistance;
        private bool _died; // Keeps track of whether the player has died
        private float _damageTakenThisRun;
        
        public Action OnWin;
        public Action OnFlawlessWin;

        public LevelData CurrentLevelData => GetCurrentLevelData();
        private LevelData _currentLevelData;

        private LevelData GetCurrentLevelData() {
            if (_currentLevelData != null) return _currentLevelData;

            _currentLevelData = LevelFactory.GenerateLevel(_currentLevel.Value);
            return _currentLevelData;
        }

        public float PercentThroughLevel { get; private set; }
        
        public void PlayCurrentLevel() {
            if (_currentLevelData == null) {
                _currentLevelData = LevelFactory.GenerateLevel(_currentLevel.Value);
            }
            PlayLevel(_currentLevelData);
        }
        
        private void PlayLevel(LevelData levelData) {
            _currentLevelData = levelData;
            _totalDistance = levelData.Obstacles[^1].Value + _obstacleSpawner.GetObstacleSpawnDistance();
            _distanceTraveled = 0;
            _playerSpeed.SetValue(levelData.StartingSpeed);
            _died = false;
            _damageTakenThisRun = 0;
            
            _obstacleSpawner.DestroyAllObstacles();
            _creditSpawner.DestroyAllCredits();
            
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

        private void SpawnEnemy(EnemySpawnable spawnable) {
            _enemySpawner.Spawn(spawnable.NumberToSpawn);
        }

        private void UpdateSpeed() {
            float newSpeed = Mathf.Lerp(_currentLevelData.StartingSpeed, _currentLevelData.EndingSpeed, PercentThroughLevel);
            _playerSpeed.SetValue(newSpeed);
        }

        private void ObstaclePassHandler(bool isFinishLine) {
            if (_died) return;
            
            bool isValidWin = isFinishLine;
            if (isValidWin) {
                AfterPassFinish();
            }
        }

        private void AfterPassFinish() {
            OnWin?.Invoke();
            _obstacleSpawner.DestroyAllObstacles();
            _currentLevel.Add(1);
            if (_damageTakenThisRun == 0) {
                OnFlawlessWin?.Invoke();
                _credits.Add(25);
            }
            
            Invoke(nameof(AdvanceFromLevel), _endAnimationTime.Value);
        }

        private void AdvanceFromLevel() {
            GameManager.ChangeGameState(GameState.PreGame);
        }

        private void HandleGameStateChange(GameState state) {
            bool isLevel = state == GameState.Game;
            gameObject.SetActive(isLevel);

            bool isPregame = state == GameState.PreGame;
            if (isPregame) {
                _currentLevelData = LevelFactory.GenerateLevel(_currentLevel.Value);
            }
        }
        
        private void HandlePlayerDeath() {
            _playerLives.Add(-1);
            
            _died = true;
            DOTween.To(()=>_playerSpeed.Value, x => _playerSpeed.SetValue(x), 0, 1.0f);
        }
        
        private void HandleTakeDamage(float damage) {
            _damageTakenThisRun += damage;
        }
        
        private void Awake() {
            ObstacleBehavior.OnObstaclePass += ObstaclePassHandler;
            GameManager.OnGameStateChange += HandleGameStateChange;
            Movement.OnPlayerDeath += HandlePlayerDeath;
            Movement.OnTakeDamage += HandleTakeDamage;
        }

        private void OnDestroy() {
            ObstacleBehavior.OnObstaclePass -= ObstaclePassHandler;
            GameManager.OnGameStateChange -= HandleGameStateChange;
            Movement.OnPlayerDeath -= HandlePlayerDeath;
            Movement.OnTakeDamage -= HandleTakeDamage;
        }
    }
}