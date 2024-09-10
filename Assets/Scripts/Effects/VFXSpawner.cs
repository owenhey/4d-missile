using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Player;
using Scripts.Core.Weapons;
using UnityEngine;

namespace Scripts.Effects {
    public class VFXSpawner : MonoBehaviour {
        [SerializeField] private GameObject _bombPositionMarker;
        [SerializeField] private GameObject _bigExplosion;
        [SerializeField] private Movement _player;
        
        private void HandleBombThrow(Vector3 position) {
            Instantiate(_bombPositionMarker, transform).transform.position = position;
        }

        private void HandlePlayerDeath() {
            Vector3 playerPos = _player.transform.position;
            GameObject bigExplosion = Instantiate(_bigExplosion, transform);
            bigExplosion.transform.position = playerPos;
            Destroy(bigExplosion, 2);
        }
        
        private void OnEnable() {
            PlayerWeapons.OnBombThrow += HandleBombThrow;
            Movement.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable() {
            PlayerWeapons.OnBombThrow -= HandleBombThrow;
            Movement.OnPlayerDeath -= HandlePlayerDeath;
        }
    }
}
