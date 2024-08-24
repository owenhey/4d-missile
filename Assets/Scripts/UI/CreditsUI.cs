using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Scripts.UI {
    public class CreditsUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private IntReference _credits;

        private void OnEnable() {
            _credits.OnValueChanged += Display;
            Display(_credits.Value);
        }

        private void OnDisable() {
            _credits.OnValueChanged -= Display;
        }

        private void Display(int amount) {
            _textField.text = "Credits: " + amount;
        }
    }
}