using UnityEngine;

namespace Scripts.Core.Obstacles.Hiding {
    public abstract class ObstacleHider : MonoBehaviour {
        public abstract void Hide();

        protected abstract void OnDestroy();
    }
}