using System.Collections.Generic;
using System.Linq;
using Scripts.Misc;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Core.Upgrades {
    public struct ShopUpgradeData {
        public UpgradeDefinition UpgradeDef;
        public int LevelsToUpgrade;
        public int Cost;

        public ShopUpgradeData(UpgradeDefinition upgradeDef, int levelsToUpgrade, int cost) {
            UpgradeDef = upgradeDef;
            LevelsToUpgrade = levelsToUpgrade;
            Cost = cost;
        }
    }
    
    public class ShopManager : MonoBehaviour {
        [SerializeField] private UpgradeDefinition[] _regularUpgrades;
        [SerializeField] private UpgradeDefinition[] _powerfulUpgrades;
        [SerializeField] private IntReference _playerCredits;
        
        public System.Action<ShopUpgradeData[]> OnShopRefresh;
        
        public void RefreshShop() {
            List<UpgradeDefinition> possibleUpgrades = new List<UpgradeDefinition>(_regularUpgrades);
            List<ShopUpgradeData> chosenUpgrades = new List<ShopUpgradeData>(_regularUpgrades.Length);

            // First, show 2 upgrades that are regular ones
            while (chosenUpgrades.Count < 2 && possibleUpgrades.Count > 0) {
                int randomIndex = Random.Range(0, possibleUpgrades.Count);
                
                // This is just one upgrade for the base cost
                var upgrade = possibleUpgrades[randomIndex];
                ShopUpgradeData singleUpgrade = new(upgrade, 1, upgrade.Cost);
                chosenUpgrades.Add(singleUpgrade);
                
                possibleUpgrades.RemoveAt(randomIndex);
            }
            
            // Next, either choose a powerful one, or a doubled up version of a base one.
            // If there are none left, then just give a regular one
            bool choosePowerfulOne = Random.Range(0.0f, 1.0f) < .5f;
            if (choosePowerfulOne) {
                UpgradeDefinition randomPowerful = GetRandomPowerfulUpgrade();
                if (randomPowerful == null) {
                    var upgrade = possibleUpgrades.GetRandom();
                    ShopUpgradeData singleUpgrade = new(upgrade, 1, upgrade.Cost);
                    chosenUpgrades.Add(singleUpgrade);
                }
                else {
                    // This means successfully grabbed a random powerful one
                    var singleUpgrade = new ShopUpgradeData(randomPowerful, 1, randomPowerful.Cost);
                    chosenUpgrades.Add(singleUpgrade);
                }
            }
            else {
                float percentageOff = .80f;
                int numLevelsToGive = 2;
                
                // Upgrade one of the regular ones to a double cost less one
                var chosenUpgrade = possibleUpgrades.GetRandom();
                int newPrice = (int)(chosenUpgrade.Cost * numLevelsToGive * percentageOff);
                chosenUpgrades.Add(new ShopUpgradeData(chosenUpgrade, numLevelsToGive, newPrice));
            }

            OnShopRefresh?.Invoke(chosenUpgrades.ToArray());
        }
        
        private UpgradeDefinition GetRandomPowerfulUpgrade() {
            List<UpgradeDefinition> possibleUpgrades = new List<UpgradeDefinition>(_powerfulUpgrades.Length);
            foreach (var upgrade in _powerfulUpgrades) {
                bool canShow = upgrade.LevelToUpgrade.Value == upgrade.BaseLevel;
                
                // Just hard code in this one
                if (upgrade.UpgradeName == "Grenade Count") {
                    canShow = true;
                }
                
                if (canShow) {
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