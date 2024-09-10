using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Player {
    [CreateAssetMenu(fileName = "PlayerSpeedCalculation", menuName = "ScriptableObjects/StatCalcs/PlayerSpeedCalc", order = 0)]
    public class PlayerSpeedCalculation : ScriptableObject {
        [SerializeField] private float _basePlayerSpeed = 3.5f;
        [SerializeField] private IntReference _playerSpeedLevel;
        
        public float GetSpeed() {
            return _basePlayerSpeed + _playerSpeedLevel.Value * 6.5f;
        }
    }
}