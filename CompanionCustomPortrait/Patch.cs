using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.UI.SaveLoadWindow;
using Kingmaker.UI.UnitSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CompanionCustomPortrait
{
    [HarmonyPatch(typeof(UnitUISettings), "PortraitBlueprint", MethodType.Getter)]
    public static class GetPortraitBlueprint_Patch
    {
        public static bool Prefix(UnitUISettings __instance, ref BlueprintPortrait __result)
        {

            if (!Main.enabled) return true;
            if (!__instance.Owner.Blueprint.LocalizedName) return true;

            var p = CompanionCustomPortraitsManager.Instance.GetPortrait(__instance.Owner.Blueprint.LocalizedName.String.Key);

            if (p != null)
            {
                __result = p;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(UnitUISettings), "Portrait", MethodType.Getter)]
    public static class GetPortrait_Patch
    {
        public static bool Prefix(UnitUISettings __instance, ref PortraitData __result, ref BlueprintPortrait ___m_Portrait)
        {
            if (!Main.enabled) return true;
            if (!__instance.Owner.Blueprint.LocalizedName) return true;

            BlueprintPortrait p = null;

            if (___m_Portrait != null && ___m_Portrait.AssetGuid.Equals("8134f34ef1cc67c498f1ae616995023d"))
            {
                p = CompanionCustomPortraitsManager.Instance.GetPortrait("ValeriScar");
            }

            if (p == null)
            {
                p = CompanionCustomPortraitsManager.Instance.GetPortrait(__instance.Owner.Blueprint.LocalizedName.String.Key);
            }

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
            if (!__instance.LocalizedName) return true;

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

    [HarmonyPatch(typeof(SaveManager), "ExtractPortrait")]
    public static class ExtractPortrait_Patch
    {
        public static bool Prefix(SaveManager __instance, ref PortraitForSave __result, UnitEntityData u)
        {
            var name = u.UISettings.Owner.Blueprint.LocalizedName;
            if (name && CompanionCustomPortraitsManager.Instance.GetPortrait(name.String.Key))
            {
                var portrait = (BlueprintPortrait)typeof(BlueprintUnit).GetField("m_Portrait", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(u.UISettings.Owner.Blueprint);
                __result = new PortraitForSave(portrait);
                return false;
            }

            return true;
        }
    }
    
    [HarmonyPatch(typeof(SaveLoadPortraits), "Set")]
    public static class SaveLoadPortraits_Patch
    {
        public static bool Prefix(SaveLoadPortraits __instance, SaveInfo saveInfo, ref List<SaveLoadPortait> ___m_Portraits)
        {
            try
            {
                __instance.Reset();
                if (saveInfo == null)
                {
                    return false;
                }
                List<Sprite> list = new List<Sprite>();
                if (saveInfo.PartyPortraits != null)
                {
                    list.AddRange(from companions in saveInfo.PartyPortraits
                                  where companions != null && companions.Data != null
                                  select companions.Data.SmallPortrait);
                }
                for (int i = 0; i < ___m_Portraits.Count; i++)
                {
                    ___m_Portraits[i].Set((list.Count <= i) ? null : list[i]);
                }
            }
            catch (Exception e)
            {
                FileLog.Log(e.ToString());
            }
            
            return false;
        }
    }
}
