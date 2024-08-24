using UnityEngine;

namespace Scripts.Misc {
    public class Rotate : MonoBehaviour {
        [SerializeField] private Vector3 _rotationAmount;
        [SerializeField] private bool _randomizeDirection = false;

        private float _factor = 1;

        private void Awake() {
            if (!_randomizeDirection) return;
            _factor = Random.Range(0.0f, 1.0f) < .5f ? -1 : 1.0f;
        }
        
        private void Update() {
            transform.Rotate(_rotationAmount * (_factor * Time.deltaTime));
        }
    }
}