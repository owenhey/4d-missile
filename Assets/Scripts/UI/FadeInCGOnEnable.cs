using DG.Tweening;
using UnityEngine;

namespace Scripts.UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInCGOnEnable : MonoBehaviour {
        public float TargetA = 1.0f;

        private void OnEnable() {
            var cg = GetComponent<CanvasGroup>();
            cg.DOFade(TargetA, .5f).From(0).SetUpdate(true);
        }
    }
}