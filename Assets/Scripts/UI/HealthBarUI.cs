using UnityEngine;
using Scripts.Core.Player;

namespace Scripts.UI {
    public class HealthBarUI : MonoBehaviour {
        [SerializeField] private Movement _movement;
        [SerializeField] private RectTransform _progressBarRT;
        [SerializeField] private float _totalProgressBarWidth;
        
        private void Update() {
            float x = Mathf.Lerp(0, _totalProgressBarWidth, _movement.Health / _movement.MaxHealth);
            Vector2 newSizeDelta = new Vector2(x, _progressBarRT.sizeDelta.y);
            _progressBarRT.sizeDelta = newSizeDelta;
        }
    }
}