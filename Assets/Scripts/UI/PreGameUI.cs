using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core;
using Scripts.Core.Level;
using Scripts.Core.Upgrades;
using Scripts.Misc;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class PreGameUI : MonoBehaviour {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _refreshShopButton;
        [SerializeField] private ShopManager _shopManager;
        [SerializeField] private IntReference _currentLevel;
        [SerializeField] private LevelManager _levelManager;
        
        [Header("Text Section")] 
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private ModifierUIData _modifierTitleField;
        [SerializeField] private ModifierUIData[] _modifierList;

        [Header("Upgrade Section")] 
        [SerializeField] private List<UpgradeOptionUI> _upgradeOptionUIs;
        [SerializeField] private ToggleGroup _optionsToggleGroup;
        [SerializeField] private IntReference _playerCredits;
            
        private LevelData _generatedLevel;
        
        private void HandleShopRefresh(ShopUpgradeData[] upgrades) {
            _upgradeOptionUIs.EnsureEnoughElementsAndSetActive(upgrades.Length);

            for (int i = 0; i < upgrades.Length; i++) {
                _upgradeOptionUIs[i].Setup(upgrades[i], _playerCredits.Value);
            }
            
            _optionsToggleGroup.SetAllTogglesOff();
        }

        private void OnEnable() {
            UpdateModifierText();
            CreditsChangeHandler(_playerCredits.Value);
        }

        private void UpdateModifierText() {
            _modifierTitleField.Text.DOFade(1.0f, .3f).From(0).SetDelay(.75f);
            _modifierTitleField.Parent.transform.DOScale(Vector3.one * 1.25f, .15f).SetDelay(.75f).OnComplete(() => {
                _modifierTitleField.Parent.transform.DOScale(Vector3.one, .15f);
            });
            
            LevelData currentLevelData = _levelManager.CurrentLevelData;
            if (currentLevelData.Modifiers.Count == 0) {
                _modifierList[0].Text.text = "None";
                _modifierList[0].Text.color = Color.white;
                _modifierList[0].Parent.gameObject.SetActive(true);
                _modifierList[0].Text.DOFade(1.0f, .3f).From(0).SetDelay(1.25f);
                _modifierList[0].Parent.transform.DOScale(Vector3.one * 1.25f, .15f).SetDelay(1.25f).OnComplete(() => {
                    _modifierList[0].Parent.transform.DOScale(Vector3.one, .15f);
                });

                for (int i = 1; i < _modifierList.Length; i++) {
                    _modifierList[i].Parent.gameObject.SetActive(false);
                }
                LayoutRebuildModifierList();
                return;
            }

            if (currentLevelData.Modifiers.Count == 4) {
                _modifierList[0].Text.text = "Hell";
                _modifierList[0].Text.color = new Color(.75f, 0, 0, 1);
                _modifierList[0].Parent.gameObject.SetActive(true);
                _modifierList[0].Text.DOFade(1.0f, .3f).From(0).SetDelay(1.25f);
                _modifierList[0].Parent.transform.DOScale(Vector3.one * 1.75f, .2f).SetDelay(1.25f).OnComplete(() => {
                    _modifierList[0].Parent.transform.DOScale(Vector3.one, .2f);
                });

                for (int i = 1; i < _modifierList.Length; i++) {
                    _modifierList[i].Parent.gameObject.SetActive(false);
                }
                LayoutRebuildModifierList();
                return;
            }

            for (int i = 0; i < _modifierList.Length; i++) {
                bool active = i < currentLevelData.Modifiers.Count;
                _modifierList[i].Parent.gameObject.SetActive(active);
                if (!active) continue;

                _modifierList[i].Text.text = currentLevelData.Modifiers[i];

                int x = i;
                _modifierList[x].Text.color = Color.white;
                _modifierList[x].Text.DOFade(1.0f, .3f).From(0).SetDelay((x + 6) * .25f);
                _modifierList[x].Parent.transform.DOScale(Vector3.one * 1.25f, .15f).SetDelay((x + 6) * .25f).OnComplete(() => {
                    _modifierList[x].Parent.transform.DOScale(Vector3.one, .15f);
                });
            }

            LayoutRebuildModifierList();
        }

        private void LayoutRebuildModifierList() {
            foreach (var modItem in _modifierList) {
                LayoutRebuilder.ForceRebuildLayoutImmediate(modItem.Text.transform as RectTransform);
                LayoutRebuilder.ForceRebuildLayoutImmediate(modItem.Parent);
            }
        }

        private void OnShopRefreshClick() {
            _playerCredits.Add(-15);
            _shopManager.RefreshShop();
        }

        private void CreditsChangeHandler(int value) {
            _refreshShopButton.interactable = value >= 20;
        }

        private void Awake() {
            _nextButton.onClick.AddListener(OnNextClick);
            _refreshShopButton.onClick.AddListener(OnShopRefreshClick);
            GameManager.OnGameStateChange += HandleGameStateChange;
            _shopManager.OnShopRefresh += HandleShopRefresh;
            _playerCredits.OnValueChanged += CreditsChangeHandler;
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange -= HandleGameStateChange;
            _shopManager.OnShopRefresh -= HandleShopRefresh;
            _playerCredits.OnValueChanged -= CreditsChangeHandler;
        }
        
        private void HandleGameStateChange(GameState newState) {
            bool isPreGame = newState == GameState.PreGame;
            gameObject.SetActive(isPreGame);
            _levelText.text = $"Level {_currentLevel.Value} of 12";
        }
        
        private void OnNextClick() {
            GameManager.ChangeGameState(GameState.Game);
        }
        
        [System.Serializable] 
        private struct ModifierUIData {
            public TextMeshProUGUI Text;
            public RectTransform Parent;
        }
    }
}