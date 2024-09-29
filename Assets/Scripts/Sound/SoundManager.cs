using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Core;
using Scripts.Core.Credits;
using Scripts.Core.Enemies;
using Scripts.Core.Level;
using Scripts.Core.Obstacles;
using Scripts.Core.Player;
using Scripts.Core.Upgrades;
using Scripts.Core.Weapons;
using Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Sound {
    public class SoundManager : MonoBehaviour {
        [System.Serializable]
        private struct AudioSourceData {
            public AudioSource AS;
            public float Volume;
        }
        
        private static SoundManager _instance;

        [SerializeField] private AudioSource _normalClickSound;
        [SerializeField] private AudioSource _highClickSound;
        [SerializeField] private AudioSource _creditSound;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private AudioSource _laserSound;
        [SerializeField] private AudioSource _explosion1;
        [SerializeField] private AudioSource _explosion2;
        [SerializeField] private AudioSource _explosion3;
        [SerializeField] private AudioSource _upgrade;
        [SerializeField] private AudioSource _obstacleHit;
        [SerializeField] private AudioSource _whoosh1;
        [SerializeField] private AudioSource _whoosh2;
        [SerializeField] private AudioSource _obstacleGood;
        [SerializeField] private AudioSource _timeSlow;
        [SerializeField] private AudioSource _timeSlowEnd;

        [Header("Tracks")] 
        [SerializeField] private IntReference _level;
        [SerializeField] private AudioSourceData _menuTrack;
        [SerializeField] private AudioSourceData _earlyLevelsTrack;
        [SerializeField] private AudioSourceData _midLevelsTrack;
        [SerializeField] private AudioSourceData _lateLevelsTrack;

        private AudioSource _playingTrack;
        
        private void Awake() {
            _instance = this;
            
            var allButtons = FindObjectsByType<UnityEngine.UI.Button>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var button in allButtons) {
                button.AddComponent<BasicHoverSounds>();
            }

            GameManager.OnGameStateChange += HandleGameStateChange;
            CreditBoxBehavior.OnCreditBoxCollected += PlayCoinPickUp;
            Movement.OnPlayerDeath += PlayDeathSound;
            BasicGrenade.OnGrenadeExplode += PlayExplosion1;
            EnemyBomb.OnBombExplode += PlayExplosion2;
            EnemyTimedExplode.OnExplode += PlayExplosion3;
            LaserWeapon.OnLaserFire += PlayLaser;
            UpgradeDefinition.OnUpgrade += PlayUpgrade;
            ObstacleBehavior.OnHitObstacle += PlayHitObstacle;
            PlayerWeapons.OnBombThrow += PlayWhoosh1;
            EnemyBombWeapon.OnBombThrow += PlayWhoosh2;
            ObstacleBehavior.OnObstaclePass += PlayPassObstacle;
            PlayerWeapons.OnNearbyBomb += PlayExplosion1;
            PlayerWeapons.OnTimeSlow += PlayTimeSlow;
            PlayerWeapons.OnTimeSlowEnd += PlayTimeSlowEnd;
        }

        private void HandleGameStateChange(GameState state) {
            bool isGame = state == GameState.Game;
            if (isGame) {
                Debug.Log($"Level for: {_level.Value}");
                int level = _level.Value;
                AudioSourceData track;
                float multiplier = 1.0f;
                if (level < 6) {
                    track = _earlyLevelsTrack;
                    multiplier = Helpers.RemapClamp(level, 1, 5, 1.0f, 1.3f);
                }
                else if (level < 12) {
                    track = _midLevelsTrack;
                    multiplier = Helpers.RemapClamp(level, 6, 11, 1.0f, 1.3f);
                }
                else {
                    track = _lateLevelsTrack;
                    multiplier = Helpers.RemapClamp(level, 11, 12, 1.0f, 1.25f);
                }
                SwitchToTrack(track, multiplier);
            }
            else {
                SwitchToTrack(_menuTrack, 1.0f);
            }
        }

        private void SwitchToTrack(AudioSourceData track, float speedMultipler) {
            if (track.AS == _playingTrack && track.AS.isPlaying) {
                return;
            }

            float delay = 0;
            if (_playingTrack != null && _playingTrack.isPlaying) {
                var prevTrack = _playingTrack;
                prevTrack.DOKill();
                prevTrack.DOFade(0, 2.0f).OnComplete(prevTrack.Stop);
                delay = 1.0f;
            }

            track.AS.time = 0;
            track.AS.Play();
            track.AS.DOKill();
            track.AS.DOFade(track.Volume, 2.0f).From(0).SetDelay(delay);
            track.AS.pitch = speedMultipler;
            
            _playingTrack = track.AS;
        }

        public static void PlayRegularClick() {
            _instance.PlayWithSlightlyAlteredPitch(_instance._normalClickSound);
        }

        public static void PlayHighClick() {
            _instance.PlayWithSlightlyAlteredPitch(_instance._highClickSound);
        }
        
        private void PlayCoinPickUp(int _, Vector3 __) {
            PlayWithSlightlyAlteredPitch(_creditSound);
        }

        private void PlayDeathSound() {
            PlayWithSlightlyAlteredPitch(_deathSound);
        }
        
        private void PlayLaser() {
            PlayWithSlightlyAlteredPitch(_laserSound);
        }

        private void PlayExplosion1(Vector3 _) => PlayExplosion1();
        
        private void PlayExplosion1() {
            PlayWithSlightlyAlteredPitch(_explosion1);
        }
        
        private void PlayExplosion2() {
            PlayWithSlightlyAlteredPitch(_explosion2);
        }
        
        private void PlayExplosion3() {
            PlayWithSlightlyAlteredPitch(_explosion3);
        }
        
        private void PlayUpgrade() {
            PlayWithSlightlyAlteredPitch(_upgrade);
        }
        
        private void PlayHitObstacle() {
            PlayWithSlightlyAlteredPitch(_obstacleHit);
        }
        
        private void PlayWhoosh1(Vector3 _) {
            PlayWithSlightlyAlteredPitch(_whoosh1);
        }
        
        private void PlayWhoosh2() {
            PlayWithSlightlyAlteredPitch(_whoosh2);
        }
        
        private void PlayPassObstacle(bool _) {
            PlayWithSlightlyAlteredPitch(_obstacleGood);
        }
        
        private void PlayTimeSlow() {
            PlayWithSlightlyAlteredPitch(_timeSlow);
        }
        
        private void PlayTimeSlowEnd() {
            PlayWithSlightlyAlteredPitch(_timeSlowEnd);
        }

        private void PlayWithSlightlyAlteredPitch(AudioSource source) {
            if (source.isPlaying) {
                source.time = 0;
            }

            source.pitch = Random.Range(.95f, 1.05f);
            source.Play();
        }
    }
}