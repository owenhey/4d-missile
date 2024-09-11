using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class TimeSlowUI : MonoBehaviour {
        [SerializeField] private GameObject _content;
        
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _remainingTimeImage;
        [SerializeField] private IntReference _timeSlowLevel;
        [SerializeField] private IntReference _timeSlowsRemaining;
        [SerializeField] private FloatReference _timeSlowDuration;
        
        private void OnEnable() {
            bool hasTimeSlow = _timeSlowLevel.Value > 0;
            _content.SetActive(hasTimeSlow);
            if (!hasTimeSlow) return;
            
            _timeSlowDuration.OnValueChanged += HandleTimeSlowDurationChange;
            _timeSlowsRemaining.OnValueChanged += HandleTimeSlowsRemainingChange;
            
            HandleTimeSlowsRemainingChange(_timeSlowLevel.Value);
            HandleTimeSlowDurationChange(_timeSlowDuration.Value);
        }
        
        private void OnDisable() {
            _timeSlowDuration.OnValueChanged -= HandleTimeSlowDurationChange;
            _timeSlowsRemaining.OnValueChanged -= HandleTimeSlowsRemainingChange;
        }

        private void HandleTimeSlowsRemainingChange(int newCount) {
            if (newCount > 0) {
                _text.text = "Time Slow (T): Ready";
            }
            else {
                _text.text = "Time Slow (T): Used";
            }
        }
        
        private void HandleTimeSlowDurationChange(float cooldownRemaining) {
            _remainingTimeImage.fillAmount = cooldownRemaining;
        }
    }
}