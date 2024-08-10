using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core {
    public enum GameState {
        PreGame,
        Game
    }
    public class GameManager : MonoBehaviour {
        public static Action<GameState> OnGameStateChange;
        
        private void Start() {
            OnGameStateChange?.Invoke(GameState.PreGame);
        }

        public static void ChangeGameState(GameState newState) {
            OnGameStateChange?.Invoke(newState);
        }
    }
}
