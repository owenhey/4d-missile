using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core;
using Scripts.Core.Credits;
using Scripts.Core.Player;
using Scripts.Core.Weapons;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Effects {
    public class VFXSpawner : MonoBehaviour {
        [SerializeField] private GameObject _bombPositionMarker;
        [SerializeField] private GameObject _bigExplosion;
        [SerializeField] private GameObject _nearbyBombExplosion;
        [SerializeField] private GameObject _creditExplosion;
        [SerializeField] private Movement _player;
        [SerializeField] private Transform _cameraTransform;
        
        [Header("Dots / Lines")] 
        [SerializeField] private ParticleSystem _dotsPS;
        [SerializeField] private ParticleSystem _linesPS;
        [SerializeField] private FloatReference _playerSpeed;
        
        private void HandleBombThrow(Vector3 position) {
            Instantiate(_bombPositionMarker, transform).transform.position = position;
        }

        private void HandlePlayerDeath() {
            Vector3 playerPos = _player.transform.position;
            GameObject bigExplosion = Instantiate(_bigExplosion, transform);
            bigExplosion.transform.position = playerPos;
            Destroy(bigExplosion, 2);
            
            // Stop the dot PSs
            _dotsPS.Stop();
            _linesPS.Stop();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.G)) {
                _dotsPS.Stop();
                _linesPS.Stop();
            }
        }

        private void HandlePlayerSpeedChange(float newSpeed) {
            newSpeed = Mathf.Max(10, newSpeed);
            var velOverTime = _dotsPS.velocityOverLifetime;
            velOverTime.z = -newSpeed * .5f;

            velOverTime = _linesPS.velocityOverLifetime;
            velOverTime.z = -newSpeed;
        }
        
        private void HandleNearbyBomb(Vector3 pos) {
            GameObject bigExplosion = Instantiate(_nearbyBombExplosion, transform);
            bigExplosion.transform.position = pos;
            Destroy(bigExplosion, 2);
        }
        
        private void HandleCreditCollected(int _, Vector3 pos) {
            GameObject creditExplosion = Instantiate(_creditExplosion, transform);
            creditExplosion.transform.position = pos;
            Destroy(creditExplosion, 2);
        }

        private void HandleGameStateChange(GameState state) {
            if (state == GameState.Game) {
                _dotsPS.Play();
            }
        }

        private void HandleTakeDamage(float damage) {
            _cameraTransform.DOShakePosition(.25f, .5f, 30);
        }
        

        private void Awake() {
            GameManager.OnGameStateChange += HandleGameStateChange;
        }

        private void OnEnable() {
            PlayerWeapons.OnBombThrow += HandleBombThrow;
            Movement.OnPlayerDeath += HandlePlayerDeath;
            _playerSpeed.OnValueChanged += HandlePlayerSpeedChange;
            PlayerWeapons.OnNearbyBomb += HandleNearbyBomb;
            CreditBoxBehavior.OnCreditBoxCollected += HandleCreditCollected;
            Movement.OnTakeDamage += HandleTakeDamage;
            
            HandlePlayerSpeedChange(_playerSpeed.Value);
        }

        private void OnDisable() {
            PlayerWeapons.OnBombThrow -= HandleBombThrow;
            Movement.OnPlayerDeath -= HandlePlayerDeath;
            _playerSpeed.OnValueChanged -= HandlePlayerSpeedChange;
            PlayerWeapons.OnNearbyBomb += HandleNearbyBomb;
            Movement.OnTakeDamage -= HandleTakeDamage;
            CreditBoxBehavior.OnCreditBoxCollected += HandleCreditCollected;
        }
    }
}
