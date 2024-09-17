using System;
using System.Collections.Generic;
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
        [SerializeField] private ShopManager _shopManager;
        [SerializeField] private IntReference _currentLevel;
        [SerializeField] private LevelManager _levelManager;
        
        [Header("Text Section")] 
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _modifierListField;

        [Header("Upgrade Section")] 
        [SerializeField] private List<UpgradeOptionUI> _upgradeOptionUIs;
        [SerializeField] private ToggleGroup _optionsToggleGroup;
        [SerializeField] private IntReference _playerCredits;

        private LevelData _generatedLevel;
        
        private void HandleShopRefresh(UpgradeDefinition[] upgrades) {
            _upgradeOptionUIs.EnsureEnoughElementsAndSetActive(upgrades.Length);

            for (int i = 0; i < upgrades.Length; i++) {
                _upgradeOptionUIs[i].Setup(upgrades[i], _playerCredits.Value);
            }
            
            _optionsToggleGroup.SetAllTogglesOff();
        }

        private void OnEnable() {
            UpdateModifierText();
        }

        private void UpdateModifierText() {
            LevelData currentLevelData = _levelManager.CurrentLevelData;
            if (currentLevelData.Modifiers.Count == 0) {
                _modifierListField.text = "None";
                return;
            }

            if (currentLevelData.Modifiers.Count == 4) {
                _modifierListField.text = "Hell";
                return;
            }

            string text = "";
            for (var index = 0; index < currentLevelData.Modifiers.Count; index++) {
                var mod = currentLevelData.Modifiers[index];
                text += mod;
                if (index < currentLevelData.Modifiers.Count - 1) {
                    text += "<br>";
                }
            }

            _modifierListField.text = text;
        }

        private void Awake() {
            _nextButton.onClick.AddListener(OnNextClick);
            GameManager.OnGameStateChange += HandleGameStateChange;
            _shopManager.OnShopRefresh += HandleShopRefresh;
        }

        private void OnDestroy() {
            GameManager.OnGameStateChange += HandleGameStateChange;
            _shopManager.OnShopRefresh -= HandleShopRefresh;
        }
        
        private void HandleGameStateChange(GameState newState) {
            bool isPreGame = newState == GameState.PreGame;
            gameObject.SetActive(isPreGame);
            _levelText.text = $"Level {_currentLevel.Value} of 12";
        }
        
        private void OnNextClick() {
            GameManager.ChangeGameState(GameState.Game);
        }
    }
}