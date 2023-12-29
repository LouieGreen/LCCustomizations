using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCustomizations.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class StaticLengthOfDayPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPrefix]
        static void setTimeSpeedMultiplier(ref float ___globalTimeSpeedMultiplier)
        {
            ___globalTimeSpeedMultiplier = LCCustomizationsModBase.staticTimeSpeed.Value;
        }
    }
}