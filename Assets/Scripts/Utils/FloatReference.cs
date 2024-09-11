using System;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Utils {
    [CreateAssetMenu(fileName = "MyFloat", menuName = "VariableReferences/FloatReference", order = 0)]
    public class FloatReference : ScriptableObject {
        [SerializeField] private float _value;
        public float Value {
            get {
                if (_useStartValue && !_accessed) {
                    _value = _startValue;
                    _accessed = true;
                }
                return _value;
            }
        }

        [Space(20)]
        [SerializeField] private bool _useStartValue;
        [SerializeField] private float _startValue;
        private bool _accessed = false;

        private void InitValue() {
            if (_useStartValue) _value = _startValue;
        }
        
        public void SetValue(float newValue) {
            _value = newValue;
            OnValueChanged?.Invoke(newValue);
        }

        public void Add(float delta) {
            SetValue(Value + delta);
        }

        public System.Action<float> OnValueChanged;
    }
}