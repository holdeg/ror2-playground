using RoR2;
using R2API;
using UnityEngine;
using System.Reflection;

namespace RoR2Playground.Modules
{
    // Much of the below is based on and heavily references a few public repositories for other mods or tutorial mods, hosted on 
    // GitHub - to their respective creators, thank for allowing me to use them in this project! Links:
    // * xiaoxiao921's CustomItem, which can be found at https://github.com/xiaoxiao921/CustomItem
    // * Komrade's Aetherium Item Creation tutorial - at https://github.com/derslayr10/RoR2CreateItemTemplate

    internal static class ItemHandler
    {
        internal static GameObject TestEquipPrefab;
        internal static EquipmentIndex TestEquipIndex;

        private const string ModPrefix = "@holdeg:";
        private const string PrefabPath = ModPrefix + "Assets/Import/belt/belt.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/belt_icon/8george.png";

        internal static void RegisterItems()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RoR2Playground.ror2playground"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                TestEquipPrefab = bundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
            }

            TestEquipment();

            AddLanguageTokens();
        }

        private static void TestEquipment()
        {
            var testEquipmentDef = new EquipmentDef
            {
                name = "holdegTestEquipment",
                cooldown = 5f,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "TESTEQUIPMENT_NAME",
                pickupToken = "TESTEQUIPMENT_PICKUP",
                descriptionToken = "TESTEQUIPMENT_DESC",
                loreToken = "TESTEQUIPMENT_LORE",
                canDrop = true,
                enigmaCompatible = false,
            };


            var itemDisplayRules = new ItemDisplayRule[1]; // null for no display
            itemDisplayRules[0].followerPrefab = TestEquipPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor's Chest

            var testEquip = new CustomEquipment(testEquipmentDef, itemDisplayRules);

            TestEquipIndex = ItemAPI.Add(testEquip); // ItemAPI sends back the EquipmentIndex of your equipment
        }

        private static void AddLanguageTokens()
        {
            LanguageAPI.Add("TESTEQUIPMENT_NAME", "Test Equipment");
            LanguageAPI.Add("TESTEQUIPMENT_PICKUP", "Does nothing."); // Short text that appears on pickup - no numbers
            LanguageAPI.Add("TESTEQUIPMENT_DESC", // The Description is where you put the actual numbers.
                "Instantly heal for <style=cIsHealing>0% of your maximum health</style>, gain <style=cIsUtility>0% movement speed</style>, and gain <style=cIsDamage>0% attack damage and attack speed</style> for <style=cIsUtility>0</style> <style=cStack>(+0 per stack)</style> seconds.");
            LanguageAPI.Add("TESTEQUIPMENT_LORE", // flava
                "baller.");
        }
    }
}