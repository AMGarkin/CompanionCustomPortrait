using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CompanionCustomPortrait
{
    class CustomPortraitData : PortraitData
    {
        public CustomPortraitData(string path) : base(null)
        {
            m_CustomPortraitDirectory = path;

            UploadImages(true);
        }

        public new void UploadImages(bool force = false)
        {
            var PortraitImage = UploadSprite(Path.Combine(m_CustomPortraitDirectory, "Small.png"), BlueprintRoot.Instance.CharGen.BasePortraitSmall, force);
            var HalfLengthImage = UploadSprite(Path.Combine(m_CustomPortraitDirectory, "Medium.png"), BlueprintRoot.Instance.CharGen.BasePortraitMedium, force);
            var FullLengthImage = UploadSprite(Path.Combine(m_CustomPortraitDirectory, "Fulllength.png"), BlueprintRoot.Instance.CharGen.BasePortraitBig, force);

            var baseClass = typeof(CustomPortraitData).BaseType;

            baseClass.GetField("m_PortraitImage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, PortraitImage);
            baseClass.GetField("m_HalfLengthImage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, HalfLengthImage);
            baseClass.GetField("m_FullLengthImage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, FullLengthImage);
        }

        private static Sprite UploadSprite(string file, Sprite baseSprite, bool force = false)
        {
            Sprite result;
            try
            {
                result = CustomPortraitsManager.Instance.LoadPortrait(file, baseSprite, force);
            }
            catch (Exception ex)
            {
                Harmony12.FileLog.Log("Failed to load portrait " + file);
                Harmony12.FileLog.Log(ex.ToString());
                result = baseSprite;
            }
            return result;
        }

        private readonly string m_CustomPortraitDirectory;
    }
}
