using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LCCustomizations.Patches;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCustomizations
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class LCCustomizationsModBase : BaseUnityPlugin
    {
        private const string modGUID = "Thisnamework.LCCustomizations";
        private const string modName = "LCCustomizations";
        private const string modVersion = "1.0.0";
        private readonly Harmony harmony = new Harmony(modGUID);
        private static LCCustomizationsModBase Instance;
        internal static ManualLogSource log = new ManualLogSource(modGUID);

        public static ConfigEntry<bool> enableStaticTimeSpeed;
        public static ConfigEntry<bool> enableDynamicTimeSpeed;
        public static ConfigEntry<float> staticTimeSpeed;
        public static ConfigEntry<bool> disableScrapLossOnWipe;
        public static ConfigEntry<bool> enableInfiniteSprint;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            enableStaticTimeSpeed = Config.Bind<bool>("General", "enableStaticTimeSpeed", false, "Enables the static time speed modifier.");
            staticTimeSpeed = Config.Bind<float>("General", "staticTimeSpeed", 0.5f, "Enabled if the 'enableStaticTimeSpeed' is set to true. Slows the passage of time, game default is 1f.");
            enableDynamicTimeSpeed = Config.Bind<bool>("General", "enableDynamicTimeSpeed", true, "Enables the dynamic time speed modifier - depends on the number of people at the start of the level. 1 is 0.5, 2 is 0.65, 3 is 0.8 and 4 is 1.0 (default). Disabled if static is enabled.");
            disableScrapLossOnWipe = Config.Bind<bool>("General", "disableScrapLossOnWipe", true, "Disabled scrap loss on team wipe, default true.");
            enableInfiniteSprint = Config.Bind<bool>("General", "enableInfiniteSprint", false, "Enabled infinite sprint, default false.");

            //static time logs
            Logger.LogInfo("Static time speed modifier enabled:   " + enableStaticTimeSpeed.Value);
            if (enableStaticTimeSpeed.Value)
            {
                enableDynamicTimeSpeed.Value = false;
                Logger.LogInfo("Disable dynamic time speed because static is enabled.");
                Logger.LogInfo("Static time speed set to:             " + staticTimeSpeed.Value);
            }

            //dynamic time logs
            if (enableDynamicTimeSpeed.Value)
                if (!enableStaticTimeSpeed.Value)
                    Logger.LogInfo("Dynamic time speed modifier enabled:  " + enableDynamicTimeSpeed.Value);
                else
                    Logger.LogInfo("Dynamic time speed disabled because static enabled." + enableDynamicTimeSpeed.Value);


            //other logs
            Logger.LogInfo("Scrap loss disabled:                  " + disableScrapLossOnWipe.Value);
            Logger.LogInfo("Infinite sprint:                      " + enableInfiniteSprint.Value);

            ///////////////////////////////////////////

            //base patch
            harmony.PatchAll(typeof(LCCustomizationsModBase));

            //static time speed patch
            if (enableStaticTimeSpeed.Value)
                harmony.PatchAll(typeof(StaticLengthOfDayPatch));

            //dynamic time speed patch
            if (enableDynamicTimeSpeed.Value)
                harmony.PatchAll(typeof(DynamicLengthOfDayPatch));

            //infinite sprint patch
            if (enableInfiniteSprint.Value)
                harmony.PatchAll(typeof(InfiniteSprintPatch));

            //enable scrap loss patch
            if (disableScrapLossOnWipe.Value)
                harmony.PatchAll(typeof(DisableScrapLossPatch));
        }
    }
}
