using System.Collections;
using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Upgrades {
    [CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/Base", fileName = "upgrade")]
    public class UpgradeDefinition : ScriptableObject {
        public string UpgradeName;
        public new string name => UpgradeName;
        [SerializeField] private IntReference _levelToUpgrade;
        public IntReference LevelToUpgrade => _levelToUpgrade;

        public Sprite Icon;
        [TextArea(4,4)] public string Text;
        [Min(0)] public int Cost = 50;

        public void Upgrade(int levels) {
            _levelToUpgrade.Add(levels);
        }
    }
}