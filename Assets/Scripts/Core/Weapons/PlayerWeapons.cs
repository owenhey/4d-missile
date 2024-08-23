using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Weapons {
    public class PlayerWeapons : MonoBehaviour {
        [SerializeField] private LayerMask _gamePlaneMask;
        [SerializeField] private FloatReference _validRadius;
        [SerializeField] private BasicGrenade _basicGrenade;
        [SerializeField] private Transform _weaponParent;
        
        private Camera _camera;
        
        public static Collider[] HitColliders = new Collider[8];
        
        public static System.Action<Vector3> OnBombThrow;

        private void Start() {
            _camera = Camera.main;
        }
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                ShootGrenade();
            }
        }

        private void ShootGrenade() {
            Vector3 targetPos = GetMousePos();
            var grenade = Instantiate(_basicGrenade, _weaponParent);
            grenade.Setup(transform.position, targetPos);
            
            OnBombThrow?.Invoke(targetPos);
        }

        private Vector3 GetMousePos() {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _gamePlaneMask)) {
                Vector3 directionFrom0 = Vector3.ClampMagnitude(hit.point, _validRadius.Value);
                return directionFrom0;
            }
            return Vector3.zero;
        }
    }
}