using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class ProgressBarUI : MonoBehaviour {
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private RectTransform _progressBarRT;
        [SerializeField] private float _totalProgressBarWidth;
        
        private void Update() {
            float x = Mathf.Lerp(0, _totalProgressBarWidth, _levelManager.PercentThroughLevel);
            Vector2 newSizeDelta = new Vector2(x, _progressBarRT.sizeDelta.y);
            _progressBarRT.sizeDelta = newSizeDelta;
        }
    }
}