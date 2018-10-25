using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace CompanionCustomPortrait
{
    public static class Main
    {
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            enabled = modEntry.Active;

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUI;

            try
            {
                var harmony = HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                modEntry.Logger.Log(e.ToString());
                return false;
            }

            CreateDirectories();

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Open Portraits Directory", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
            {
                System.Diagnostics.Process.Start(CompanionCustomPortraitsManager.GetPortraitsDirectory());
            }

            if (GUILayout.Button("Reload Portraits", new GUILayoutOption[] { GUILayout.ExpandWidth(false) }))
            {
                CompanionCustomPortraitsManager.Instance.LoadPortraits();
            }

            GUILayout.Label(string.Format("Reload may not take effect immediately"), new GUILayoutOption[]{GUILayout.ExpandWidth(false)});

            GUILayout.EndHorizontal();
        }

        public static void CreateDirectories()
        {
            string customPortraitsDirectory = CompanionCustomPortraitsManager.GetPortraitsDirectory();

            foreach (string companionName in CompanionCustomPortraitsManager.CompanionGUID.Values)
            {
                Directory.CreateDirectory(Path.Combine(customPortraitsDirectory, companionName));
            }
        }
    }
}
