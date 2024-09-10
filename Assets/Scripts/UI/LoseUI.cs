using System;
using System.Collections.Generic;
using Scripts.Core;
using Scripts.Core.Level;
using Scripts.Core.Upgrades;
using Scripts.Misc;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class LoseUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _mainText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private IntReference _currentLevel;
        
        private void Awake() {
            _nextButton.onClick.AddListener(OnPlayAgainClick);
            _quitButton.onClick.AddListener(OnQuitClick);
            GameManager.OnGameStateChange += HandleGameStateChange;
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange -= HandleGameStateChange;
        }
        
        private void HandleGameStateChange(GameState newState) {
            bool isPreGame = newState == GameState.Lose;
            gameObject.SetActive(isPreGame);

            _mainText.text = $"You made it to level {_currentLevel.Value}";
        }

        private void OnPlayAgainClick() {
            GameManager.ResetGame();
        }
        
        private void OnQuitClick() {
            Application.Quit();
        }
    }
}