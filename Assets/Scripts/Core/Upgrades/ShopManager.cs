using System.Collections.Generic;
using System.Linq;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Upgrades {
    public class ShopManager : MonoBehaviour {
        [SerializeField] private UpgradeDefinition[] _possibleUpgrades;
        [SerializeField] private IntReference _playerCredits;
        
        public System.Action<UpgradeDefinition[]> OnShopRefresh;
        
        public void RefreshShop() {
            int numUpgrades = Random.Range(2, 4);
            List<UpgradeDefinition> possibleUpgrades = new List<UpgradeDefinition>(_possibleUpgrades);
            List<UpgradeDefinition> chosenUpgrades = new List<UpgradeDefinition>(numUpgrades);

            while (chosenUpgrades.Count < numUpgrades && possibleUpgrades.Count > 0) {
                int randomIndex = Random.Range(0, possibleUpgrades.Count);
                chosenUpgrades.Add(possibleUpgrades[randomIndex]);
                possibleUpgrades.RemoveAt(randomIndex);
            }
            
            OnShopRefresh?.Invoke(chosenUpgrades.ToArray());
        }

        public void PurchaseUpgrade(UpgradeDefinition upgrade) {
            upgrade.Upgrade(1);
            _playerCredits.Add(-upgrade.Cost);
        }
    }
}