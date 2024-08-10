using System.Collections;
using UnityEngine;

namespace Scripts.Core.Enemies {
    public abstract class EnemyWeapon : MonoBehaviour {
        [SerializeField] protected Enemy _baseEnemy;
        [Header("Weapon")]
        [SerializeField] private float _fireStartTime = 2;
        [SerializeField] private float _timeBetweenFires = 4;
        
        private void Start() {
            StartCoroutine(WeaponCycle());
        }

        protected virtual IEnumerator WeaponCycle() {
            yield return new WaitForSeconds(_fireStartTime);
            while (true) {
                FireWeapon();
                yield return new WaitForSeconds(_timeBetweenFires);
            }
        }
        
        protected abstract void FireWeapon();
    }
}