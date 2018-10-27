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

            UploadImages();
        }

        public void UploadImages()
        {
            var PortraitImage = UploadSprite(Path.Combine(m_CustomPortraitDirectory, "Small.png"), BlueprintRoot.Instance.CharGen.BasePortraitSmall);
            var HalfLengthImage = UploadSprite(Path.Combine(m_CustomPortraitDirectory, "Medium.png"), BlueprintRoot.Instance.CharGen.BasePortraitMedium);
            var FullLengthImage = UploadSprite(Path.Combine(m_CustomPortraitDirectory, "Fulllength.png"), BlueprintRoot.Instance.CharGen.BasePortraitBig);

            var baseClass = typeof(CustomPortraitData).BaseType;

            baseClass.GetField("m_PortraitImage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, PortraitImage);
            baseClass.GetField("m_HalfLengthImage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, HalfLengthImage);
            baseClass.GetField("m_FullLengthImage", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, FullLengthImage);
        }

        private static Sprite UploadSprite(string file, Sprite baseSprite)
        {
            Sprite result;
            try
            {
                byte[] data = File.ReadAllBytes(file);
                int width = baseSprite.texture.width;
                int height = baseSprite.texture.height;

                Texture2D texture2D = new Texture2D(width, height);
                texture2D.LoadImage(data);
                result = Sprite.Create(texture2D, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f);
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
