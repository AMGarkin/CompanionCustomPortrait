using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionCustomPortrait
{
    [HarmonyPatch(typeof(UnitEntityData), "Portrait", MethodType.Getter)]
    public static class GetPortrait_Patch
    {
        public static bool Prefix(UnitEntityData __instance, ref PortraitData __result)
        {
            if (!Main.enabled) return true;

            string guid = __instance.Blueprint.LocalizedName.String.Key;

            var p = CompanionCustomPortraitsManager.Instance.GetPortrait(guid);

            if (p != null)
            {
                __result = p.Data;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(BlueprintUnit), "PortraitSafe", MethodType.Getter)]
    public static class GetPortraitSafe_Patch
    {
        public static bool Prefix(BlueprintUnit __instance, ref BlueprintPortrait __result)
        {
            if (!Main.enabled) return true;

            string guid = __instance.LocalizedName.String.Key;

            var p = CompanionCustomPortraitsManager.Instance.GetPortrait(guid);

            if (p != null)
            {
                __result = p;
                return false;
            }

            return true;
        }
    }
}
