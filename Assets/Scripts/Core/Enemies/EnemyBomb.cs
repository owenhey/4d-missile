using System;
using DG.Tweening;
using Scripts.Core.Player;
using Scripts.Core.Weapons;
using Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Enemies {
    public class EnemyBomb : MonoBehaviour {
        [SerializeField] private GameObject _explosion;
        [SerializeField] private GameObject _model;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _animateColor;

        [SerializeField] private IntReference _currentLevel;
        
        private Material _material;
        private float _animateMatTimerStart;
        private float _explodeDelay;

        public static Action OnBombExplode;
            
        public void Setup(Vector3 startPos, Vector3 endPos, float timeToArrive, float explodeDelay) {
            transform.position = startPos;
            _explodeDelay = explodeDelay;
            
            _model.gameObject.SetActive(true);
            _explosion.gameObject.SetActive(false);
            
            transform.Rotate(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            transform.DOMove(endPos, timeToArrive).OnComplete(ExplodeAnimation);
            
            _animateMatTimerStart = Time.time;

            CopyAndAssignMaterial();
        }

        private void ExplodeAnimation() {
            float timer = 0;
            DOTween.To(()=> timer, x=> timer = x, 0, _explodeDelay).OnComplete(Explode);
            
            _animateMatTimerStart = Time.time;
        }

        private void Update() {
            float timeElapsed = Time.time - _animateMatTimerStart;
            float percentAlong = timeElapsed / _explodeDelay;
            float sinSpeed = 10.0f * Mathf.Lerp(1.0f, 3.0f, percentAlong);

            float sinValue = Mathf.Sin(timeElapsed * sinSpeed);
            float value = (sinValue + 1) / 2.0f;
            Color color = Color.Lerp(_baseColor, _animateColor, value);
            _material.color = color;
        }

        private void Explode() {
            _model.gameObject.SetActive(false);
            _explosion.gameObject.SetActive(true);
            
            DamageInArea();
            
            OnBombExplode?.Invoke();
            
            Destroy(gameObject, 1.0f);
        }

        private void DamageInArea() {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, PlayerWeapons.HitColliders, Enemy.PlayerMask);
            for (int i = 0; i < numColliders; i++) {
                if(PlayerWeapons.HitColliders[i].TryGetComponent<Movement>(out Movement m)) {
                    int damage = 30 + (_currentLevel.Value - 1) * 4;
                    m.TakeDamage(damage);
                }
            }
        }

        private void CopyAndAssignMaterial() {
            _material = new Material(_meshRenderer.material);
            _meshRenderer.material = _material;
        }
    }
}