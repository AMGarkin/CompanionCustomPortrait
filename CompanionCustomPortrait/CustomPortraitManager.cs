using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CompanionCustomPortrait
{
    class CompanionCustomPortraitsManager
    {
        private static CompanionCustomPortraitsManager s_Instance;

        public static CompanionCustomPortraitsManager Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new CompanionCustomPortraitsManager();
                }

                return s_Instance;
            }
        }

        public static readonly Dictionary<string, string> CompanionGUID = new Dictionary<string, string>
        {
            { "bfac222a-f891-4b74-844d-25110d9c5e06", "Amiri"},
            { "53e725f1-219f-4b0b-8ae0-64942623dbf1", "Jubilost"},
            { "83fd1fd9-e43d-4247-b5de-a8fc10b94008", "Valerie"},
            { "49d40534-4b4b-4d9c-ba26-91992a66a632", "Octavia"},
            { "9e0c44f8-7017-49ad-8653-15f9afde7ae1", "Regongar"},
            { "9f24f778-85a5-4df2-ab35-1a40ceaf59c5", "Ekundayo"},
            { "cee3db84-6f46-49a8-8a43-677d05b7fc62", "Nok-Nok"},
            { "ca6226e2-4c69-4441-85ff-37ae71670f99", "Harrim"},
            { "d67ba497-c2ab-402b-a7d0-8013188a6e1a", "Tristian"},
            { "3adcd3b1-ff5f-4113-a3dd-ed16c28b517b", "Jaethal"},
            { "efc204f4-e317-4403-a8ad-058503370c36", "Linzi"},
            { "ValeriScar", "Valerie Alternative"},
        };

        private static readonly string[] PortraitFileNames = { "Small.png", "Medium.png", "FullLength.png" };

        public static readonly string PortraitsDirectoryName = "CompanionCustomPortraits";

        private Dictionary<string, BlueprintPortrait> portraits;

        public void LoadPortraits()
        {
            portraits = new Dictionary<string, BlueprintPortrait>();

            string customPortraitsDirectory = GetPortraitsDirectory();

            foreach (string guid in CompanionGUID.Keys)
            {
                string companionName = CompanionGUID[guid];

                bool notfound = false;

                foreach (string filename in PortraitFileNames)
                {
                    if (!File.Exists(Path.Combine(Path.Combine(customPortraitsDirectory, companionName, filename))))
                    {
                        notfound = true;
                        break;
                    }
                }

                if (notfound) continue;

                portraits[guid] = new BlueprintPortrait{Data = new CustomPortraitData(Path.Combine(customPortraitsDirectory, companionName))};
            }
        }

        public BlueprintPortrait GetPortrait(string guid)
        {
            if (portraits == null)
            {
                LoadPortraits();
            }

            return portraits.Get(guid, null);
        }

        public static string GetPortraitsDirectory()
        {
            var gamePath = Application.dataPath;
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                gamePath += "/../../";
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                gamePath += "/../";
            }

            return Path.Combine(gamePath, PortraitsDirectoryName);
        }
    }
}
