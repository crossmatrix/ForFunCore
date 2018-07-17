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
    public static string atlasABName = "UIAtlas";

    private AssetBundleManifest _manifest;
    private Dictionary<string, AssetBundle> _dpdDic;
    private Dictionary<string, Object> _resourceDic;
    private Dictionary<string, string> _sceneDic;
    private Dictionary<string, Sprite> _spDic;

    public ResourceCtrl() {
        curInst = this;
    }

    //temp
    public void Init()
    {
        _dpdDic = new Dictionary<string, AssetBundle>();
        _resourceDic = new Dictionary<string, Object>();
        _sceneDic = new Dictionary<string, string>();
        _spDic = new Dictionary<string, Sprite>();
        AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath + bundleFolderName);
        _manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        LoadDependency(shaderABName.ToLower());
        Shader.WarmupAllShaders();
    }

    public Object GetResource(string path)
    {
        string rsPath = path;
        if (!Path.HasExtension(path))
        {
            rsPath = Path.ChangeExtension(path, ".prefab");
        }
        rsPath = rsPath.ToLower();

        Object asset;
        if (!_resourceDic.TryGetValue(rsPath, out asset)) {
            string bdName = rsPath.Substring(0, rsPath.LastIndexOf("."));
            string rsName = Path.GetFileName(rsPath);
            AssetBundle bundle = GetBundle(bdName);
            asset = bundle.LoadAsset<Object>(rsName);
            bundle.Unload(false);
            _resourceDic.Add(rsPath, asset);
        }
        return asset;
    }

    public AsyncOperation LoadScene(string path)
    {
        path = path.ToLower();
        string scenePath;
        if (!_sceneDic.TryGetValue(path, out scenePath))
        {
            AssetBundle bundle = GetBundle(path);
            scenePath = bundle.GetAllScenePaths()[0];
            _sceneDic.Add(path, scenePath);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single);
        return operation;
    }

    public Sprite GetSprite(string spritePath) {
        Sprite rs;
        if (!_spDic.TryGetValue(spritePath, out rs)) {
            string[] temp = spritePath.Split('/');
            string atlasName = temp[0];
            string bdName = (atlasABName + "/" + atlasName).ToLower();
            AssetBundle bundle = LoadDependency(bdName);
            Sprite[] allSprite = bundle.LoadAllAssets<Sprite>();
            foreach (Sprite sp in allSprite) {
                _spDic.Add(atlasName + "/" + sp.name, sp);
            }
            rs = _spDic[spritePath];
        }
        return rs;
    }

    private AssetBundle GetBundle(string path)
    {
        foreach (string dpdName in _manifest.GetAllDependencies(path))
        {
            LoadDependency(dpdName);
        }
        return AssetBundle.LoadFromFile(bundlePath + path);
    }

    private AssetBundle LoadDependency(string dpdName)
    {
        AssetBundle dpdBundle;
        if (!_dpdDic.TryGetValue(dpdName, out dpdBundle))
        {
            dpdBundle = AssetBundle.LoadFromFile(bundlePath + dpdName);
            _dpdDic.Add(dpdName, dpdBundle);
        }
        return dpdBundle;
    }
}
