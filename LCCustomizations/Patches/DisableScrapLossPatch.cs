using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCustomizations.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class DisableScrapLossPatch
    {
        private static bool savedValue;

        [HarmonyPatch("DespawnPropsAtEndOfRound")]
        [HarmonyPrefix]
        private static void patchDespawnPropsAtEndOfRoundPrefix()
        {
            savedValue = StartOfRound.Instance.allPlayersDead;
            StartOfRound.Instance.allPlayersDead = false;
        }

        [HarmonyPatch("DespawnPropsAtEndOfRound")]
        [HarmonyPostfix]
        private static void patchDespawnPropsAtEndOfRoundPostfix()
        {
            StartOfRound.Instance.allPlayersDead = savedValue;
        }
    }
}
