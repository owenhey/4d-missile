using System;
using System.Collections;
using DG.Tweening;
using Scripts.Core.Player;
using Scripts.Core.Weapons;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Enemies {
    public class EnemyTimedExplode : EnemyWeapon {
        [SerializeField] private Transform _model;
        [SerializeField] private float _timeToExplode = 5.0f;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private IntReference _currentLevel;
        
        [SerializeField] private MeshRenderer _meshRenderer;
        private Material _material;
        private static readonly int T = Shader.PropertyToID("_T");

        public static Action OnExplode;
        
        private void Awake() {
            float factor = _harder ? _harderFactor : 1.0f;
            _timeToExplode *= factor;
            CopyAndAssignMaterial();
        }

        protected override void FireWeapon() {
            StartCoroutine(ExplodeTimer());
        }

        private IEnumerator ExplodeTimer() {
            float startTime = Time.time;
            float endTime = startTime + _timeToExplode;

            while (Time.time < endTime) {
                float t = (endTime - Time.time) / _timeToExplode;
                _material.SetFloat(T, t);
                yield return null;
            }

            Explode();
        }

        private void Explode() {
            _model.DOShakePosition(.4f, 1.0f, 10);
            
            OnExplode?.Invoke();
            
            // Deal the damage
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 20.0f, PlayerWeapons.HitColliders, Enemy.PlayerMask);
            for (int i = 0; i < numColliders; i++) {
                if(PlayerWeapons.HitColliders[i].TryGetComponent(out Movement m)){
                    int damage = 30 + (_currentLevel.Value - 1) * 4;
                    m.TakeDamage(30);
                }
            }
            
            _particleSystem.Play();
        }

        private void CopyAndAssignMaterial() {
            _material = new Material(_meshRenderer.material);
            _meshRenderer.material = _material;
        }
    }
}