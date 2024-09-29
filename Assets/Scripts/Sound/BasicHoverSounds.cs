using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Sound {
    public class BasicHoverSounds : MonoBehaviour, IPointerEnterHandler {
        private void Awake() {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            SoundManager.PlayHighClick();
        }

        private void OnClick() {
            SoundManager.PlayRegularClick();
        }
    }
}