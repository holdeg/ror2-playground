namespace RoR2Playground.Modules
{
    // Much of the below is based on and heavily references a few public repositories for other mods or tutorial mods, hosted on 
    // GitHub - to their respective creators, thank for allowing me to use them in this project! Links:
    // * xiaoxiao921's CustomItem, which can be found at https://github.com/xiaoxiao921/CustomItem
    // * Komrade's Aetherium Item Creation tutorial - at https://github.com/derslayr10/RoR2CreateItemTemplate

    internal static class EquipmentHandler
    {

        internal static void RegisterEquipment()
        {
            new Equipment.DoNothingEquipment().Init();
            new Equipment.ShinyPenny().Init();
        }
    }
}