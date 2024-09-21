using System;
using DG.Tweening;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Weapons {
    public class PlayerWeapons : MonoBehaviour {
        [SerializeField] private LayerMask _gamePlaneMask;
        [SerializeField] private FloatReference _validRadius;
        [SerializeField] private BasicGrenade _basicGrenade;
        [SerializeField] private Transform _weaponParent;
        
        [Header("Bomb Stats")] 
        [SerializeField] private IntReference _bombLevel;
        [SerializeField] private IntReference _bombCharges;
        [SerializeField] private FloatReference _bombCooldown;

        [Header("Time Slow Stats")]
        [SerializeField] private IntReference _timeLevel;
        [SerializeField] private IntReference _timeSlowCount;
        [SerializeField] private FloatReference _timeCooldown;
        
        private Camera _camera;
        
        private bool _lookingForBombRefresh = false;
        private Tween _bombCooldownTween;
        
        private bool _lookingForTimeSlowFinish = false;
        private Tween _timeSlowTween;
        
        
        public static Collider[] HitColliders = new Collider[8];
        
        public static System.Action<Vector3> OnBombThrow;

        private void Start() {
            _camera = Camera.main;
            GameManager.OnGameStateChange += HandleGameStateChange;
        }

        private void OnEnable() {
            _bombCooldown.OnValueChanged += HandleBombCooldown;
            _timeCooldown.OnValueChanged += HandleTimeSlowDuration;
        }

        private void OnDisable() {
            _bombCooldown.OnValueChanged -= HandleBombCooldown;
            _timeCooldown.OnValueChanged -= HandleTimeSlowDuration;
        }

        private void HandleGameStateChange(GameState newState) {
            if (newState == GameState.Game) {
                // Reset the bombs
                _bombCharges.SetValue(_bombLevel.Value);
                _bombCooldown.SetValue(0);
                _lookingForBombRefresh = false;
                _bombCooldownTween?.Kill();
                
                _timeCooldown.SetValue(0);
                // Give a single time slow if the level is greater than 0
                bool hasTimeSlowAbility = _timeLevel.Value > 0;
                _timeSlowCount.SetValue(hasTimeSlowAbility ? 1 : 0);
                _timeSlowTween?.Kill();
            }
        }

        private void Update() {
            bool tryToFireGrenade = Input.GetKeyDown(KeyCode.Mouse0);
            bool canThrowGrenade = _bombCharges.Value > 0;
            if (tryToFireGrenade && canThrowGrenade) {
                ShootGrenade();
            }
            
            bool tryToTimeSlow = Input.GetKeyDown(KeyCode.T);
            bool canTimeSlow = _timeSlowCount.Value > 0;
            
            if (tryToTimeSlow && canTimeSlow) {
                TimeSlow();
            }
        }

        private void TimeSlow() {
            Time.timeScale = .5f;
            _timeSlowCount.Add(-1);
            
            _timeSlowTween?.Kill();
            _timeSlowTween = DOTween.To(() => _timeCooldown.Value, x => _timeCooldown.SetValue(x), 0, 3f).From(1)
                .SetEase(Ease.Linear).SetUpdate(true);
            _lookingForTimeSlowFinish = true;
        }
        
        private void HandleTimeSlowDuration(float value) {
            if (!_lookingForTimeSlowFinish) return;
            
            bool valueIsBasicallyZero = Mathf.Abs(value) < .001f;
            if (valueIsBasicallyZero) {
                _lookingForTimeSlowFinish = true;
                Time.timeScale = 1.0f;
            }
        }

        private void ShootGrenade() {
            Vector3 targetPos = GetMousePos();
            var grenade = Instantiate(_basicGrenade, _weaponParent);
            grenade.Setup(transform.position, targetPos);
            
            // Deal with the cooldown
            _bombCharges.Add(-1);
            StartBombCooldown();
            
            OnBombThrow?.Invoke(targetPos);
        }

        private void StartBombCooldown() {
            if (_lookingForBombRefresh) return; // This stops it if there is already a cooldown happening
            _bombCooldownTween?.Kill();
            _bombCooldownTween = DOTween.To(()=>_bombCooldown.Value, x => _bombCooldown.SetValue(x), 0, 3.5f).From(1).SetEase(Ease.Linear);
            _lookingForBombRefresh = true;
        }

        private void HandleBombCooldown(float value) {
            if (!_lookingForBombRefresh) return;
            
            bool valueIsBasicallyZero = Mathf.Abs(value) < .001f;
            if (valueIsBasicallyZero) {
                _lookingForBombRefresh = false;
                _bombCharges.Add(1);
                _bombCooldown.SetValue(0);
                
                // See if we need to restart the timer
                if (_bombCharges.Value < _bombLevel.Value) {
                    StartBombCooldown();
                }
            }
        }

        private Vector3 GetMousePos() {
            RaycastHit hit;
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, 50, _gamePlaneMask)) {
                Vector3 directionFrom0 = Vector3.ClampMagnitude(hit.point, _validRadius.Value);
                return directionFrom0;
            }
            return Vector3.zero;
        }
    }
}