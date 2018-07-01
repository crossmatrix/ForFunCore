using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICtrl
{
    public static UICtrl curInst { get; set; }

    private Camera _uiCam;

    public UICtrl() {
        curInst = this;

        _uiCam = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    public void AddUI(GameObject obj)
    {
        obj.transform.SetParent(_uiCam.transform, false);
        obj.GetComponent<Canvas>().worldCamera = _uiCam;
    }

    public static void AddUIEvent(GameObject obj, EventTriggerType evType, LuaFunction func)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        UnityAction<BaseEventData> act = (d) =>
        {
            func.Call();
        };
        EventTrigger.Entry entry = trigger.triggers.Find((x) =>
        {
            return x.eventID == evType;
        });
        if (entry == null)
        {
            entry = new EventTrigger.Entry();
            entry.eventID = evType;
            entry.callback.AddListener(act);
            trigger.triggers.Add(entry);
        }
        else
        {
            entry.callback.RemoveAllListeners();
            entry.callback.AddListener(act);
        }
    }

    public static void AddClick(GameObject obj, LuaFunction ev)
    {
        Button btn = obj.GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => {
            ev.Call();
        });
    }
}
