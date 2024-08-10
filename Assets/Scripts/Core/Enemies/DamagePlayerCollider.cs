using System;
using UnityEngine;
using Scripts.Core.Player;

namespace Scripts.Core.Enemies {
    public class DamagePlayerCollider : MonoBehaviour {
        public float DamageAmount = 25;
        [SerializeField] private bool _canDamageMoreThanOnce = false;
        [SerializeField] private Collider _collider;

        public bool HasDamaged = false;
        public bool Enabled {
            set {
                _collider.enabled = value;
            }
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!_canDamageMoreThanOnce && HasDamaged) return;
            if (other.CompareTag("Player")) {
                other.GetComponent<Movement>().TakeDamage(DamageAmount);
                HasDamaged = true;
            }
        }
    }
}