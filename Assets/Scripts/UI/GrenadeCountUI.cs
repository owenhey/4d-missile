using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class GrenadeCountUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private IntReference _grenadeCount;
        [SerializeField] private FloatReference _grenadeCooldown;

        private void OnEnable() {
            _grenadeCount.OnValueChanged += HandleGrenadeCountChange;
            _grenadeCooldown.OnValueChanged += HandleGrenadeCooldownChange;
            
            HandleGrenadeCountChange(_grenadeCount.Value);
            HandleGrenadeCooldownChange(_grenadeCooldown.Value);
        }
        
        private void OnDisable() {
            _grenadeCount.OnValueChanged -= HandleGrenadeCountChange;
            _grenadeCooldown.OnValueChanged -= HandleGrenadeCooldownChange;
        }

        private void HandleGrenadeCountChange(int newCount) {
            _text.text = $"Grenades: {newCount}";
        }
        
        private void HandleGrenadeCooldownChange(float cooldownRemaining) {
            _cooldownImage.fillAmount = 1 - cooldownRemaining;
        }
    }
}