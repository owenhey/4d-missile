using System;
using System.Collections;
using Scripts.Core.Player;
using UnityEngine;

namespace Scripts.Core.Level {
    public class DieRoutine : MonoBehaviour {
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
            GameManager.ChangeGameState(GameState.Lose);
        }
    }
}