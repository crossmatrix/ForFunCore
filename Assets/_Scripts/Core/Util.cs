using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LuaInterface;

public class Util : MonoBehaviour
{
    private static ObjectCache objCache
    {
        get
        {
            return ObjectCache.curInst;
        }
    }
    private static ResourceCtrl resCtrl
    {
        get
        {
            return ResourceCtrl.curInst;
        }
    }
    private static UICtrl uiCtrl
    {
        get
        {
            return UICtrl.curInst;
        }
    }
    private static GameProxy proxy {
        get {
            return GameProxy.instance;
        }
    }

    public static bool CheckPtr(uint ptr)
    {
        return objCache.CheckPtr(ptr);
    }

    public static void ClearPtr(uint ptr)
    {
        objCache.ClearPtr(ptr);
    }

    public static void DestroyPtr(uint ptr)
    {
        objCache.DestroyPtr(ptr);
    }

    public static uint GetPtr(uint rootPtr, string loc, int typeVal)
    {
        return objCache.GetPtr(rootPtr, loc, typeVal);
    }

    public static void InitResourceCtrl()
    {
        resCtrl.Init();
    }

    public static uint NewObj(string name, uint parentPtr = 0, int active = 0)
    {
        GameObject obj = new GameObject(name);
        _SetObject(obj, active, parentPtr);
        return objCache.NewPtr(obj);
    }

    public static uint NewPref(string path, uint parentPtr = 0, int active = 0)
    {
        GameObject inst = _InstObj(path);
        _SetObject(inst, active, parentPtr);
        return objCache.NewPtr(inst);
    }

    public static void SetObject(uint ptr, int active = 0, uint parentPtr = 0) {
        _SetObject(objCache.GetGameObject(ptr), active, parentPtr);
    }

    public static void LoadScene(string path)
    {
        AsyncOperation ao = resCtrl.LoadScene(path);
        proxy.StartCo(_WaitLoadScene(ao, path));
    }

    public static uint NewUI(string path)
    {
        GameObject inst = _InstObj(path);
        uiCtrl.AddUI(inst);
        return objCache.NewPtr(inst);
    }

    public static void SetText(uint ptr, string content)
    {
        object obj = objCache.GetData(ptr);
        Text _t = obj as Text;
        _t.text = content;
    }

    public static void SetText(uint rootPtr, string loc, string content)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        Text _t = root.transform.Find(loc).GetComponent<Text>();
        _t.text = content;
    }

    public static void SetImage(uint ptr, string spritePath)
    {
        object obj = objCache.GetData(ptr);
        Image _img = obj as Image;
        _img.sprite = resCtrl.GetSprite(spritePath);
    }

    public static void SetImage(uint rootPtr, string loc, string spritePath)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        Image _img = root.transform.Find(loc).GetComponent<Image>();
        _img.sprite = resCtrl.GetSprite(spritePath);
    }

    //public static uint InitSR(uint rootPtr, string loc, SRContainer.DlgWrapItem onWrap)
    //{
    //    GameObject root = objCache.GetGameObject(rootPtr);
    //    SRContainer _sr = root.transform.Find(loc).GetComponent<SRContainer>();
    //    _sr.Init(onWrap);
    //    return objCache.NewPtr(_sr);
    //}

    //public static void RefreshSR(uint ptr, int num)
    //{
    //    object obj = objCache.GetData(ptr);
    //    SRContainer _sr = obj as SRContainer;
    //    _sr.Refresh(num);
    //}

    //public static int SRSelect(uint srPtr, uint ptr, bool isHl)
    //{
    //    object obj = objCache.GetData(srPtr);
    //    SRContainer _sr = obj as SRContainer;
    //    GameObject item = objCache.GetGameObject(ptr);
    //    return _sr.Select(item, isHl);
    //}

    public static void SetUIEv(uint ptr, int tp, LuaFunction ev)
    {
        GameObject _obj = objCache.GetGameObject(ptr);
        UICtrl.AddUIEvent(_obj, (EventTriggerType)tp, ev);
    }

    public static void SetUIEv(uint rootPtr, string loc, int tp, LuaFunction ev)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        GameObject _obj = root.transform.Find(loc).gameObject;
        UICtrl.AddUIEvent(_obj, (EventTriggerType)tp, ev);
    }

    public static void AddClick(uint ptr, LuaFunction ev)
    {
        GameObject _obj = objCache.GetGameObject(ptr);
        UICtrl.AddClick(_obj, ev);
    }

    public static void AddClick(uint rootPtr, string loc, LuaFunction ev)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        GameObject _obj = root.transform.Find(loc).gameObject;
        UICtrl.AddClick(_obj, ev);
    }

    private static GameObject _InstObj(string path)
    {
        GameObject rs = resCtrl.GetResource(path) as GameObject;
        GameObject inst = GameObject.Instantiate(rs);
        inst.name = rs.name;
        return inst;
    }

    private static IEnumerator _WaitLoadScene(AsyncOperation ao, string path)
    {
        yield return new WaitUntil(() =>
        {
            return ao.isDone;
        });
        LuaRun.curInst.TrigEv(path);
    }

    private static void _SetObject(GameObject obj, int active, uint parentPtr)
    {
        if (active == 1)
        {
            obj.SetActive(true);
        }
        else if (active == 2) {
            obj.SetActive(false);
        }
        if (parentPtr != 0)
        {
            Transform parent = objCache.GetGameObject(parentPtr).transform;
            obj.transform.SetParent(parent, false);
        }
    }
}
