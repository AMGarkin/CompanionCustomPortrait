using Kingmaker.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.IO;
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
            //companions
            { "9608ea376ae7821419b6a7adfe575e3e", "Amiri" },
            { "f8774a1b6043236418eeae0ed490ee94", "Ekundayo" },
            { "a87b6eb02054bb5439d3de2ba34f950b", "Harrim" },
            { "8fe333f837f7b5e4bb0725199b101fdd", "Jaethal" },
            { "fd639d177de61144d9971536d584b47b", "Jubilost" },
            { "a46809e5b42212045a29d7e37fc27d94", "Linzi" },
            { "2fe663d085beff742a83ae87b63fffaa", "Nok-Nok" },
            { "3d7f4e873564c1746813ed4c465bc87d", "Octavia" },
            { "6e7302bb773adf04299dbe8832562d50", "Regongar" },
            { "cfb97eb4f37ff4a468cd64857a6922c2", "Valerie" },
            { "8134f34ef1cc67c498f1ae616995023d", "Valerie Scar" },
            { "82e448d871cb4014891a2ccf705e910a", "Tristian" },
            { "c0230e1fee517664784355441dd5b6e8", "Tristian Blind" },
            //advisors
            { "09fb6748fe58dea4fbf3f37c5e4ba8ab", "Bartholomew Delgado" },
            { "3a664504f1764d040ba3340eb580253e", "Jhod Kavken" },
            { "4a2c9558226bb8345ac402ab6099df61", "Kassil Aldori" },
            { "f51ff39fb951c1241944e49ea438ccdd", "Kesten Garess" },
            { "0a2dfc67a2b91f04c90f5d07364ebc94", "Lander Lebeda" },
            { "2dd54e7a2506faa4d9b817cf13df0c77", "Maegar Varn" },
            { "b1ec1e6279f1ff643bb8a5ffc7061675", "Shandra Mervey" },
            { "ca7e9df2ad3b43843894e3a393a5d08d", "The Storyteller" },
            { "53378baf75366a940bb5b35e4e465ea5", "Tsanna" },
            { "eb5a7b5b6e7a5234a935599a3007fd64", "Vordakai" },
            { "7fcf39f3dc615094990f43db961f615c", "Vordakai Blind" },
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
