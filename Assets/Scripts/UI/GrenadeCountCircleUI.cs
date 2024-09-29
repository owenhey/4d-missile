using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.UI {
    public class GrenadeCountCircleUI : MonoBehaviour {
        [SerializeField] private float _radius;
        [SerializeField] private float _distanceAwayFromEachOther = .5f;
        [SerializeField] private Transform _center;

        [SerializeField] private IntReference _currentGrenadeCount;

        [SerializeField] private List<Transform> _whiteSpheres;
        
        private void HandleGrenadeCountChange(int count) {
            for (int i = 0; i < _whiteSpheres.Count; i++) {
                var sphere = _whiteSpheres[i];
                bool currentlyActive = sphere.gameObject.activeInHierarchy;
                bool shouldBeActive = i < count;

                if (shouldBeActive && !currentlyActive) {
                    sphere.DOScale(1.4f, .12f).From(0).OnComplete(() => {
                        sphere.DOScale(1.0f, .06f);
                    });
                }
            
                sphere.gameObject.SetActive(shouldBeActive);
            }
            

            float angleStep = _distanceAwayFromEachOther;
            float startAngle = -((count - 1) / 4.0f);
            for (int i = 0; i < count; i++) {
            
                float x = Mathf.Cos(startAngle + i * angleStep) * _radius;
                float y = Mathf.Sin(startAngle + i * angleStep) * _radius;
            
                _whiteSpheres[i].transform.localPosition = new Vector3(x, y, 0);
            }
        }
        
        private void OnEnable() {
            _currentGrenadeCount.OnValueChanged += HandleGrenadeCountChange;
            HandleGrenadeCountChange(_currentGrenadeCount.Value);
        }

        private void OnDisable() {
            _currentGrenadeCount.OnValueChanged -= HandleGrenadeCountChange;
        }
    }
}
