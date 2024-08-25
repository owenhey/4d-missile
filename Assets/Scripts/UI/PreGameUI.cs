using System;
using System.Collections.Generic;
using Scripts.Core;
using Scripts.Core.Level;
using Scripts.Core.Upgrades;
using Scripts.Misc;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI {
    public class PreGameUI : MonoBehaviour {
        [SerializeField] private Button _nextButton;
        [SerializeField] private ShopManager _shopManager;

        [Header("Upgrade Section")] 
        [SerializeField] private List<UpgradeOptionUI> _upgradeOptionUIs;
        [SerializeField] private ToggleGroup _optionsToggleGroup;
        [SerializeField] private IntReference _playerCredits;
        
        private void HandleShopRefresh(UpgradeDefinition[] upgrades) {
            _upgradeOptionUIs.EnsureEnoughElementsAndSetActive(upgrades.Length);

            for (int i = 0; i < upgrades.Length; i++) {
                _upgradeOptionUIs[i].Setup(upgrades[i], _playerCredits.Value);
            }
            
            _optionsToggleGroup.SetAllTogglesOff();
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
        }

        private void OnNextClick() {
            TryGiveUpgrade();
            
            GameManager.ChangeGameState(GameState.Game);
        }

        private void TryGiveUpgrade() {
            // See if any of the toggles are on
            for (int i = 0; i < _upgradeOptionUIs.Count; i++) {
                var upgradeOptionUI = _upgradeOptionUIs[i];
                if (upgradeOptionUI.Toggle.isOn) {
                    _shopManager.PurchaseUpgrade(upgradeOptionUI.GetUpgradeShowing());
                    break;
                }
            }
        }
    }
}