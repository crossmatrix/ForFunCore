using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WndObjCache : EditorWindow {
    private Vector2 pos;

    [MenuItem("Tools/WndObjCache")]
    static void Init() {
        WndObjCache wnd = EditorWindow.GetWindow<WndObjCache>();
        wnd.Show();

        wnd.Repaint();
    }

    void OnGUI()
    {
        var map = ObjectCache.curInst.GetMapForEditor();
        pos = GUILayout.BeginScrollView(pos, false, true, GUILayout.ExpandHeight(true));
        if (Application.isPlaying) {
            foreach (KeyValuePair<uint, object> kv in map)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(kv.Key.ToString());
                GUILayout.Label(kv.Value.ToString());
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
    }

    void Update() {
        Repaint();
    }
}
