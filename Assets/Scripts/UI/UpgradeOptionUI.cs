using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Upgrades;
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
        [SerializeField] private Button _buyButton;
        [SerializeField] private IntReference _playerCredits;
        [SerializeField] private GameObject _buyContent;
        [SerializeField] private GameObject _purchasedContent;

        private UpgradeDefinition _upgradeShowing;
        public UpgradeDefinition GetUpgradeShowing() => _upgradeShowing;

        private void Awake() {
            _buyButton.onClick.AddListener(TryBuy);
            _playerCredits.OnValueChanged += RefreshBuyButton;
        }
        
        public void Setup(UpgradeDefinition upgradeDef, int playerCredits) {
            _upgradeShowing = upgradeDef;
            
            _optionTitleField.text = upgradeDef.UpgradeName;
            _costField.text = "Credits: " + upgradeDef.Cost;
            _iconImage.sprite = upgradeDef.Icon;

            bool canPurchase = playerCredits > upgradeDef.Cost;
            _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = canPurchase ? "Buy" : "Not enough credits";
            _cg.interactable = canPurchase;
            
            ShowBuyButton();
            RefreshBuyButton(_playerCredits.Value);

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