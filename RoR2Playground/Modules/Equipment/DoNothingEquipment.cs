using R2API;
using RoR2;
using UnityEngine;

namespace RoR2Playground.Modules.Equipment
{
    class DoNothingEquipment : EquipmentBase
    {
        public override string EquipmentName => "Blursed Portrait";

        public override string EquipmentLangTokenName => "BLURSED_PORTRAIT";

        public override string EquipmentPickupDesc => "Looks kinda funky.";

        public override string EquipmentFullDescription => "Instantly heal for <style=cIsHealing>0% of your maximum health</style>, gain <style=cIsUtility>0% movement speed</style>, and gain <style=cIsDamage>0% attack damage and attack speed</style> for <style=cIsUtility>0</style> <style=cStack>(+0 per stack)</style> seconds.";

        public override string EquipmentLore => "baller.";

        public override string EquipmentModelPath => "@RoR2Playground:Assets/Import/belt/belt.prefab";

        public override string EquipmentIconPath => "@RoR2Playground:Assets/Import/belt_icon/8george.png";

        public override bool EnigmaCompatible => base.EnigmaCompatible;

        public override bool CanDrop => base.CanDrop;

        public override float Cooldown => 5f;

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            GameObject followerPrefab = Resources.Load<GameObject>(EquipmentModelPath);
            var itemDisplay = followerPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = Utils.ItemHelpers.ItemDisplaySetup(followerPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = followerPrefab,
                    childName = "Chest",
                    localPos = new Vector3(-0.35f, -0.1f, 0f),
                    localAngles = new Vector3(0f, 180f, 0f),
                    localScale = new Vector3(0.15f, 0.15f, 0.15f),
                }
            });
            return rules;
        }

        public override void Init()
        {
            CreateItemDisplayRules();
            CreateLang();
            CreateEquipment();
        }

        protected override bool EquipmentAction(EquipmentSlot slot)
        {
            Chat.AddMessage("Tell me I'm pretty.");
            return true;
        }
    }
}
