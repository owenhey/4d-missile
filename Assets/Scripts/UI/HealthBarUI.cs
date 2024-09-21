using UnityEngine;
using Scripts.Core.Player;
using Scripts.Utils;

namespace Scripts.UI {
    public class HealthBarUI : MonoBehaviour {
        [SerializeField] private RectTransform _progressBarRT;
        [SerializeField] private IntReference _playerMaxHealthLevel;
        [SerializeField] private MaxHealthCalculation _playerMaxHealth;
        [SerializeField] private FloatReference _playerHealth;
        
        private float _totalProgressBarWidth;
        private void OnEnable() {
            MaxHealthChangeHandler(_playerMaxHealthLevel.Value);
            _playerMaxHealthLevel.OnValueChanged += MaxHealthChangeHandler;
        }

        private void OnDisable() {
            _playerMaxHealthLevel.OnValueChanged -= MaxHealthChangeHandler;
        }

        private void MaxHealthChangeHandler(int maxHealth) {
            float totalWidth = (_playerMaxHealth.GetMaxHealth() * 3);
            
            _totalProgressBarWidth = totalWidth - 10;
            var rt = transform as RectTransform;
            rt.sizeDelta = new Vector2(totalWidth, rt.sizeDelta.y);
        }
        
        private void Update() {
            float x = Mathf.Lerp(0, _totalProgressBarWidth, _playerHealth.Value / _playerMaxHealth.GetMaxHealth());
            Vector2 newSizeDelta = new Vector2(x, _progressBarRT.sizeDelta.y);
            _progressBarRT.sizeDelta = newSizeDelta;
        }
    }
}