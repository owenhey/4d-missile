using System.Collections;
using UnityEngine;

namespace Scripts.Core.Enemies {
    public abstract class EnemyWeapon : MonoBehaviour {
        protected static float _harderFactor = .8f;
        
        [SerializeField] protected Enemy _baseEnemy;
        protected bool _harder => _baseEnemy.Harder;
        
        [Header("Weapon")]
        [SerializeField] private float _fireStartTime = 2;
        [SerializeField] private float _timeBetweenFires = 4;
        
        private void Start() {
            StartCoroutine(WeaponCycle());
        }

        protected virtual IEnumerator WeaponCycle() {
            float factor = _harder ? _harderFactor : 1.0f; 
            yield return new WaitForSeconds(_fireStartTime * factor);
            while (true) {
                FireWeapon();
                yield return new WaitForSeconds(_timeBetweenFires * factor);
            }
        }
        
        protected abstract void FireWeapon();
    }
}