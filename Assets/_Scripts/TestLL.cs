using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class TestLL : MonoBehaviour {
    private ResourceCtrl resCtrl;
    private void Start()
    {
        resCtrl = new ResourceCtrl();
        resCtrl.Init();

        string name = "UI/UILogin";
        Instantiate(resCtrl.GetResource(name) as GameObject);
        //Instantiate(resCtrl.GetResource(name) as GameObject);




        //string root = Application.persistentDataPath + "/AB/";
        //AssetBundle rb = AssetBundle.LoadFromFile(root + "AB");
        //AssetBundleManifest rm = rb.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        //string name = "Model/Eff1";
        //foreach (string dpdPath in rm.GetAllDependencies(name)) {
        //    Debug.Log(root + dpdPath);
        //    AssetBundle.LoadFromFile(root + dpdPath);
        //}
        //AssetBundle mb = AssetBundle.LoadFromFile(root + name);
        //GameObject rs = mb.LoadAsset<GameObject>("Eff1.prefab");
        //mb.Unload(false);

        //Instantiate<GameObject>(rs);

        //mb.Unload(true);
    }
}
