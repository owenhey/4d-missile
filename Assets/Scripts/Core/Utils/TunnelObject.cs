using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Utils {
    public class TunnelObject : MonoBehaviour {
        [SerializeField] private FloatReference _tunnelSpeed;
        private Transform _trans;
        
        private void Awake() {
            _trans = transform;
        }
        
        public void Update() {
            _trans.position += new Vector3(0, 0, -_tunnelSpeed.Value * Time.deltaTime);
            if (_trans.position.z < -10) {
                Destroy(gameObject);
            }
        }
    }
}