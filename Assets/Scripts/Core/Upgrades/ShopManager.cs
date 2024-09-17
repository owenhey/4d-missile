using System.Collections.Generic;
using System.Linq;
using Scripts.Misc;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Upgrades {
    public class ShopManager : MonoBehaviour {
        [SerializeField] private UpgradeDefinition[] _regularUpgrades;
        [SerializeField] private UpgradeDefinition[] _powerfulUpgrades;
        [SerializeField] private IntReference _playerCredits;
        
        public System.Action<UpgradeDefinition[]> OnShopRefresh;
        
        public void RefreshShop() {
            List<UpgradeDefinition> possibleUpgrades = new List<UpgradeDefinition>(_regularUpgrades);
            List<UpgradeDefinition> chosenUpgrades = new List<UpgradeDefinition>(_regularUpgrades.Length);

            // First, show 2 upgrades that are regular ones
            while (chosenUpgrades.Count < 2 && possibleUpgrades.Count > 0) {
                int randomIndex = Random.Range(0, possibleUpgrades.Count);
                chosenUpgrades.Add(possibleUpgrades[randomIndex]);
                possibleUpgrades.RemoveAt(randomIndex);
            }
            
            // Next, either choose a powerful one, or a doubled up version of a base one.
            // If there are none left, then just give a regular one
            bool choosePowerfulOne = Random.Range(0.0f, 1.0f) < .5f;
            if (choosePowerfulOne) {
                UpgradeDefinition randomPowerful = GetRandomPowerfulUpgrade();
                if (randomPowerful == null) {
                    chosenUpgrades.Add(possibleUpgrades.GetRandom());
                }
                else {
                    chosenUpgrades.Add(randomPowerful);
                }
            }
            else {
                // Upgrade one of the regular ones to a double powerful one
                // TODO: double upgrade here
                chosenUpgrades.Add(possibleUpgrades.GetRandom());
            }

            OnShopRefresh?.Invoke(chosenUpgrades.ToArray());
        }
        
        private UpgradeDefinition GetRandomPowerfulUpgrade() {
            List<UpgradeDefinition> possibleUpgrades = new List<UpgradeDefinition>(_powerfulUpgrades.Length);
            foreach (var upgrade in _powerfulUpgrades) {
                bool hasntChosenYet = upgrade.LevelToUpgrade.Value == upgrade.BaseLevel;
                if (hasntChosenYet) {
                    possibleUpgrades.Add(upgrade);
                }
            }

            return possibleUpgrades.GetRandom();
        }

        public void PurchaseUpgrade(UpgradeDefinition upgrade) {
            upgrade.Upgrade(1);
            _playerCredits.Add(-upgrade.Cost);
        }
    }
}