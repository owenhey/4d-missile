using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Level;
using Scripts.Core.Upgrades;
using UnityEngine;

namespace Scripts.Core {
    public enum GameState {
        PreGame,
        Game
    }
    public class GameManager : MonoBehaviour {
        [SerializeField] private ShopManager _shopManager;
        [SerializeField] private LevelManager _levelManager;
        
        public static Action<GameState> OnGameStateChange;

        private static GameManager _instance;

        private void OnPreGame() {
            _shopManager.RefreshShop();
        }

        private void OnGame() {
            _levelManager.PlayTestLevel();
        }
        
        private void Start() {
            _instance = this;
            ChangeGameState(GameState.PreGame);
        }
        
        private void GameStateChangeHandler(GameState newState) {
            switch (newState) {
                case GameState.PreGame:
                    OnPreGame();
                    break;
                case GameState.Game:
                    OnGame();
                    break;
            }
        }
        
        public static void ChangeGameState(GameState newState) {
            OnGameStateChange?.Invoke(newState);
            _instance.GameStateChangeHandler(newState);
        }
    }
}
