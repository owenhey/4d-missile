using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Player {
    [CreateAssetMenu(fileName = "PlayerAccCalculation", menuName = "ScriptableObjects/StatCalcs/PlayerAccCalc", order = 0)]
    public class PlayerAccCalculation : ScriptableObject {
        [SerializeField] private float _baseAcc = .5f;
        [SerializeField] private IntReference _playerAccLevel;
        
        // https://www.desmos.com/calculator/qsjpoos8ge
        public float GetAcceleration() {
            float x = (float)_playerAccLevel.Value;
            float exp = (x - 1) * .25f;
            float bottom = 1 + (Mathf.Exp(exp));
            float final = 2 * (1 / bottom);
            
            return _baseAcc * final;
        }

        public void Calc(int v) {
            float x = v;
            float exp = (x - 1) * .25f;
            float bottom = 1 + (Mathf.Exp(exp));
            float final = 2 * (1 / bottom);
            Debug.Log($"V: {v}, value is: {final}");
        }
    }
}