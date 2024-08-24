using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

namespace Scripts.Core.Credits {
    [RequireComponent(typeof(BoxCollider))]
    public class CreditBoxBehavior : MonoBehaviour {
        [SerializeField] private IntReference _playerCredits;
        [SerializeField] private BoxCollider _trigger;

        [SerializeField] private Transform[] _sidePanels;
        
        public int CreditAmount = 10;
        
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                GiveCreditsToPlayer();
                DestroyAnimation();
                _trigger.enabled = false;
            }
        }

        private void GiveCreditsToPlayer() {
            int randomCreditAmount = (int)(Random.Range(.75f, 1.25f) * CreditAmount);
            _playerCredits.Add(randomCreditAmount);
        }

        private void DestroyAnimation() {
            transform.DOScale(Vector3.one * 1.4f, .2f).SetEase(Ease.OutQuad).OnComplete(() => {
                transform.DOScale(Vector3.zero, .2f).SetEase(Ease.InQuad).OnComplete((() => Destroy(gameObject)));
            });
            DOTween.To(()=> _rotateSpeed, x=> _rotateSpeed = x, 2500, .4f);
        }

        private void Update() {
            RotateAnimation();
        }

        private float _rotateSpeed = 0;
        private void RotateAnimation() {
            transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
        }
    }
}
