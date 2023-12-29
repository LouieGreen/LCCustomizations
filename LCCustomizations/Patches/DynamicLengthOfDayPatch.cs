using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCCustomizations.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]
    internal class DynamicLengthOfDayPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPrefix]
        static void setTimeSpeedMultiplier(ref float ___globalTimeSpeedMultiplier)
        {
            int numPlayers = StartOfRound.Instance.connectedPlayersAmount;
            float timeSpeed = 1.0f;

            if (numPlayers == 1)
                timeSpeed = 0.5f;
            else if (numPlayers == 2)
                timeSpeed = 0.65f;
            else if (numPlayers == 3)
                timeSpeed = 0.8f;
            else if (numPlayers >= 4)
                timeSpeed = 1.0f;

            ___globalTimeSpeedMultiplier = timeSpeed;
        }
    }
}
