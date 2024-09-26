using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Core.Enemies {
    public class EnemyBombWeapon : EnemyWeapon {
        [Header("Bomb stuff")]
        [SerializeField] private EnemyBomb _bombPrefab;
        [SerializeField] private float _bombTravelTime = 1.0f;
        [SerializeField] private float _bombDelayTime = .75f;
        
        
        protected override void FireWeapon() {
            Vector3 playerPos = _baseEnemy.PlayerTrans.position;
            float timeFactor = _baseEnemy.Harder ? _harderFactor : 1.0f;
            Instantiate(_bombPrefab).Setup(transform.position, playerPos, _bombTravelTime * timeFactor, _bombDelayTime * timeFactor);
        }
    }
}