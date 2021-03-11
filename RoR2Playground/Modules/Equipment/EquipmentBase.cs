using BepInEx.Configuration;
using RoR2;
using R2API;
using UnityEngine;

namespace RoR2Playground.Modules
{

    //this is our base class that all equipments will inherit
    public abstract class EquipmentBase
    {

        // sets equipment name
        public abstract string EquipmentName { get; }
        // sets lang token
        public abstract string EquipmentLangTokenName { get; }
        // sets the pickup description
        public abstract string EquipmentPickupDesc { get; }
        // sets the full description
        public abstract string EquipmentFullDescription { get; }
        // sets logbook lore
        public abstract string EquipmentLore { get; }

        // sets paths for model and icons
        public abstract string EquipmentModelPath { get; }
        public abstract string EquipmentIconPath { get; }

        // controls whether the equipment is compatible with enigma
        public virtual bool EnigmaCompatible { get; } = true;

        // controls whether the equipment drops naturally
        public virtual bool CanDrop { get; } = true;

        // equipment cooldown on use
        public virtual float Cooldown { get; } = 45f;

        // gives the equipment an index
        public EquipmentIndex Index;

        // creates necessary GameObject field for display rules
        public static GameObject EquipmentBodyModelPrefab;

        // initializes the equipment
        public abstract void Init();

        // sets the lang tokens for in game use
        protected void CreateLang()
        {
            LanguageAPI.Add("EQUIP_" + EquipmentLangTokenName + "_NAME", EquipmentName);
            LanguageAPI.Add("EQUIP_" + EquipmentLangTokenName + "_PICKUP", EquipmentPickupDesc);
            LanguageAPI.Add("EQUIP_" + EquipmentLangTokenName + "_DESCRIPTION", EquipmentFullDescription);
            LanguageAPI.Add("EQUIP_" + EquipmentLangTokenName + "_LORE", EquipmentLore);
        }

        // sets display rules
        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        // actually defines the equipment
        protected void CreateEquipment()
        {
            EquipmentDef equipmentDef = new EquipmentDef()
            {
                name = "EQUIP_" + EquipmentLangTokenName,
                nameToken = "EQUIP_" + EquipmentLangTokenName + "_NAME",
                pickupToken = "EQUIP_" + EquipmentLangTokenName + "_PICKUP",
                descriptionToken = "EQUIP_" + EquipmentLangTokenName + "_DESCRIPTION",
                loreToken = "EQUIP_" + EquipmentLangTokenName + "_LORE",
                pickupModelPath = EquipmentModelPath,
                pickupIconPath = EquipmentIconPath,
                enigmaCompatible = EnigmaCompatible,
                canDrop = CanDrop,
                cooldown = Cooldown,
            };

            var equipmentDisplayRuleDict = CreateItemDisplayRules();
            this.Index = ItemAPI.Add(new CustomEquipment(equipmentDef, equipmentDisplayRuleDict));
            On.RoR2.EquipmentSlot.PerformEquipmentAction += this.PerformEquipmentAction;
        }

        private bool PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentIndex equipIndex)
        {
            bool equipmentIsThis = equipIndex == this.Index;
            bool output;
            if (equipmentIsThis)
            {
                output = this.EquipmentAction(self);
            }
            else
            {
                output = orig(self, equipIndex);
            }
            return output;
        }

        protected abstract bool EquipmentAction(EquipmentSlot slot);
    }

}