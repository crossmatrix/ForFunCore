using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ResourceCtrl 
{
    public static ResourceCtrl curInst { get; set; }

    public static string bundleFolderName = "AB";
    public static string bundlePath = string.Format("{0}/{1}/", Application.persistentDataPath, bundleFolderName);
    public static string localResourcePath = "Assets/_Resources/";
    public static string localArtPath = "Assets/_Art/";
    public static string shaderABName = "Shader";
    public static string artABName = "Art";

    private AssetBundleManifest _manifest;
    private Dictionary<string, AssetBundle> _dpdDic;
    private Dictionary<string, Object> _resourceDic;
    private Dictionary<string, string> _sceneDic;

    public ResourceCtrl() {
        curInst = this;
    }

    //temp
    public void Init()
    {
        _dpdDic = new Dictionary<string, AssetBundle>();
        _resourceDic = new Dictionary<string, Object>();
        _sceneDic = new Dictionary<string, string>();
        AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath + bundleFolderName);
        _manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        LoadDependency(shaderABName.ToLower());
        Shader.WarmupAllShaders();
    }

    public Object GetResource(string path)
    {
        if (!Path.HasExtension(path)) {
            path = Path.ChangeExtension(path, ".prefab");
        }
        Object asset;
        if (!_resourceDic.TryGetValue(path, out asset)) {
            string rsName;
            AssetBundle bundle = GetBundle(path, out rsName);
            asset = bundle.LoadAsset<Object>(rsName);
            bundle.Unload(false);
            _resourceDic.Add(path, asset);
        }
        return asset;
    }

    public AsyncOperation LoadScene(string path)
    {
        string scenePath;
        if (!_sceneDic.TryGetValue(path, out scenePath))
        {
            string rsName;
            AssetBundle bundle = GetBundle(path, out rsName);
            scenePath = bundle.GetAllScenePaths()[0];
            _sceneDic.Add(path, scenePath);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        return operation;
    }

    private AssetBundle GetBundle(string path, out string rsName) {
        string relativePath = path.ToLower();
        string dir = Path.GetDirectoryName(relativePath);
        string fileName = Path.GetFileNameWithoutExtension(relativePath);
        string extension = Path.GetExtension(fileName);
        string bundleName = dir + "/" + fileName;
        foreach (string dpdName in _manifest.GetAllDependencies(bundleName))
        {
            LoadDependency(dpdName);
        }
        rsName = fileName + extension;
        return AssetBundle.LoadFromFile(bundlePath + bundleName);
    }

    private void LoadDependency(string dpdName)
    {
        AssetBundle dpdBundle;
        if (!_dpdDic.TryGetValue(dpdName, out dpdBundle))
        {
            dpdBundle = AssetBundle.LoadFromFile(bundlePath + dpdName);
            _dpdDic.Add(dpdName, dpdBundle);
        }
    }
}
