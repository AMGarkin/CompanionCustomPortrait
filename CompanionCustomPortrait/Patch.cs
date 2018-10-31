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
        public static bool Prefix(ref BlueprintPortrait __result, BlueprintPortrait ___m_Portrait)
        {

            if (!Main.enabled) return true;
            if (___m_Portrait?.AssetGuid == null) return true;

            BlueprintPortrait p = CompanionCustomPortraitsManager.Instance.GetPortrait(___m_Portrait.AssetGuid);

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
        public static bool Prefix(ref PortraitData __result, BlueprintPortrait ___m_Portrait)
        {
            if (!Main.enabled) return true;
            if (___m_Portrait?.AssetGuid == null) return true;

            BlueprintPortrait p = CompanionCustomPortraitsManager.Instance.GetPortrait(___m_Portrait.AssetGuid);

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
        public static bool Prefix(ref BlueprintPortrait __result, BlueprintPortrait ___m_Portrait)
        {
            if (!Main.enabled) return true;
            if (___m_Portrait?.AssetGuid == null) return true;

            BlueprintPortrait p = CompanionCustomPortraitsManager.Instance.GetPortrait(___m_Portrait.AssetGuid);

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
        public static bool Prefix(ref PortraitForSave __result, UnitEntityData u)
        {
            if (!Main.enabled) return true;
            if (u.UISettings?.PortraitBlueprint?.AssetGuid == null) return true;

            BlueprintPortrait portrait = CompanionCustomPortraitsManager.Instance.GetPortrait(u.UISettings.PortraitBlueprint.AssetGuid);

            if (portrait != null)
            {
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
