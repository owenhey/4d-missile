using System;
using DG.Tweening;
using Scripts.Core.Level;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Player {
    public class PlayerLevelAnimation : MonoBehaviour {
        [SerializeField] private Movement _player;
        [SerializeField] private LevelManager _levelManager;
        
        [SerializeField] private float _startAnimationTime = 1.0f;
        [SerializeField] private FloatReference _endAnimationTime;
        
        [Header("Start Animation")] 
        [SerializeField] private Transform _startAnimTransStart;
        [SerializeField] private Transform _startAnimTransEnd;
        
        [Header("End Animation")] 
        [SerializeField] private Transform _endAnimTransEnd;

        private void Awake() {
            GameManager.OnGameStateChange += OnGameStateChange;
            _levelManager.OnWin += PlayEndLevel;
        }
        
        private void OnDestroy() {
            GameManager.OnGameStateChange -= OnGameStateChange;
            _levelManager.OnWin -= PlayEndLevel;
        }
        
        private void OnGameStateChange(GameState gameState) {
            if (gameState == GameState.Game) {
                PlayStartLevel();
            }
        }

        private void PlayStartLevel() {
            _player.CanMoveWithMouse = false;
            _player.transform.DOMove(_startAnimTransEnd.position, _startAnimationTime).
                From(_startAnimTransStart.position).OnComplete(() => {
                _player.CanMoveWithMouse = true;
            });
        }

        private void PlayEndLevel() {
            _player.CanMoveWithMouse = false;
            _player.transform.DOMove(_endAnimTransEnd.position, _endAnimationTime.Value).SetEase(Ease.InCubic);
        }
    }
}