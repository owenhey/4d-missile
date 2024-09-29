using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Core.Enemies {
    public class LaserWeapon : EnemyWeapon {
        [Header("Laser stuff")]
        [SerializeField] private Transform _laser;
        [SerializeField] private float _laserDelayTime;
        [SerializeField] private DamagePlayerCollider _damagePlayerCollider;

        public static System.Action OnLaserFire;
        
        protected override void FireWeapon() {
            StartCoroutine(FireLaser());
        }

        private IEnumerator FireLaser() {
            Vector3 playerPos = _baseEnemy.PlayerTrans.position;
            Vector3 torwardsPlayer = playerPos - transform.position;
            
            _laser.gameObject.SetActive(true);
            _laser.localScale = new Vector3(.07f, .07f, 1);
            _laser.forward = torwardsPlayer;
            _damagePlayerCollider.Enabled = false;
            _damagePlayerCollider.HasDamaged = false;
            
            float factor = _harder ? _harderFactor : 1.0f;

            yield return new WaitForSeconds(_laserDelayTime * factor);
            
            OnLaserFire?.Invoke();
            _laser.DOScale(new Vector3(.4f, .4f, 1), .1f).OnComplete(() => {
                _damagePlayerCollider.Enabled = true;
                _laser.DOScale(new Vector3(0, 0, 1), .15f).SetDelay(.1f).OnComplete(() => {
                    _damagePlayerCollider.Enabled = false;
                });
            });
        }
    }
}