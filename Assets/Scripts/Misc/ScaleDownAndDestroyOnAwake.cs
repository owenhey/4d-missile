using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Misc {
    public class ScaleDownAndDestroyOnAwake : MonoBehaviour {
        public float TimeToScale = .25f;
        private void Awake() {
            transform.DOScale(Vector3.one * 1.25f, TimeToScale * .5f).From(0);
            transform.DOScale(Vector3.zero, TimeToScale).SetDelay(TimeToScale * .5f).SetEase(Ease.InQuad);
        }
    }
}