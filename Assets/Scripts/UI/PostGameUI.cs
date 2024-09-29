using System;
using System.Collections;
using DG.Tweening;
using Scripts.Core;
using Scripts.Core.Level;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class PostGameUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _levelCompleteText;
        [SerializeField] private TextMeshProUGUI _flawlessText;
        [SerializeField] private TextMeshProUGUI _flawlessPopup;
        [SerializeField] private TextMeshProUGUI _creditsEarnedText;
        [SerializeField] private TextMeshProUGUI _creditsCounterText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private IntReference _playerCredits;
        [SerializeField] private LevelManager _levelManager;

        private int targetCredits;
        
        private void OnEnable() {
            if (_levelManager.LastLevelPerformanceData != null) {
                ShowPostGameAnimation(_levelManager.LastLevelPerformanceData);
            }
        }

        private void ShowPostGameAnimation(LevelPerformanceData performance) {
            IEnumerator Anim() {
                bool flawless = performance.DamageTaken == 0;
                Color whiteButTransparent = new Color(1, 1, 1, 0);
                _flawlessText.color = whiteButTransparent;
                _levelCompleteText.color = whiteButTransparent;
                _creditsEarnedText.color = whiteButTransparent;
                _continueButton.GetComponent<CanvasGroup>().alpha = 0;
                _creditsEarnedText.text = $"Credits Earned: <color=#00000000>{performance.CreditsEarned + (flawless ? 25 : 0)}</color>";
                _creditsCounterText.text = "";
                _flawlessPopup.transform.localScale = Vector3.zero; 
                
                _flawlessText.gameObject.SetActive(flawless);
                yield return new WaitForSeconds(.5f);
                _levelCompleteText.text = $"Level {performance.Level} Complete!";
                
                // Check for victory
                if (performance.Level > 12) {
                    _levelCompleteText.text = $"YOU WIN! Keep Going?";
                }
                
                _levelCompleteText.DOFade(1, .5f).From(0);
                yield return new WaitForSeconds(.5f);
                
                _creditsEarnedText.DOFade(1, .5f).From(0);
                int x = 0;
                var creditsTween = DOTween.To(() => x, (y) => x = y, performance.CreditsEarned, 2.0f).SetEase(Ease.OutCubic).OnUpdate(() => {
                    _creditsCounterText.text = $"{x}";
                });

                if (flawless) {
                    yield return new WaitForSeconds(.5f);
                    _flawlessText.DOFade(1, .5f).From(0);
                    _flawlessPopup.rectTransform.DOScale(1.3f, .2f).SetDelay(1.5f).From(0).OnComplete(() => {
                        _flawlessPopup.rectTransform.DOScale(1f, .1f).OnComplete(() => {
                            _playerCredits.Add(25);
                            creditsTween.Kill();
                            DOTween.To(() => x, (y) => x = y, performance.CreditsEarned + 25, 1.0f).SetEase(Ease.OutCubic).OnUpdate(() => {
                                _creditsCounterText.text = $"{x}";
                            });
                        });
                    });
                }

                yield return new WaitForSeconds(.5f);
                _continueButton.GetComponent<CanvasGroup>().DOFade(1.0f, .5f).From(0);
            }

            StartCoroutine(Anim());
        }
        
        private void Continue() {
            GameManager.ChangeGameState(GameState.PreGame);
        }

        private void HandleGameStateChange(GameState state) {
            bool isPostGame = state == GameState.PostGame;
            gameObject.SetActive(isPostGame);
        }
        
        private void Awake() {
            GameManager.OnGameStateChange += HandleGameStateChange;
            _continueButton.onClick.AddListener(Continue);
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange -= HandleGameStateChange;
            _continueButton.onClick.AddListener(Continue);
        }
    }
}