using Scripts.Utils;
using UnityEngine;
using Scripts.Core.Player;

namespace Scripts.Core.Enemies {
    public class Enemy : MonoBehaviour {
        [SerializeField] private float _currentHealth;
        [SerializeField] private FloatReference _validMoveRadius;
        [SerializeField] private float _moveAwayDistance = 3;
        [SerializeField] private float _moveDamp = .15f;
        [SerializeField] private float _moveRadius = 2;

        private Vector3 _moveVel;
        
        [HideInInspector] public Transform PlayerTrans;
        private Vector3 _startingPos;
        
        
        public static LayerMask EnemyMask = 1 << 7; // only enemy mask
        

        private void Awake() {
            PlayerTrans = GameObject.FindFirstObjectByType<Movement>().transform;
            
        }

        private void Start() {
            _startingPos = transform.position;
        }
        
        public virtual void Damage(float damage) {
            _currentHealth -= damage;
            if (_currentHealth <= 0) {
                Die();
            }
        }

        private void Update() {
            Vector3 directionToPlayer = transform.position - PlayerTrans.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer < _moveAwayDistance)
            {
                // Move away from player
                Vector3 moveDirection = directionToPlayer.normalized;
                Vector3 targetPosition = transform.position + moveDirection * (_moveAwayDistance - distanceToPlayer);

                // Ensure we stay within the move radius
                if (Vector3.Distance(targetPosition, _startingPos) > _moveRadius)
                {
                    targetPosition = _startingPos + (targetPosition - _startingPos).normalized * _moveRadius;
                }

                // Move towards the target position
                targetPosition = Vector3.ClampMagnitude(targetPosition, _validMoveRadius.Value);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _moveVel, _moveDamp);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, _startingPos, ref _moveVel, _moveDamp * 2.0f);
            }
        }
        
        protected virtual void Die() {
            Destroy(gameObject);
        }
    }
}