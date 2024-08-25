using UnityEngine;
using Scripts.Core.Player;
using Scripts.Utils;

namespace Scripts.UI {
    public class HealthBarUI : MonoBehaviour {
        [SerializeField] private RectTransform _progressBarRT;
        [SerializeField] private float _totalProgressBarWidth;
        [SerializeField] private MaxHealthCalculation _playerMaxHealth;
        [SerializeField] private FloatReference _playerHealth;
        
        private void Update() {
            float x = Mathf.Lerp(0, _totalProgressBarWidth, _playerHealth.Value / _playerMaxHealth.GetMaxHealth());
            Vector2 newSizeDelta = new Vector2(x, _progressBarRT.sizeDelta.y);
            _progressBarRT.sizeDelta = newSizeDelta;
        }
    }
}