using Unity.Mathematics;
using UnityEngine;
using Scripts.Utils;

namespace Scripts.Core.Player {
    public class PlayerRotateWithMovement : MonoBehaviour {
        private Vector3 _lastFramePosition;
        private float _xVelVel;
        private float _currentXVel;

        [SerializeField] private ParticleSystem _particleSystem;

        private void Start() {
            _lastFramePosition = transform.position;
        }

        private void Update() {
            if (Time.timeScale == 0) return;
            
            Vector3 delta = transform.position - _lastFramePosition;
            _lastFramePosition = transform.position;
            float xVelocity = delta.x / (Time.deltaTime);
            float yVelocity = delta.y / (Time.deltaTime);
            xVelocity = Mathf.Clamp(xVelocity, -20f, 20f);

            _currentXVel = Mathf.SmoothDamp(_currentXVel, xVelocity, ref _xVelVel, .1f);

            transform.eulerAngles = new Vector3(15, 0, _currentXVel * -3.5f);

            float upDownMultiplier = Helpers.RemapClamp(yVelocity, -10, 10, 0, 1.0f);
            if (yVelocity < -5) upDownMultiplier = 0;
            float leftRightMultiplier = Helpers.RemapClamp(Mathf.Abs(xVelocity), 0, 15, 0, 1.0f);
            
            ParticleSystem.EmissionModule emission = _particleSystem.emission;
            emission.rateOverDistanceMultiplier = 20 * upDownMultiplier + 20 * leftRightMultiplier;
            emission.rateOverTimeMultiplier = 50 * upDownMultiplier + 50 * leftRightMultiplier;
        }
    }
}