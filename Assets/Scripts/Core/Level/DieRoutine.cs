using System;
using System.Collections;
using Scripts.Core.Player;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Level {
    public class DieRoutine : MonoBehaviour {
        [SerializeField] private IntReference _numLivesRemaining;
        private void Awake() {
            Movement.OnPlayerDeath += StartDeathAnimation;
        }

        private void OnDestroy() {
            Movement.OnPlayerDeath -= StartDeathAnimation;
        }

        private void StartDeathAnimation() {
            StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation() {
            yield return new WaitForSeconds(2.0f);
            if (_numLivesRemaining.Value <= 0) {
                GameManager.ChangeGameState(GameState.Lose);
            }
            else {
                GameManager.ChangeGameState(GameState.PreGame);
            }
        }
    }
}