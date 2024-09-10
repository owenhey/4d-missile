using UnityEngine;
using DG.Tweening;

namespace Scripts.Core.Obstacles.Hiding {
    public class BasicSlideHide : ObstacleHider {
        public const float SLIDE_TIME = .35f;
        
        [System.Serializable]
        public struct BasicSlideData {
            public Transform Target;
            public Transform NewPosition;
        }

        [SerializeField] private BasicSlideData[] _sliders;
        
        public override void Hide() {
            foreach (var slider in _sliders) {
                slider.Target.DOLocalMove(slider.NewPosition.localPosition, SLIDE_TIME).SetEase(Ease.OutQuad);
            }
        }

        protected override void OnDestroy() {
            foreach (var slider in _sliders) {
                slider.Target.DOKill();
            }
        }
    }
}