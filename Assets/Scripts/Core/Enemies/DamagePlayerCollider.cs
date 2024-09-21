using System;
using UnityEngine;
using Scripts.Core.Player;
using Scripts.Utils;

namespace Scripts.Core.Enemies {
    public class DamagePlayerCollider : MonoBehaviour {
        [SerializeField] private bool _canDamageMoreThanOnce = false;
        [SerializeField] private Collider _collider;
        [SerializeField] private IntReference _currentLevel;

        public bool HasDamaged = false;
        public bool Enabled {
            set {
                _collider.enabled = value;
            }
        }
        
        private void OnTriggerEnter(Collider other) {
            if (!_canDamageMoreThanOnce && HasDamaged) return;
            if (other.CompareTag("Player")) {
                int damage = 30 + (_currentLevel.Value - 1) * 4;
                other.GetComponent<Movement>().TakeDamage(damage);
                HasDamaged = true;
            }
        }
    }
}