using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEditor.Callbacks;
using System.IO;
using System.Text.RegularExpressions;

public class LogLocation : Editor
{
    private static string _sublimePath = Application.dataPath + "/../Custom/Sublime Text 3/subl.exe";
    private static object _consoleWindow = null;
    private static List<string> _queryPathList = null;
    private static string _luaPattern = string.Format("{0}(.*?){1}]:(\\d+)", "\"", "\"");
    private static string _tracePattern = "stack traceback";

    private static object consoleWindow
    {
        get
        {
            if (_consoleWindow == null)
            {
                Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
                Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
                FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
                _consoleWindow = fieldInfo.GetValue(null);
            }
            return _consoleWindow;
        }
    }

    private static List<string> queryPathList
    {
        get
        {
            if (_queryPathList == null)
            {
                _queryPathList = new List<string>();
                _queryPathList.Add(LuaConst.luaDir);
                _queryPathList.Add(LuaConst.toluaDir);
            }
            return _queryPathList;
        }
    }

    [OnOpenAsset()]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        bool fromConsole = (object)EditorWindow.focusedWindow == consoleWindow;
        bool fromProjLua = false;
        string assetPath = "";
        if (line == -1)
        {
            assetPath = AssetDatabase.GetAssetPath(instanceID);
            fromProjLua = assetPath.EndsWith(".lua");
        }

        if (fromConsole)
        {
            FieldInfo info = consoleWindow.GetType().GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            string activeStr = info.GetValue(consoleWindow).ToString();
            if (!string.IsNullOrEmpty(activeStr))
            {
                string _file, _line;
                bool analysisRs = AnalysisLuaPos(activeStr, out _file, out _line);
                if (analysisRs)
                {
                    OpenLuaIDE(_file, int.Parse(_line));
                }
                return analysisRs;
            }
            return false;
        }
        else if (fromProjLua)
        {
            OpenLuaIDE(Path.GetFullPath(assetPath));
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void OpenLuaIDE(string path, int line = 1)
    {
        if (File.Exists(_sublimePath))
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = _sublimePath;
            proc.StartInfo.Arguments = string.Format("{0}:{1}", path, line);
            proc.Start();
            //Debug.Log(proc.StartInfo.FileName);
            //Debug.Log(proc.StartInfo.Arguments);
        }
        else
        {
            Debug.LogError("not find sumblime");
        }
    }

    private static string FindFullPath(string shortPath)
    {
        string fullPath = "";
        //Debug.Log(">>> " + shortPath);
        List<string> list = queryPathList;
        foreach (string qPath in list)
        {
            string temp = qPath + "/" + shortPath;
            if (File.Exists(temp))
            {
                fullPath = temp;
                break;
            }
        }
        return fullPath;
    }

    private static bool AnalysisLuaPos(string checkPart, out string _file, out string _line)
    {
        //如果有多个traceback，匹配第一个traceback的第一个lua位置，否则返回false
        //如果有1个或者0个traceback，从第一行匹配所有可能项
        //1.如果没有->向下找，直到有，如果全部都没有则返回false
        //2.如果有多个->选择最后一个
        //3.如果只有一个->使用这个
        bool findRs = false;

        int count = Regex.Matches(checkPart, _tracePattern).Count;
        if (count > 1) {
            string chunkPart = checkPart.Substring(checkPart.IndexOf(_tracePattern));
            GroupCollection collection = Regex.Match(chunkPart, _luaPattern).Groups;
            _file = collection[1].Value;
            _line = collection[2].Value;
        }
        else
        {
            string fstLine = checkPart.Split('\n')[0];
            MatchCollection mc = Regex.Matches(fstLine, _luaPattern);
            if (mc.Count == 0)
            {
                GroupCollection collection = Regex.Match(checkPart, _luaPattern).Groups;
                _file = collection[1].Value;
                _line = collection[2].Value;
            }
            else if (mc.Count == 1)
            {
                _file = mc[0].Groups[1].Value;
                _line = mc[0].Groups[2].Value;
            }
            else
            {
                int lastIndex = mc.Count - 1;
                _file = mc[lastIndex].Groups[1].Value;
                _line = mc[lastIndex].Groups[2].Value;
            }
        }
        
        if (!string.IsNullOrEmpty(_file) && !string.IsNullOrEmpty(_line))
        {
            if (_file.EndsWith(".cs"))
            {
                findRs = false;
            }
            else {
                findRs = true;
                if (!_file.EndsWith(".lua"))
                {
                    _file = _file + ".lua";
                }
                _file = FindFullPath(_file);
            }
        }
        return findRs;
    }
}
