using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI{
    public class UpgradeOptionUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _optionTitleField;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _selectedImage;

        [SerializeField] private Button _button;
        public Button Button => _button;

        public void SetSelected(bool selected) {
            _selectedImage.gameObject.SetActive(selected);
        }
    }
}