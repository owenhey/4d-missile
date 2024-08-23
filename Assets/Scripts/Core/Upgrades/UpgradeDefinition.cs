using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Upgrades {
    [CreateAssetMenu(menuName = "ScriptableObjects/Upgrades/Base", fileName = "upgrade")]
    public class UpgradeDefinition : ScriptableObject {
        public string UpgradeName;
        public new string name => UpgradeName;

        public Sprite Icon;
        [TextArea(4,4)] public string Text;
        [Min(0)] public int Cost = 50;
    }
}