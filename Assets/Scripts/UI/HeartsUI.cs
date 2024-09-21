using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Misc;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class HeartsUI : MonoBehaviour {
        [SerializeField] private IntReference _heartCount;
        [SerializeField] private List<Image> _hearts;

        private void HeartChangeHandler(int heartCount, bool animate) {
            _hearts.EnsureEnoughElements(heartCount);
            for (int i = 0; i < Mathf.Max(heartCount, _hearts.Count); i++) {
                bool activeBefore = _hearts[i].gameObject.activeInHierarchy;
                bool activeNow = i < heartCount;

                if (animate) {
                    if (!activeBefore && activeNow) {
                        var heartRT = _hearts[i].rectTransform;
                        heartRT.gameObject.SetActive(true);
                        heartRT.DOKill();
                        heartRT.DOScale(1.25f, .2f).From(0).OnComplete(() => {
                            heartRT.DOScale(1, .1f);
                        });
                    }
                    if (activeBefore && !activeNow) {
                        var heartRT = _hearts[i].rectTransform;
                        heartRT.gameObject.SetActive(true);
                        heartRT.DOScale(1.25f, .2f).OnComplete(() => {
                            heartRT.DOScale(0, .1f).OnComplete(() => {
                                heartRT.gameObject.SetActive(false);
                            });
                        });
                    }
                }
                else {
                    _hearts[i].gameObject.SetActive(activeNow);
                }
            }
        }
        
        private void HeartChangeHandler(int heartCount) {
            HeartChangeHandler(heartCount, true);
        }
        
        private void OnEnable() {
            _heartCount.OnValueChanged += HeartChangeHandler;
            HeartChangeHandler(_heartCount.Value, false);
        }

        private void OnDisable() {
            _heartCount.OnValueChanged -= HeartChangeHandler;
        }
    }
}