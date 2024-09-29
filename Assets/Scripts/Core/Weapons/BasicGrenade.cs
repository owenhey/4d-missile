using System;
using DG.Tweening;
using Scripts.Core.Enemies;
using Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Weapons {
    public class BasicGrenade : MonoBehaviour {
        [SerializeField] private GameObject _explosion;
        [SerializeField] private GameObject _model;
        [SerializeField] private float _timeToGetThere;
        [SerializeField] private IntReference _grenadeRadiusLevel;
        [SerializeField] private ParticleSystem _explosionPS;
        
        private Vector3 _startPos;
        private Vector3 _endPos;
        private float _elapsedTime;
        private Vector3 _initialVelocity;
        private bool _fired = false;

        public static Action OnGrenadeExplode;

        public void Setup(Vector3 startPos, Vector3 endPos) {
            _startPos = startPos;
            _endPos = endPos;
            transform.position = startPos;
            _elapsedTime = 0f;
            
            _model.gameObject.SetActive(true);
            _explosion.gameObject.SetActive(false);
            
            transform.Rotate(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));

            // Calculate initial velocity
            Vector3 displacement = _endPos - _startPos;
            float gravity = Physics.gravity.magnitude;
            _initialVelocity = new Vector3(
                displacement.x / _timeToGetThere,
                (displacement.y / _timeToGetThere) + (0.5f * gravity * _timeToGetThere),
                displacement.z / _timeToGetThere
            );

            var mainPS = _explosionPS.main;
            mainPS.startSpeedMultiplier = GetGrenadeRadius();

            var dampenPS = _explosionPS.limitVelocityOverLifetime;
            dampenPS.limit = 2 * GetGrenadeRadius();
        }

        private void Update() {
            if (_elapsedTime < _timeToGetThere) {
                _elapsedTime += Time.deltaTime;
                float t = _elapsedTime / _timeToGetThere;

                // Calculate position using projectile motion equation
                Vector3 newPosition = _startPos + _initialVelocity * _elapsedTime;
                newPosition.y += 0.5f * Physics.gravity.y * _elapsedTime * _elapsedTime;

                transform.position = newPosition;
            }
            else {
                if (!_fired) {
                    Explode();
                    _fired = true;
                }
            }
        }

        private void Explode() {
            _model.gameObject.SetActive(false);
            _explosion.gameObject.SetActive(true);
            
            OnGrenadeExplode?.Invoke();
            
            DamageInArea();
            
            Destroy(gameObject, 1.0f);
        }

        private void DamageInArea() {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, GetGrenadeRadius(), PlayerWeapons.HitColliders, Enemy.EnemyMask);
            for (int i = 0; i < numColliders; i++) {
                if(PlayerWeapons.HitColliders[i].TryGetComponent<Enemy>(out Enemy e)){
                    e.Damage(100);
                }
            }
        }

        private float GetGrenadeRadius() {
            return 1.0f + (_grenadeRadiusLevel.Value - 1) * .666f;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos() {
            Gizmos.color = new Color(1, 1, 1, .25f);
            Gizmos.DrawSphere(transform.position, GetGrenadeRadius());
        }
#endif
    }
}