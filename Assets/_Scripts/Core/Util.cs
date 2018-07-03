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

    public static bool CheckPtr(int ptr)
    {
        return objCache.CheckPtr(ptr);
    }

    public static void ClearPtr(int ptr)
    {
        objCache.ClearPtr(ptr);
    }

    public static void DestroyPtr(int ptr)
    {
        objCache.DestroyPtr(ptr);
    }

    public static int GetPtr(int rootPtr, string loc, int typeVal)
    {
        return objCache.GetPtr(rootPtr, loc, typeVal);
    }

    public static void InitResourceCtrl()
    {
        resCtrl.Init();
    }

    public static int NewObj(string name, int parentPtr)
    {
        GameObject obj = new GameObject(name);
        if (parentPtr != 0)
        {
            Transform parent = objCache.GetGameObject(parentPtr).transform;
            obj.transform.SetParent(parent, false);
        }
        return objCache.NewPtr(obj);
    }

    public static int NewPref(string path, int parentPtr = 0)
    {
        GameObject inst = _InstObj(path);
        if (parentPtr != 0)
        {
            Transform parent = objCache.GetGameObject(parentPtr).transform;
            inst.transform.SetParent(parent, false);
        }
        return objCache.NewPtr(inst);
    }

    public static void SetActive(int ptr, bool state)
    {
        GameObject _go = objCache.GetGameObject(ptr);
        _go.SetActive(state);
    }

    public static void SetParent(int ptr, int parentPtr)
    {
        Transform parent = objCache.GetGameObject(parentPtr).transform;
        Transform self = objCache.GetGameObject(ptr).transform;
        self.SetParent(parent, false);
    }

    public static void LoadScene(string path)
    {
        AsyncOperation ao = resCtrl.LoadScene(path);
        proxy.StartCo(WaitLoadScene(ao, path));
    }

    private static IEnumerator WaitLoadScene(AsyncOperation ao, string path)
    {
        yield return new WaitUntil(() =>
        {
            return ao.isDone;
        });
        LuaRun.curInst.TrigEv(path);
    }

    public static int NewUI(string path)
    {
        GameObject inst = _InstObj(path);
        uiCtrl.AddUI(inst);
        return objCache.NewPtr(inst);
    }

    public static void SetText(int ptr, string content)
    {
        object obj = objCache.GetData(ptr);
        Text _t = obj as Text;
        _t.text = content;
    }

    public static void SetText(int rootPtr, string loc, string content)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        Text _text = root.transform.Find(loc).GetComponent<Text>();
        _text.text = content;
    }

    public static int InitSR(int rootPtr, string loc, SRContainer.DlgWrapItem onWrap)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        SRContainer _sr = root.transform.Find(loc).GetComponent<SRContainer>();
        _sr.Init(onWrap);
        return objCache.NewPtr(_sr);
    }

    public static void RefreshSR(int ptr, int num)
    {
        object obj = objCache.GetData(ptr);
        SRContainer _sr = obj as SRContainer;
        _sr.Refresh(num);
    }

    public static int SRSelect(int srPtr, int ptr, bool isHl)
    {
        object obj = objCache.GetData(srPtr);
        SRContainer _sr = obj as SRContainer;
        GameObject item = objCache.GetGameObject(ptr);
        return _sr.Select(item, isHl);
    }

    public static void SetUIEv(int rootPtr, string loc, int tp, LuaFunction ev)
    {
        GameObject root = objCache.GetGameObject(rootPtr);
        GameObject _obj = root.transform.Find(loc).gameObject;
        UICtrl.AddUIEvent(_obj, (EventTriggerType)tp, ev);
    }

    public static void SetUIEv(int ptr, int tp, LuaFunction ev)
    {
        GameObject _obj = objCache.GetGameObject(ptr);
        UICtrl.AddUIEvent(_obj, (EventTriggerType)tp, ev);
    }

    public static void AddClick(int ptr, LuaFunction ev)
    {
        GameObject _obj = objCache.GetGameObject(ptr);
        UICtrl.AddClick(_obj, ev);
    }

    public static void AddClick(int rootPtr, string loc, LuaFunction ev)
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
}
