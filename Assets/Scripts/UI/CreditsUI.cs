using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Scripts.UI {
    public class CreditsUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _textField;
        [SerializeField] private IntReference _credits;

        private void OnEnable() {
            _credits.OnValueChanged += Display;
            Display(_credits.Value, false);
        }

        private void OnDisable() {
            _credits.OnValueChanged -= Display;
        }

        private void Display(int amount) {
            Display(amount, true);
        }

        private void Display(int amount, bool animate) {
            _textField.text = "Credits: " + amount;
            if (animate) {
                Animate();
            }
        }

        private void Animate() {
            _textField.transform.DOShakePosition(.2f, 15, 20);
        }
    }
}