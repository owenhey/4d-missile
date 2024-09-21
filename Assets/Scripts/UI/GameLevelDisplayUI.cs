using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Scripts.UI {
    public class GameLevelDisplayUI : MonoBehaviour {
        [SerializeField] private IntReference _level;
        [SerializeField] private TextMeshProUGUI _tmpro;

        private void OnEnable() {
            LevelChangeHandler(_level.Value);
            _level.OnValueChanged += LevelChangeHandler;
        }

        private void OnDisable() {
            _level.OnValueChanged -= LevelChangeHandler;
        }

        private void LevelChangeHandler(int level) {
            _tmpro.text = $"Level {level}";
        }
    }
}
