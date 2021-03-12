using R2API;
using RoR2;
using System;
using UnityEngine;

namespace RoR2Playground.Modules.Equipment
{
    class ShinyPenny : EquipmentBase
    {
        public override string EquipmentName => "Shiny Penny";

        public override string EquipmentLangTokenName => "SHINY_PENNY";

        public override string EquipmentPickupDesc => "Makes you especially lucky for a few seconds.";

        public override string EquipmentFullDescription => "tbd luck";

        public override string EquipmentLore => "Ooh, a penny!";

        public override string EquipmentModelPath => "Prefabs/PickupModels/PickupClover";

        public override string EquipmentIconPath => "Textures/ItemIcons/texCloverIcon";

        public override bool EnigmaCompatible => base.EnigmaCompatible;

        public override bool CanDrop => base.CanDrop;

        public override float Cooldown => 5f;

        public BuffIndex LuckBuff { get; private set; }

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
            CreateBuff();
            Hook();
            CreateEquipment();
        }

        private void CreateBuff()
        {
            CustomBuff buff = new CustomBuff(new BuffDef
            {
                buffColor = Color.magenta,
                canStack = false,
                isDebuff = false,
                name = "ShinyPennyLuck",
                iconPath = "Textures/ItemIcons/texCloverIcon"
            });
            this.LuckBuff = BuffAPI.Add(buff);
        }

        private void Hook()
        {
            On.RoR2.Util.CheckRoll_float_float_CharacterMaster += CheckRollWithPenny;
        }

        private bool CheckRollWithPenny(On.RoR2.Util.orig_CheckRoll_float_float_CharacterMaster orig, float percentChance, float luck, CharacterMaster effectOriginMaster)
        {
            float modLuck = luck;
            if (effectOriginMaster)
            {
                if (effectOriginMaster.hasBody)
                {
                    if (effectOriginMaster.GetBody().HasBuff(this.LuckBuff))
                    {
                        modLuck += 2;
                    }
                }
            }
            return orig(percentChance, modLuck, effectOriginMaster);
        }

        protected override bool EquipmentAction(EquipmentSlot slot)
        {
            slot.characterBody.AddTimedBuffAuthority(this.LuckBuff, 5);
            return true;
        }
    }
}
