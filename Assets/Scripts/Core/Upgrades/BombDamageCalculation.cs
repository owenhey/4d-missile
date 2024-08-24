using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Weapons {
    [CreateAssetMenu(fileName = "BombDamageCalculation", menuName = "ScriptableObjects/StatCalcs/BombDamageCalc", order = 0)]
    public class BombDamageCalculation : ScriptableObject {
        [SerializeField] private float _baseBombDamage = 10.0f;
        [SerializeField] private IntReference _bombDamageLevel;
        
        public float GetBombDamage() {
            return _baseBombDamage + (_bombDamageLevel.Value - 1) * 5.0f;
        }
    }
}