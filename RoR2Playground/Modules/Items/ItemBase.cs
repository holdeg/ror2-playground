using BepInEx.Configuration;
using RoR2;
using R2API;
using UnityEngine;

namespace RoR2Playground.Modules
{

    //this is our base class that all items will inherit
    public abstract class ItemBase
    {

        // sets item name
        public abstract string ItemName { get; }
        // sets lang token
        public abstract string ItemLangTokenName { get; }
        // sets the pickup description
        public abstract string ItemPickupDesc { get; }
        // sets the full description
        public abstract string ItemFullDescription { get; }
        // sets logbook lore
        public abstract string ItemLore { get; }

        // sets item tier
        public abstract ItemTier Tier { get; }
        // sets item tags. See ItemTag in a tool like dnSpy to learn more.
        public virtual ItemTag[] ItemTags { get; } = { };

        // sets paths for model and icons
        public abstract string ItemModelPath { get; }
        public abstract string ItemIconPath { get; }

        // determines whether item can be removed
        public virtual bool CanRemove { get; }
        // determines if item is hidden from the game
        public virtual bool Hidden { get; }

        // gives the item an index
        public ItemIndex Index;

        // creates necessary GameObject field for display rules
        public static GameObject ItemBodyModelPrefab;

        // initializes the item
        public abstract void Init(ConfigFile config);

        // sets the lang tokens for in game use
        protected void CreateLang()
        {
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_NAME", ItemName);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_PICKUP", ItemPickupDesc);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_DESCRIPTION", ItemFullDescription);
            LanguageAPI.Add("ITEM_" + ItemLangTokenName + "_LORE", ItemLore);
        }

        // sets display rules
        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        // actually defines the item
        protected void CreateItem()
        {
            ItemDef itemDef = new ItemDef()
            {
                name = "ITEM_" + ItemLangTokenName,
                nameToken = "ITEM_" + ItemLangTokenName + "_NAME",
                pickupToken = "ITEM_" + ItemLangTokenName + "_PICKUP",
                descriptionToken = "ITEM_" + ItemLangTokenName + "_DESCRIPTION",
                loreToken = "ITEM_" + ItemLangTokenName + "_LORE",
                pickupModelPath = ItemModelPath,
                pickupIconPath = ItemIconPath,
                hidden = Hidden,
                tags = ItemTags,
                canRemove = CanRemove,
                tier = Tier,
                
            };
            var itemDisplayRuleDict = CreateItemDisplayRules();
            Index = ItemAPI.Add(new CustomItem(itemDef, itemDisplayRuleDict));
        }

        // where hooks go
        public abstract void Hooks();

        // gets count of item from CharacterBody or CharacterMaster
        public int GetCount(CharacterBody body)
        {
            if (!body || !body.inventory)
            {
                return 0;
            }
            return body.inventory.GetItemCount(Index);
        }

        public int GetCount(CharacterMaster master)
        {
            if (!master || !master.inventory)
            {
                return 0;

            }
            return master.inventory.GetItemCount(Index);
        }

    }

}