using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ArtTool : MonoBehaviour {
    private static string artPath = "Assets/_Art/";

    [MenuItem("Tools/CopyPref(test)")]
    public static void CopyPref(){
        foreach (string srcPath in Directory.GetFiles(artPath, "*.prefab", SearchOption.AllDirectories)) {
            File.Copy(srcPath, ResourceCtrl.localResourcePath + "Model/" + Path.GetFileName(srcPath));
        }
    }
}
