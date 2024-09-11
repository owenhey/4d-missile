using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Player {
    [CreateAssetMenu(fileName = "MaxHealthCalculation", menuName = "ScriptableObjects/StatCalcs/MaxHealthCalc", order = 0)]
    public class MaxHealthCalculation : ScriptableObject {
        [SerializeField] private float _baseMaxHealth = 50.0f;
        [SerializeField] private IntReference _maxHealthLevel;
        
        public float GetMaxHealth() {
            return _baseMaxHealth * (_maxHealthLevel.Value);
        }
    }
}