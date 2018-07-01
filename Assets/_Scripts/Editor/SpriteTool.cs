using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteTool : MonoBehaviour
{
    [MenuItem("Assets/TexToSp", true)]
    public static bool SplitToSpriteValid()
    {
        return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(Texture2D);
    }

    [MenuItem("Assets/TexToSp")]
    public static void SplitToSprite()
    {
        Texture2D tex = Selection.activeObject as Texture2D;
        if (tex != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(tex);
            string metaPath = assetPath.Replace(".png", ".txt");
            TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>(metaPath);
            if (ta == null)
            {
                Debug.LogError("没有找到" + tex.name + "的sprite分割文件");
                return;
            }

            TextureImporter ti = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            TextureImporterSettings settings = new TextureImporterSettings();

            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Multiple;

            SpriteMetaData[] spritesheetMeta = GetSpMetaData(ta.text);
            ti.spritesheet = spritesheetMeta;

            ti.ReadTextureSettings(settings);
            settings.readable = false;
            settings.mipmapEnabled = false;
            ti.SetTextureSettings(settings);

            AssetDatabase.DeleteAsset(metaPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(assetPath);
        }
    }

    static SpriteMetaData[] GetSpMetaData(string spDatas)
    {
        List<SpriteMetaData> metaList = new List<SpriteMetaData>();
        foreach (string line in spDatas.Split('\n'))
        {
            if (line.Length > 0)
            {
                metaList.Add(SpMetaParse(line));
            }
        }
        return metaList.ToArray();
    }

    static SpriteMetaData SpMetaParse(string line)
    {
        SpriteMetaData meta = new SpriteMetaData();
        string[] block = line.Split(':');
        meta.name = block[0];

        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach (string slice in block[1].Split(','))
        {
            string[] kv = slice.Split('=');
            dic.Add(kv[0], kv[1]);
        }

        meta.rect = new Rect(int.Parse(dic["posX"]), int.Parse(dic["posY"]), int.Parse(dic["width"]), int.Parse(dic["height"]));
        meta.border = new Vector4(GetVal(dic, "l"), GetVal(dic, "b"), GetVal(dic, "r"), GetVal(dic, "t"));

        return meta;
    }

    static int GetVal(Dictionary<string, string> dic, string key)
    {
        string val;
        if (dic.TryGetValue(key, out val))
        {
            return int.Parse(val);
        }
        else
        {
            return 0;
        }
    }
}
