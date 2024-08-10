using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Misc{
    public class SinBob : MonoBehaviour {
        public Vector3 Amp;
        public float Freq = 1;

        private Vector3 _negAmp;
        private Vector3 _startPos;
        private Transform _trans;
        private float _randomOffset;
        private float _randomFreq;
        
        private void Start() {
            _trans = transform;
            if (_trans.parent == null) {
                _startPos = _trans.position;
            }
            else {
                _startPos = _trans.localPosition;
            }

            _randomOffset = Random.Range(0.0f, 100.0f);
            _randomFreq = Freq * Random.Range(.9f, 1.1f);
            _negAmp = Amp * -1;
        }

        private void Update() {
            if (_trans.parent == null) {
                _trans.position = _startPos + Vector3.Lerp(_negAmp, Amp,
                    Mathf.Sin(_randomOffset + Time.time + (_randomFreq * Time.time)));
            }
            else {
                _trans.localPosition = _startPos + Vector3.Lerp(_negAmp, Amp,
                    .5f * (1 + Mathf.Sin(_randomOffset + Time.time + (_randomFreq * Time.time))));
            }
        }
    }
}