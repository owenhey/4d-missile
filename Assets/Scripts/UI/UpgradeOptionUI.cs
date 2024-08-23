using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI{
    public class UpgradeOptionUI : MonoBehaviour {
        [SerializeField] private PreGameUI _parentBehavior;
        [SerializeField] private TextMeshProUGUI _optionTitleField;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _costField;
        [SerializeField] private Image _selectedImage;

        [SerializeField] private Toggle _toggle;
        public Toggle Toggle => _toggle;

        public void Setup(UpgradeDefinition upgradeDef) {
            _optionTitleField.text = upgradeDef.UpgradeName;
            _costField.text = "Credits: " + upgradeDef.Cost;
            _iconImage.sprite = upgradeDef.Icon;
        }
        
        public void ShowSelected(bool selected) {
            _selectedImage.gameObject.SetActive(selected);
            _parentBehavior.OnOptionClick();
        }
    }
}