using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectCache {
    public static ObjectCache curInst { get; set; }

    private static System.Type[] _objType = new System.Type[]{
        typeof(GameObject),
        typeof(Transform),
        typeof(Text),
        typeof(SRContainer),
        typeof(Button)
    };

    private Dictionary<int, object> _objectMap;
    private Queue<int> _unUsedIdQ;
    private int _recoardFlag;

    public ObjectCache() {
        curInst = this;

        _recoardFlag = 1;
        _objectMap = new Dictionary<int, object>();
        _unUsedIdQ = new Queue<int>();
    }

#if UNITY_EDITOR
    public Dictionary<int, object> GetMapForEditor() {
        return _objectMap;
    }
#endif

    public int NewPtr(object obj) {
        int id;
        if(_unUsedIdQ.Count > 0){
            id = _unUsedIdQ.Dequeue();
        }else{
            id = _recoardFlag;
            _recoardFlag++;
        }
        _objectMap[id] = obj;
        return id;
    }

    public void DestroyPtr(int ptr)
    {
        GameObject go = GetGameObject(ptr);
        ClearPtr(ptr);
        UnityEngine.Object.Destroy(go);
    }

    public void ClearPtr(int ptr) {
        _objectMap.Remove(ptr);
        _unUsedIdQ.Enqueue(ptr);
    }

    public bool CheckPtr(int ptr)
    {
        GetData(ptr);
        return true;
    }

    public int GetPtr(int rootPtr, string loc, int typeVal)
    {
        GameObject root = GetGameObject(rootPtr);
        Transform trans = root.transform.Find(loc);
        object _obj;
        if (typeVal == 0)
        {
            _obj = trans.gameObject;
        }
        else if (typeVal == 1)
        {
            _obj = trans;
        }
        else
        {
            _obj = trans.GetComponent(_objType[typeVal]);
        }
        return NewPtr(_obj);
    }

    public object GetData(int ptr)
    {
        object obj;
        _objectMap.TryGetValue(ptr, out obj);
        if (obj == null || obj.Equals(null))
        {
            throw new Exception("fatal: not find ptr relation object");
        }
        return obj;
    }

    public GameObject GetGameObject(int ptr)
    {
        object obj = GetData(ptr);
        GameObject rs = obj as GameObject;
        if (rs == null)
        {
            rs = (obj as Component).gameObject;
        }
        return rs;
    }
}
