using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Player {
    public class Movement : MonoBehaviour {
        [SerializeField] private LayerMask _gamePlaneMask;
        [SerializeField] private FloatReference _validRadius;
        [SerializeField] private Transform _mousePos;
        
        private Transform _playerTrans;
        private Camera _camera;

        private Vector3 _playerVel;

        public static System.Action<float> OnTakeDamage;

        [ReadOnly] public float Health;
        public float MaxHealth = 100;

        private void Awake() {
            _playerTrans = transform;
            _camera = Camera.main;
            Health = MaxHealth;
        }

        private void OnEnable() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnDisable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void TakeDamage(float amount) {
            Health -= amount;
            OnTakeDamage?.Invoke(amount);
            if (Health < 0) {
                Die();
            }
        }

        private void Die() {
            
        }

        private void Update() {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _gamePlaneMask)) {
                Vector3 directionFrom0 = Vector3.ClampMagnitude(hit.point, _validRadius.Value);
                float maxSpeed = Input.GetKey(KeyCode.Space) ? 1.0f : 15;
                _playerTrans.position = Vector3.SmoothDamp(_playerTrans.position, directionFrom0, ref _playerVel, .2f, maxSpeed);
                _mousePos.position = directionFrom0;
            }
        }
    }
}