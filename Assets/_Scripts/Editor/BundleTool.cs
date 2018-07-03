using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BundleTool : MonoBehaviour {
    /// <summary>
    /// 1.shader物理体积占用虽然小，但加载开销却很高，主要是cpu要解析shader
    /// 2.shader variants越多，加载和包体压力就越大
    /// 3.减少variants的一个有效办法是关闭fallback
    /// 4.统计出使用的shader单独打成一个bundle，作为游戏初始化的一部分加载并常驻（后续使用不必加载和解析）
    /// 5.如果使用了variants巨多的standard shader，不可能对standard打包（放进includelist也不行），就无法通过预加载解决（禁用standard）
    /// 6.禁用默认材质，多个地方使用默认材质时，就算shader被预加载，也会出现内置图片素材冗余打包，除此而外，Default-Material是standard的
    /// 7._Resources作为程序使用资源的目录
    /// 8.除特定文件夹下存放的原始资源直接打包外（uiatlas的图集，vedio的声音，脚本控制的mat...）其余文件夹只存prefab（包括模型，特效...）
    /// 9.打包的原则：公用必须提出，尽可能少的拆分（尽可能减少依赖）
    /// 10.我的依赖一定是公用的（除shader），所以肯定有多个bundle在使用，所以依赖bundle不会卸载，而目标bundle取出asset后就会卸载（对asset缓存）
    /// </summary>

    [MenuItem("Tools/Assetbundle/CreateBundle %q")]
    public static void CreateBundle()
    {
        BuildPipeline.BuildAssetBundles(Application.persistentDataPath + "/" + ResourceCtrl.bundleFolderName, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);

        //AssetDatabase.Refresh();
        //AssetDatabase.SaveAssets();
        //foreach (string dpd in AssetDatabase.GetDependencies("Assets/_Resources/Model/Monx1.prefab", false))
        //{
        //    Debug.Log(dpd);
        //}
    }

    [MenuItem("Tools/Assetbundle/ClearName %w")]
    public static void ClearName()
    {
        foreach (string abName in AssetDatabase.GetAllAssetBundleNames())
        {
            AssetDatabase.RemoveAssetBundleName(abName, true);
        }
    }

    [MenuItem("Tools/Assetbundle/AssignName %e")]
    public static void AssginName()
    {
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        ClearName();

        CheckRs();

        List<Shader> shaderList = new List<Shader>();
        List<Object> forceList = new List<Object>();
        Dictionary<Object, List<Object>> dpdDic = new Dictionary<Object, List<Object>>();

        DirectoryInfo dirInfo = new DirectoryInfo(ResourceCtrl.localResourcePath);
        FileInfo[] infos = dirInfo.GetFiles("*", SearchOption.AllDirectories);
        foreach (FileInfo info in infos)
        {
            if (!info.Name.EndsWith("meta"))
            {
                string path = info.FullName.Substring(info.FullName.IndexOf("Assets"));
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);

                AnalysisShader(path, asset, shaderList, forceList);
                AnalysisDpd(path, asset, dpdDic);
            }
        }
        dpdDic = TrimRepeated(dpdDic);

        foreach (Shader shader in shaderList)
        {
            Debug.Log("<color=#00ffff>[shader]</color>" + shader);
            SetBundleName(shader, BundleNameType.Shader);
        }
        foreach (KeyValuePair<Object, List<Object>> kv in dpdDic)
        {
            //string str = kv.Key + " -> ";
            //foreach (Object dpd in kv.Value)
            //{
            //    str += dpd + ", ";
            //}
            //Debug.Log(str);
            if (kv.Value.Count > 1)
            {
                Debug.Log("<color=#ff0000>[public]</color>" + kv.Key);
                SetBundleName(kv.Key, BundleNameType.Public);
            }
        }
        foreach (Object asset in forceList)
        {
            Debug.Log("<color=#00ff00>[force]</color>" + asset);
            SetBundleName(asset, BundleNameType.Force);
        }
    }

    private static void AnalysisShader(string path, Object asset, List<Shader> shaderList, List<Object> forceList) {
        System.Type shaderType = typeof(Shader);
        if (asset.GetType() == shaderType)
        {
            AddShader(shaderList, asset as Shader);
        }
        else {
            Object[] dpds = EditorUtility.CollectDependencies(new Object[] { asset });
            foreach (Object dpd in dpds) {
                if (dpd.GetType() == shaderType) {
                    //Debug.Log(">>>>>>>" + path + " - " + dpd);
                    AddShader(shaderList, dpd as Shader);
                }
            }

            if (!forceList.Contains(asset)) {
                forceList.Add(asset);
            }
        }
    }

    private static void AddShader(List<Shader> shaderList, Shader shader) {
        if (!shaderList.Contains(shader)) {
            shaderList.Add(shader);
        }
    }

    private static void AnalysisDpd(string path, Object asset, Dictionary<Object, List<Object>> dpdDic) {
        foreach (string dpdPath in AssetDatabase.GetDependencies(path, false)) {
            Object dpd = AssetDatabase.LoadAssetAtPath<Object>(dpdPath);
            if (!IsValidDpd(dpd)) {
                continue;
            }
            List<Object> belongList;
            if (!dpdDic.TryGetValue(dpd, out belongList)) {
                belongList = new List<Object>();
                dpdDic.Add(dpd, belongList);
            }
            if (!belongList.Contains(asset)) {
                belongList.Add(asset);
            }
            if (IsRecursive(dpdPath)) {
                AnalysisDpd(AssetDatabase.GetAssetPath(dpd), dpd, dpdDic);
            }
        }
    }

    private static bool IsValidDpd(Object asset) {
        bool result = true;
        System.Type type = asset.GetType();
        if (type == typeof(MonoScript) || type == typeof(Shader)) {
            result = false;
        }
        return result;
    }

    private static bool IsRecursive(string path)
    {
        bool result = true;
        string extension = Path.GetExtension(path).ToLower();
        if (extension == ".fbx")
        {
            result = false;
        }
        return result;
    }

    private static Dictionary<Object, List<Object>> TrimRepeated(Dictionary<Object, List<Object>> dpdDic)
    {
        Dictionary<Object, List<Object>> newDpdDic = new Dictionary<Object, List<Object>>();
        foreach (KeyValuePair<Object, List<Object>> kv in dpdDic)
        {
            Object dpd = kv.Key;
            List<Object> belongList = kv.Value;
            if (belongList.Count > 1) {
                List<Object> newBelongList = new List<Object>();
                foreach (Object belong in belongList) { 
                    bool isAdd = true;
                    List<Object> belongBelong;
                    if (dpdDic.TryGetValue(belong, out belongBelong))
                    {
                        if (belongBelong.Count == 1 && belongList.Contains(belongBelong[0]))
                        {
                            isAdd = false;
                        }
                    }
                    if (isAdd) {
                        newBelongList.Add(belong);
                    }
                }
                if (newBelongList.Count > 1) {
                    newDpdDic.Add(dpd, newBelongList);
                }
            }
        }
        return newDpdDic;
    }

    private static void CheckRs() {
        foreach (string file in Directory.GetFiles(ResourceCtrl.localResourcePath, "*", SearchOption.AllDirectories)) {
            if (!file.EndsWith(".meta")) {
                 Object asset = AssetDatabase.LoadAssetAtPath<Object>(file);
                 foreach (Object dpd in EditorUtility.CollectDependencies(new Object[] { asset })) {
                     System.Type tp = dpd.GetType();
                     if (tp == typeof(Material)) {
                        //禁止使用以下默认材质（如果多个地方使用同一个默认材质，无法为它制作ab）
                        if (dpd.name == "Default-Diffuse" ||
                            dpd.name == "Default-Particle" ||
                            dpd.name == "Default-Skybox" ||
                            dpd.name == "Default-Material")
                        {
                            throw new System.Exception(file + " use default mat -> " + dpd);
                        }
                     }
                     else if (tp == typeof(Shader)) {
                        //禁止使用standard相关（这是一个变体shader，如果直接加入always列表，会把大量变体shader打入包）
                        if (dpd.name.Contains("Standard"))
                        {
                            throw new System.Exception(file + " use standard shader -> " + dpd.name);
                        }
                    }
                 }
            }
        }
        //Debug.Log("Check Suc");
    }

    private static void SetBundleName(Object asset, BundleNameType nameType) {
        string path = AssetDatabase.GetAssetPath(asset);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        //Debug.Log("->" + bundleName);
        if (importer != null)
        {
            if (nameType == BundleNameType.Shader)
            {
                importer.assetBundleName = ResourceCtrl.shaderABName;
            }
            else if (nameType == BundleNameType.Public)
            {
                int _pos = ResourceCtrl.localArtPath.Length;
                importer.assetBundleName = ResourceCtrl.artABName + "/" + path.Substring(_pos, path.Length - _pos - Path.GetExtension(path).Length);
            }
            else if (nameType == BundleNameType.Force) {
                int _pos = ResourceCtrl.localResourcePath.Length;
                importer.assetBundleName = path.Substring(_pos, path.Length - _pos - Path.GetExtension(path).Length);
            }
        }
        else {
            Debug.Log("<color=#ffff00>built-in </color>" + asset);
        }
    }

    public enum BundleNameType {
        Shader,
        Public,
        Force
    }
}
