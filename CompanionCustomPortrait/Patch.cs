using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.UI.SaveLoadWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CompanionCustomPortrait
{
    [HarmonyPatch(typeof(UnitEntityData), "Portrait", MethodType.Getter)]
    public static class GetPortrait_Patch
    {
        public static bool Prefix(UnitEntityData __instance, ref PortraitData __result)
        {
            if (!Main.enabled) return true;
            if (!__instance.Blueprint.LocalizedName) return true;

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
