using System;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Utils {
    [CreateAssetMenu(fileName = "MyFloat", menuName = "VariableReferences/FloatReference", order = 0)]
    public class FloatReference : ScriptableObject {
        [SerializeField] private float _value;
        public float Value => _value;

        public void SetValue(float newValue) {
            _value = newValue;
            OnValueChanged?.Invoke(newValue);
        }

        public static System.Action<float> OnValueChanged;
    }
}