using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class DashUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private IntReference _dashLevel;
        [SerializeField] private FloatReference _dashCooldown;

        [SerializeField] private GameObject _content;
        
        private void OnEnable() {
            bool hasDash = _dashLevel.Value > 0;
            _content.SetActive(hasDash);
            if (!hasDash) return;
            
            _dashCooldown.OnValueChanged += HandleDashCooldownChange;
            
            HandleDashCooldownChange(_dashCooldown.Value);
        }
        
        private void OnDisable() {
            _dashCooldown.OnValueChanged -= HandleDashCooldownChange;
        }

        private void HandleDashCooldownChange(float cooldownRemaining) {
            _cooldownImage.fillAmount = 1 - cooldownRemaining;
        }
    }
}