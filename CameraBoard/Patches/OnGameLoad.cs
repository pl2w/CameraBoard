using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraBoard.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class OnGameLoad
    {
        public static void Postfix()
        {
            ModLoader.instance.LoadMod();
        }
    }
}
