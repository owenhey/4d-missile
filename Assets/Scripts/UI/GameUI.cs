using System;
using Scripts.Core;
using Scripts.Core.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class GameUI : MonoBehaviour {
        private void Awake() {
            GameManager.OnGameStateChange += HandleGameStateChange;
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange += HandleGameStateChange;
        }
        
        private void HandleGameStateChange(GameState newState) {
            bool isGame = newState == GameState.Game;
            gameObject.SetActive(isGame);
        }
    }
}