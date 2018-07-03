using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LuaRun : IGeneral
{
    public static LuaRun curInst { get; set; }

    private LuaState _luaState;
    private LuaLooper _loop;
    private LuaTable _profiler = null;

    private Dictionary<string, LuaFunction> _refFunc;

    public LuaRun() {
        LuaRun.curInst = this;
    }

    public void _Init(GameObject hostObj)
    {
        LuaFileUtils fileUtil = LuaFileUtils.Instance;
        _luaState = new LuaState();

        _luaState.OpenLibs(LuaDLL.luaopen_pb);
        _luaState.OpenLibs(LuaDLL.luaopen_struct);
        _luaState.OpenLibs(LuaDLL.luaopen_lpeg);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        _luaState.OpenLibs(LuaDLL.luaopen_bit);
#elif UNITY_EDITOR
        _luaState.OpenLibs(LuaDLL.luaopen_snapshot);
#endif

        _luaState.LuaSetTop(0);

        LuaBinder.Bind(_luaState);
        DelegateFactory.Init();

        _luaState.Start();

        _loop = hostObj.AddComponent<LuaLooper>();
        _loop.luaState = _luaState;

        _refFunc = new Dictionary<string, LuaFunction>();
    }

    public void _Start()
    {
        _luaState.DoFile("Main.lua");
    }

    public void _Update()
    {

    }

    public void _Close()
    {
        if (_luaState != null)
        {
            _refFunc.Clear();

            LuaState state = _luaState;
            _luaState = null;

            DetachProfiler();

            if (_loop != null)
            {
                Object.Destroy(_loop);
                _loop = null;
            }

            state.Dispose();
        }
    }

    public void TrigEv(string name) {
        LuaFunction lFunc = GetLuaFunc("trigForCS");
        lFunc.BeginPCall();
        lFunc.Push(name);
        lFunc.PCall();
        lFunc.EndPCall();
    }

    public void AttachProfiler()
    {
        if (_profiler == null)
        {
            _profiler = _luaState.Require<LuaTable>("UnityEngine.Profiler");
            _profiler.Call("start", _profiler);
        }
    }

    public void DetachProfiler()
    {
        if (_profiler != null)
        {
            _profiler.Call("stop", _profiler);
            _profiler.Dispose();
            LuaProfiler.Clear();
        }
    }

    private LuaFunction GetLuaFunc(string name)
    {
        LuaFunction lFunc;
        if (!_refFunc.TryGetValue(name, out lFunc)) {
            lFunc = _luaState.GetFunction(name);
            if (lFunc != null)
            {
                _refFunc.Add(name, lFunc);
            }
            else {
                throw new LuaException("not find luafunction: " + name);
            }
        }
        return lFunc;
    }

#if UNITY_EDITOR
    [MenuItem("Lua/Attach Profiler %,", false, 151)]
    public static void AttachLuaProfiler()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        curInst.AttachProfiler();
        Debug.Log("开启LuaProfiler");
    }

    [MenuItem("Lua/Detach Profiler %.", false, 152)]
    public static void DetachLuaProfiler()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        curInst.DetachProfiler();
        Debug.Log("关闭LuaProfiler");
    }
#endif
}
