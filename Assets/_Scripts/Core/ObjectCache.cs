using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectCache {
    public static ObjectCache curInst { get; set; }

    private static Type[] _objType = new Type[]{
        typeof(GameObject),
        typeof(Transform),
        typeof(Text),
        typeof(Image),
        typeof(SliderAid),
        typeof(SRContainer),
        typeof(Button)
    };

    private Dictionary<uint, object> _objectMap;
    private uint _recoardFlag;

    public ObjectCache() {
        curInst = this;

        _recoardFlag = 1;
        _objectMap = new Dictionary<uint, object>();
    }

#if UNITY_EDITOR
    public Dictionary<uint, object> GetMapForEditor() {
        return _objectMap;
    }
#endif

    public uint NewPtr(object obj) {
        uint ptr = _recoardFlag;
        _objectMap[ptr] = obj;
        _recoardFlag++;
        return ptr;
    }

    public void DestroyPtr(uint ptr)
    {
        GameObject go = GetGameObject(ptr);
        ClearPtr(ptr);
        UnityEngine.Object.Destroy(go);
    }

    public void ClearPtr(uint ptr) {
        _objectMap.Remove(ptr);
    }

    public bool CheckPtr(uint ptr)
    {
        GetData(ptr);
        return true;
    }

    public uint GetPtr(uint rootPtr, string loc, int typeVal)
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

    public object GetData(uint ptr)
    {
        object obj;
        _objectMap.TryGetValue(ptr, out obj);
        if (obj == null || obj.Equals(null))
        {
            throw new Exception("fatal: not find ptr relation object: " + ptr);
        }
        return obj;
    }

    public GameObject GetGameObject(uint ptr)
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
