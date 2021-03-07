using BepInEx;
using RoR2;

namespace holdeg
{
    [BepInDependency("com.benis.r2api")]
    [BepInPlugin("com.holdeg.ror2playground", "holdeg's RoR2 Playground", "0.1.0")]
    public class RoR2Playground : BaseUnityPlugin
    {
        public void Awake()
        {
            Logger.LogMessage("Loaded RoR2Playground.");
            On.EntityStates.Huntress.AimArrowSnipe.OnEnter += (orig, self) =>
            {
                Chat.AddMessage("AimArrowSnipe.OnEnter called.");
                orig(self);
            };

        }
    }
}
