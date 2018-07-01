using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
using System.Text;

public class CreateLuaFile : MonoBehaviour {
    private static string luaTplPath = "Assets/../Custom/LuaTpl/lua.lua";

    [MenuItem("Assets/Create/Lua Script", false, 80)]
    public static void CreateNewLua() {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            ScriptableObject.CreateInstance<DoCreateScriptAsset>(),
            GetSelectedPath() + "/NewLua.lua",
            null,
            luaTplPath
        );
    }

    public static string GetSelectedPath() {
        string path = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets)) {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path)) {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}

class DoCreateScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        Object o = CreateScriptAssetFromTpl(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }

    internal static Object CreateScriptAssetFromTpl(string pathName, string resourceFile) {
        string fullPath = Path.GetFullPath(pathName);
        string text;
        using (StreamReader sr = new StreamReader(resourceFile)) {
            text = sr.ReadToEnd();
            string fileName = Path.GetFileNameWithoutExtension(pathName);
            text = Regex.Replace(text, "#FileName#", fileName);
        }
        if (!string.IsNullOrEmpty(text))
        {
            UTF8Encoding encoding = new UTF8Encoding(true, false);
            using (StreamWriter sw = new StreamWriter(fullPath, false, encoding)) {
                sw.Write(text);
            }
            AssetDatabase.ImportAsset(pathName);
        }
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
    }
}