using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Weapons {
    [CreateAssetMenu(fileName = "BombRadiusCalculation", menuName = "ScriptableObjects/StatCalcs/BombRadiusCalc", order = 0)]
    public class BombRadiusCalculation : ScriptableObject {
        [SerializeField] private float _baseBombRadius =  1.0f;
        [SerializeField] private IntReference _bombRadiusLevel;
        
        public float GetBombRadius() {
            return _baseBombRadius + (_bombRadiusLevel.Value - 1) * .2f;
        }
    }
}