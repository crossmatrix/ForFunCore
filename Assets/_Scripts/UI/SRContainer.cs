using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRContainer : MonoBehaviour
{
    public enum SRType
    {
        Vert,
        Horz,
        Grid
    }

    private class WrapItem
    {
        public RectTransform rt;
        public uint ptr;
        public uint[] ptrs;
        public GameObject[] children;

        public WrapItem(RectTransform rt, int column, Dictionary<GameObject, GameObject> hlMap)
        {
            this.rt = rt;
            if (column > 1)
            {
                ptrs = new uint[column];
                children = new GameObject[column];
                for (int i = 0; i < column; i++)
                {
                    GameObject child = rt.GetChild(i).gameObject;
                    ptrs[i] = ObjectCache.curInst.NewPtr(child);
                    children[i] = child;
                    SaveHlMap(hlMap, child);
                }
            }
            else
            {
                ptr = ObjectCache.curInst.NewPtr(rt.gameObject);
                SaveHlMap(hlMap, rt.gameObject);
            }
        }

        private void SaveHlMap(Dictionary<GameObject, GameObject> hlMap, GameObject obj)
        {
            if (hlMap != null)
            {
                GameObject hlObj = obj.transform.Find(HL_NAME).gameObject;
                hlMap.Add(obj, hlObj);
                hlObj.SetActive(false);
            }
        }
    }

    public delegate void DlgWrapItem(uint ptr, int realIndex);

    const string HL_NAME = "__HL__";

    public float spacing;
    public int column;

    private List<WrapItem> _itemList;
    private DlgWrapItem _onWrapItem;

    private SRType type;

    private RectTransform rt;
    private float interval;
    private float itemSize;
    private float srMainSize;
    private float srMinorSize;
    private float initPos;
    private float halfCtSize;
    private int num;
    private int realNum;
    private int curLastIndex;
    private int curFirstIndex;

    private float lastPrg;

    private int curSelIndex;
    private GameObject prevHlObj;
    private Dictionary<GameObject, GameObject> hlMap;

    public void Init(DlgWrapItem onWrapItem)
    {
        _itemList = new List<WrapItem>();
        this._onWrapItem = onWrapItem;

        rt = transform as RectTransform;
        RectTransform srTrans = transform.parent.parent as RectTransform;
        ScrollRect sr = srTrans.GetComponent<ScrollRect>();
        RectTransform itemTrans = transform.GetChild(0) as RectTransform;
        bool readyHlMap = false;
        if (sr.horizontal)
        {
            type = SRType.Horz;
            srMainSize = srTrans.sizeDelta.x;
            srMinorSize = srTrans.sizeDelta.y;
            itemSize = itemTrans.sizeDelta.x;
            sr.onValueChanged.AddListener(OnValueChangeH);
            readyHlMap = itemTrans.Find(HL_NAME) != null;
        }
        else if (column > 1)
        {
            type = SRType.Grid;
            srMainSize = srTrans.sizeDelta.y;
            srMinorSize = srTrans.sizeDelta.x;
            itemSize = itemTrans.sizeDelta.y;
            sr.onValueChanged.AddListener(OnValueChangeG);
            readyHlMap = itemTrans.GetChild(0).Find(HL_NAME) != null;
        }
        else
        {
            type = SRType.Vert;
            srMainSize = srTrans.sizeDelta.y;
            srMinorSize = srTrans.sizeDelta.x;
            itemSize = itemTrans.sizeDelta.y;
            sr.onValueChanged.AddListener(OnValueChangeV);
            readyHlMap = itemTrans.Find(HL_NAME) != null;
        }
        interval = itemSize + spacing;

        if (readyHlMap)
        {
            hlMap = new Dictionary<GameObject, GameObject>();
        }

        int num = Mathf.CeilToInt(srMainSize / interval) + 1;
        string name = itemTrans.name;
        _itemList.Add(new WrapItem(itemTrans, column, hlMap));
        for (int i = 0; i < num; i++)
        {
            RectTransform inst = GameObject.Instantiate(itemTrans);
            inst.name = name;
            inst.SetParent(transform, false);
            _itemList.Add(new WrapItem(inst, column, hlMap));
        }
    }

    public void Refresh(int num, int target, bool isHl)
    {
        this.num = num;
        int activeNum = 0;
        int targIndex = InverseEnvConvert(target);
        int startIndex = targIndex;
        curSelIndex = -1;
        if (prevHlObj != null)
        {
            prevHlObj.SetActive(false);
        }

        if (type == SRType.Vert)
        {
            float contentSize = interval * num - spacing;
            halfCtSize = contentSize * 0.5f;
            initPos = 0.5f * (contentSize - itemSize);

            rt.sizeDelta = new Vector2(srMinorSize, contentSize);
            float limit = Mathf.Max(0, contentSize - srMainSize);
            rt.anchoredPosition = new Vector2(0, Mathf.Min(limit, startIndex * interval));

            if (num < _itemList.Count)
            {
                startIndex = 0;
            }
            else
            {
                if (startIndex > 0)
                {
                    startIndex -= 1;
                }
                if (startIndex + _itemList.Count > num)
                {
                    startIndex = num - _itemList.Count;
                }
            }

            for (int i = 0; i < _itemList.Count; i++)
            {
                WrapItem item = _itemList[i];
                RectTransform itemRt = item.rt;

                int offsetIndex = i + startIndex;
                if (offsetIndex < num)
                {
                    itemRt.gameObject.SetActive(true);
                    itemRt.anchoredPosition = new Vector2(0, FormulaV(offsetIndex));
                    _onWrapItem(item.ptr, EnvConvert(offsetIndex));
                    activeNum++;
                }
                else
                {
                    itemRt.gameObject.SetActive(false);
                }

                if (isHl && offsetIndex == targIndex)
                {
                    SetHl(targIndex, itemRt.gameObject);
                }
            }
            curFirstIndex = startIndex;
            curLastIndex = startIndex + activeNum;
        }
        else if (type == SRType.Horz)
        {
            float contentSize = interval * num - spacing;
            halfCtSize = contentSize * 0.5f;
            initPos = -0.5f * (contentSize - itemSize);

            rt.sizeDelta = new Vector2(contentSize, srMinorSize);
            float limit = Mathf.Max(0, contentSize - srMainSize);
            rt.anchoredPosition = new Vector2(-Mathf.Min(limit, startIndex * interval), 0);

            if (num < _itemList.Count)
            {
                startIndex = 0;
            }
            else
            {
                if (startIndex > 0)
                {
                    startIndex -= 1;
                }
                if (startIndex + _itemList.Count > num)
                {
                    startIndex = num - _itemList.Count;
                }
            }

            for (int i = 0; i < _itemList.Count; i++)
            {
                WrapItem item = _itemList[i];
                RectTransform itemRt = item.rt;

                int offsetIndex = i + startIndex;
                if (offsetIndex < num)
                {
                    itemRt.gameObject.SetActive(true);
                    itemRt.anchoredPosition = new Vector2(FormulaH(offsetIndex), 0);
                    _onWrapItem(item.ptr, EnvConvert(offsetIndex));
                    activeNum++;
                }
                else
                {
                    itemRt.gameObject.SetActive(false);
                }

                if (isHl && offsetIndex == targIndex)
                {
                    SetHl(targIndex, itemRt.gameObject);
                }
            }
            curFirstIndex = startIndex;
            curLastIndex = startIndex + activeNum;
        }
        else
        {
            realNum = Mathf.CeilToInt((float)num / column);
            float contentSize = interval * realNum - spacing;
            halfCtSize = contentSize * 0.5f;
            initPos = 0.5f * (contentSize - itemSize);

            rt.sizeDelta = new Vector2(srMinorSize, contentSize);
            float limit = Mathf.Max(0, contentSize - srMainSize);

            int startLine = startIndex / column;
            rt.anchoredPosition = new Vector2(0, Mathf.Min(limit, startLine * interval));

            if (realNum < _itemList.Count)
            {
                startLine = 0;
            }
            else
            {
                if (startLine > 0)
                {
                    startLine -= 1;
                }
                if (startLine + _itemList.Count > realNum)
                {
                    startLine = realNum - _itemList.Count;
                }
            }

            for (int i = 0; i < _itemList.Count; i++)
            {
                WrapItem item = _itemList[i];
                RectTransform itemRt = item.rt;

                int offsetLine = i + startLine;
                if (offsetLine < realNum)
                {
                    itemRt.gameObject.SetActive(true);
                    itemRt.anchoredPosition = new Vector2(0, FormulaV(offsetLine));
                    for (int _i = 0; _i < column; _i++)
                    {
                        int _itemIndex = offsetLine * column + _i;
                        _onWrapItem(item.ptrs[_i], EnvConvert(_itemIndex));

                        if (isHl && _itemIndex == targIndex)
                        {
                            SetHl(targIndex, item.children[_i]);
                        }
                    }
                    activeNum++;
                }
                else
                {
                    itemRt.gameObject.SetActive(false);
                }
            }
            curFirstIndex = startLine;
            curLastIndex = startLine + activeNum;
        }
    }

    public int Select(GameObject obj, bool isHl)
    {
        int rs;
        RectTransform rt = obj.transform as RectTransform;
        if (type == SRType.Vert)
        {
            float y = rt.anchoredPosition.y;
            rs = Mathf.RoundToInt((initPos - y) / interval);
        }
        else if (type == SRType.Horz)
        {
            float x = rt.anchoredPosition.x;
            rs = Mathf.RoundToInt((x - initPos) / interval);
        }
        else
        {
            float y = (obj.transform.parent as RectTransform).anchoredPosition.y;
            int line = Mathf.RoundToInt((initPos - y) / interval);
            rs = column * line + obj.transform.GetSiblingIndex();
        }

        if (isHl)
        {
            SetHl(rs, obj);
        }
        return EnvConvert(rs);
    }

    private float FormulaV(int index)
    {
        return initPos - interval * index;
    }

    private float FormulaH(int index)
    {
        return initPos + interval * index;
    }

    private int EnvConvert(int index)
    {
        return index + 1;
    }

    private int InverseEnvConvert(int index) {
        return index - 1;
    }

    private void OnValueChangeV(Vector2 delta)
    {
        if (delta.y - lastPrg > 0)
        {
            //下滑
            foreach (WrapItem item in _itemList)
            {
                RectTransform itemRt = item.rt;
                float toBottom = -itemRt.anchoredPosition.y - (srMainSize + rt.anchoredPosition.y - halfCtSize);
                if (toBottom > interval && curFirstIndex > 0)
                {
                    curFirstIndex--;
                    curLastIndex--;
                    Vector2 pos = itemRt.anchoredPosition;
                    pos.y = FormulaV(curFirstIndex);
                    itemRt.anchoredPosition = pos;
                    CheckHl(curFirstIndex, itemRt.gameObject);
                    _onWrapItem(item.ptr, EnvConvert(curFirstIndex));
                }
            }
        }
        else
        {
            //上滑
            foreach (WrapItem item in _itemList)
            {
                RectTransform itemRt = item.rt;
                float toTop = rt.anchoredPosition.y - halfCtSize + itemRt.anchoredPosition.y;
                if (toTop > interval && curLastIndex < num)
                {
                    Vector2 pos = itemRt.anchoredPosition;
                    pos.y = FormulaV(curLastIndex);
                    itemRt.anchoredPosition = pos;
                    CheckHl(curLastIndex, itemRt.gameObject);
                    _onWrapItem(item.ptr, EnvConvert(curLastIndex));
                    curLastIndex++;
                    curFirstIndex++;
                }
            }
        }
        lastPrg = delta.y;
    }

    private void OnValueChangeH(Vector2 delta)
    {
        if (delta.x - lastPrg > 0)
        {
            //左滑
            foreach (WrapItem item in _itemList)
            {
                RectTransform itemRt = item.rt;
                float toLeft = -rt.anchoredPosition.x - (halfCtSize + itemRt.anchoredPosition.x);
                if (toLeft > interval && curLastIndex < num)
                {
                    Vector2 pos = itemRt.anchoredPosition;
                    pos.x = FormulaH(curLastIndex);
                    itemRt.anchoredPosition = pos;
                    CheckHl(curLastIndex, itemRt.gameObject);
                    _onWrapItem(item.ptr, EnvConvert(curLastIndex));
                    curLastIndex++;
                    curFirstIndex++;
                }
            }
        }
        else
        {
            //右滑
            foreach (WrapItem item in _itemList)
            {
                RectTransform itemRt = item.rt;
                float toRight = itemRt.anchoredPosition.x + halfCtSize - (srMainSize - rt.anchoredPosition.x);
                if (toRight > interval && curFirstIndex > 0)
                {
                    curFirstIndex--;
                    curLastIndex--;
                    Vector2 pos = itemRt.anchoredPosition;
                    pos.x = FormulaH(curFirstIndex);
                    itemRt.anchoredPosition = pos;
                    CheckHl(curFirstIndex, itemRt.gameObject);
                    _onWrapItem(item.ptr, EnvConvert(curFirstIndex));
                }
            }
        }
        lastPrg = delta.x;
    }

    private void OnValueChangeG(Vector2 delta)
    {
        if (delta.y - lastPrg > 0)
        {
            //下滑
            foreach (WrapItem item in _itemList)
            {
                RectTransform itemRt = item.rt;
                float toBottom = -itemRt.anchoredPosition.y - (srMainSize + rt.anchoredPosition.y - halfCtSize);
                if (toBottom > interval && curFirstIndex > 0)
                {
                    curFirstIndex--;
                    curLastIndex--;
                    Vector2 pos = itemRt.anchoredPosition;
                    pos.y = FormulaV(curFirstIndex);
                    itemRt.anchoredPosition = pos;
                    for (int i = 0; i < column; i++)
                    {
                        int index = curFirstIndex * column + i;
                        CheckHl(index, item.children[i]);
                        _onWrapItem(item.ptrs[i], EnvConvert(index));
                    }
                }
            }
        }
        else
        {
            //上滑
            foreach (WrapItem item in _itemList)
            {
                RectTransform itemRt = item.rt;
                float toTop = rt.anchoredPosition.y - halfCtSize + itemRt.anchoredPosition.y;
                if (toTop > interval && curLastIndex < realNum)
                {
                    Vector2 pos = itemRt.anchoredPosition;
                    pos.y = FormulaV(curLastIndex);
                    itemRt.anchoredPosition = pos;
                    for (int i = 0; i < column; i++)
                    {
                        int index = curLastIndex * column + i;
                        CheckHl(index, item.children[i]);
                        _onWrapItem(item.ptrs[i], EnvConvert(index));
                    }
                    curLastIndex++;
                    curFirstIndex++;
                }
            }
        }
        lastPrg = delta.y;
    }

    private void CheckHl(int index, GameObject obj) {
        if (hlMap != null) {
            bool state = index == curSelIndex;
            GameObject hlObj;
            if (hlMap.TryGetValue(obj, out hlObj) && state != hlObj.activeSelf)
            {
                hlObj.SetActive(state);
            }
        }
    }

    private void SetHl(int index, GameObject obj) {
        if (hlMap != null)
        {
            curSelIndex = index;
            GameObject hlObj;
            if (hlMap.TryGetValue(obj, out hlObj))
            {
                if (prevHlObj != null)
                {
                    prevHlObj.SetActive(false);
                }
                hlObj.SetActive(true);
                prevHlObj = hlObj;
            }
        }
    }
}
