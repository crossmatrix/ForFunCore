using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class RunInfo : IGeneral {
    const float MB_UNIT = 1024 * 1024;
    const float REFRESH_INTERVAL = 1f;

    public static RunInfo curInst { get; set; }

    private float _count;
    private int _totalFrame;
    private float _fps;

    public RunInfo()
    {
        RunInfo.curInst = this;
    }

    public void _Init(GameObject hostObj)
    {

    }

    public void _Start()
    {
        _count = 0;
        _totalFrame = 0;
        _fps = 0;
    }

    public void _Update()
    {
        _totalFrame += 1;
        _count += Time.unscaledDeltaTime;
        if (_count >= REFRESH_INTERVAL)
        {
            _fps = float.Parse((_totalFrame / _count).ToString("#0.0"));
            _count = 0;
            _totalFrame = 0;
        }
    }

    public void _Close()
    {

    }

    public string GetRunningInfo() {
        string info = string.Format("total mem:{2:0.0}, fps:{3}",
            Profiler.GetTotalReservedMemoryLong() / MB_UNIT,
            _fps
        );
        return info;
    }
}
