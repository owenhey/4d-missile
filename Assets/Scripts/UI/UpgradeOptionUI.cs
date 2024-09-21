using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Upgrades;
using Scripts.Misc;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI{
    public class UpgradeOptionUI : MonoBehaviour {
        [SerializeField] private CanvasGroup _cg;
        [SerializeField] private TextMeshProUGUI _optionTitleField;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _costField;
        [SerializeField] private TextMeshProUGUI _levelCountField;
        [SerializeField] private Button _buyButton;
        [SerializeField] private IntReference _playerCredits;
        [SerializeField] private GameObject _buyContent;
        [SerializeField] private GameObject _purchasedContent;
        
        [Header("Current Level")] 
        [SerializeField] private List<Image> _currentLevelSprites;
        [SerializeField] private Color _lowLevelColor;
        [SerializeField] private Color _highLevelColor;

        private UpgradeDefinition _upgradeShowing;
        public UpgradeDefinition GetUpgradeShowing() => _upgradeShowing;

        private void Awake() {
            _buyButton.onClick.AddListener(TryBuy);
            _playerCredits.OnValueChanged += RefreshBuyButton;
        }
        
        public void Setup(ShopUpgradeData upgradeData, int playerCredits) {
            _upgradeShowing = upgradeData.UpgradeDef;
            
            _optionTitleField.text = upgradeData.UpgradeDef.UpgradeName;
            _costField.text = "Credits: " + upgradeData.Cost;
            _iconImage.sprite = upgradeData.UpgradeDef.Icon;

            bool showLevelCountField = upgradeData.LevelsToUpgrade > 1;
            _levelCountField.gameObject.SetActive(showLevelCountField);
            if (showLevelCountField) {
                _levelCountField.text = $"({upgradeData.LevelsToUpgrade}) levels";
            }

            bool canPurchase = playerCredits >= upgradeData.Cost;
            _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = canPurchase ? "Buy" : "Not enough credits";
            _cg.interactable = canPurchase;

            UpdateCurrentLevelUI();
            
            ShowBuyButton();
            RefreshBuyButton(_playerCredits.Value);

        }

        private void UpdateCurrentLevelUI() {
            int baseLevel = _upgradeShowing.BaseLevel;
            int currentLevel = _upgradeShowing.LevelToUpgrade.Value;
            int maxLevel = _upgradeShowing.MaxLevel;
            float t = Helpers.RemapClamp(currentLevel, baseLevel, maxLevel, 0, 1);
            Color color = Color.Lerp(_lowLevelColor, _highLevelColor, t);
            
            _currentLevelSprites.EnsureEnoughElementsAndSetActive(currentLevel);
            foreach (var sprite in _currentLevelSprites) {
                sprite.color = color;
            }
        }

        private void RefreshBuyButton(int numCredits) {
            bool canBuy = _playerCredits.Value >= GetUpgradeShowing().Cost;
            _cg.interactable = canBuy;
        }

        private void TryBuy() {
            int playerCredits = _playerCredits.Value;
            int cost = GetUpgradeShowing().Cost;

            if (playerCredits < cost) return;
            
            _playerCredits.Add(-GetUpgradeShowing().Cost);
            GetUpgradeShowing().Upgrade(1);
            ShowPurchased();
            UpdateCurrentLevelUI();
        }

        private void ShowBuyButton() {
            _purchasedContent.SetActive(false);
            _buyContent.SetActive(true);
        }

        private void ShowPurchased() {
            _purchasedContent.SetActive(true);
            _buyContent.SetActive(false);
        }
    }
}