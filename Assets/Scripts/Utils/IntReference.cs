using System;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Utils {
    [CreateAssetMenu(fileName = "MyInt", menuName = "VariableReferences/IntReference", order = 0)]
    public class IntReference : ScriptableObject {
        [SerializeField] private int _value;
        public int Value {
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
        [SerializeField] private int _startValue;
        [NonSerialized] private bool _accessed = false;

        private void InitValue() {
            if (_useStartValue) _value = _startValue;
        }
        
        public void SetValue(int newValue) {
            _value = newValue;
            OnValueChanged?.Invoke(newValue);
        }

        public static System.Action<int> OnValueChanged;
    }
}