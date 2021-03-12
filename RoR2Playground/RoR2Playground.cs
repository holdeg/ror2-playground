using System;
using BepInEx;
using RoR2;
using R2API;
using R2API.Utils;
using System.Reflection;
using UnityEngine;

namespace RoR2Playground
{
    // This class is templated from rob's HenryMod, which can be found at https://github.com/ArcPh1r3/HenryMod. Thanks, rob!
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI), nameof(LanguageAPI), nameof(BuffAPI))]
    [BepInPlugin(MODGUID, MODNAME, MODVERSION)]
    public class PlaygroundPlugin : BaseUnityPlugin
    {
        public const string MODVERSION = "0.2.0";
        public const string MODNAME = "RoR2Playground";
        public const string MODGUID = "com.holdeg.ror2playground";

        public static PlaygroundPlugin instance;

        public static event Action AwakeEvent;
        public static event Action StartEvent;

        private void PlaygroundPlugin_Awake()
        {
            instance = this;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RoR2Playground.ror2playground"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider("@RoR2Playground", bundle);
                ResourcesAPI.AddProvider(provider);

            }
            Modules.EquipmentHandler.RegisterEquipment();
            Hook();
        }

        private void PlaygroundPlugin_Start()
        {
            // Called on splash load.
            Logger.LogMessage("Start() called.");
        }

        public PlaygroundPlugin()
        {
            AwakeEvent += PlaygroundPlugin_Awake;
            StartEvent += PlaygroundPlugin_Start;
        }

        public void Awake()
        {
            Action awake = PlaygroundPlugin.AwakeEvent;
            if (awake == null) return;
            awake();
        }

        public void Start()
        {
            Action start = PlaygroundPlugin.StartEvent;
            if (start == null) return;
            start();
        }

        private void Hook()
        {
        }
    }
}
