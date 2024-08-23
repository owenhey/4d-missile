using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core.Weapons;
using UnityEngine;

namespace Scripts.Effects {
    public class VFXSpawner : MonoBehaviour {
        [SerializeField] private GameObject _bombPositionMarker;
        
        private void HandleBombThrow(Vector3 position) {
            Instantiate(_bombPositionMarker, transform).transform.position = position;
        }
        
        private void OnEnable() {
            PlayerWeapons.OnBombThrow += HandleBombThrow;
        }

        private void OnDisable() {
            PlayerWeapons.OnBombThrow += HandleBombThrow;
        }
    }
}
