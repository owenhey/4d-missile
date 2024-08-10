using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core;
using UnityEngine;
using Scripts.Core.Player;

namespace Scripts.Effects{
    public class ShakeOnPlayerDamage : MonoBehaviour {
        public float ShakeAmount = .25f;
        private void OnEnable() {
            Movement.OnTakeDamage += Shake;
        }

        private void OnDisable() {
            Movement.OnTakeDamage -= Shake;
        }

        private void Shake(float damage) {
            transform.DOShakePosition(.15f, Vector3.one * ShakeAmount, 25);
        }
    }
}