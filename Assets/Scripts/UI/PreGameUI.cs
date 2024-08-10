using System;
using Scripts.Core;
using Scripts.Core.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class PreGameUI : MonoBehaviour {
        [SerializeField] private Button _button;
        [SerializeField] private LevelManager _levelManager;
        
        private void Awake() {
            _button.onClick.AddListener(OnNextClick);
            GameManager.OnGameStateChange += HandleGameStateChange;
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange += HandleGameStateChange;
        }
        
        private void HandleGameStateChange(GameState newState) {
            bool isPreGame = newState == GameState.PreGame;
            gameObject.SetActive(isPreGame);
        }

        private void OnNextClick() {
            GameManager.ChangeGameState(GameState.Game);
            _levelManager.PlayTestLevel();
        }
    }
}