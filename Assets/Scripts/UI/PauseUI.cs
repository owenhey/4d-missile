using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class PauseUI : MonoBehaviour {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _unpauseButton;

        [SerializeField] private GameObject _content;

        private float _previousTimeScale = 1.0f;
        private bool _paused = false;

        private bool _previousVisible;
        private CursorLockMode _previousLockMode;
        
        private void Awake() {
            _restartButton.onClick.AddListener(Restart);
            _exitGameButton.onClick.AddListener(ExitGame);
            _unpauseButton.onClick.AddListener(Unpause);
            
            Unpause();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (_paused) {
                    Unpause();
                }
                else {
                    Pause();
                }
            }
        }
        
        private void Pause() {
            _previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            _content.SetActive(true);
            _paused = true;
            _previousVisible = Cursor.visible;
            _previousLockMode = Cursor.lockState;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Restart() {
            Unpause();
            GameManager.ResetGame();
        }

        private void ExitGame() {
            Application.Quit();
        }

        private void Unpause() {
            Time.timeScale = _previousTimeScale;
            _content.SetActive(false);
            _paused = false;
            Cursor.visible = _previousVisible;
            Cursor.lockState = _previousLockMode;
        }
    }
}