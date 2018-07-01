using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class TestLL : MonoBehaviour {
    //int比Transform等高级对象快
    //参数越少越快
    //变长参数慢,数组更慢，但能接受（100万次2个参数试验比较：0.45，0.73，1.15）
    //luatable比数组更更更慢，而且luatable过多会锁死unity！每次ToLua.CheckLuaTable可能会新建一个缓存table，然后锁住gclist并删除这个table的ref（瓶颈）
    //重载无影响
    //继承对象略微慢
    //委托比luafunction慢太多！


    public delegate void TestDlg(GameObject obj);

    private TestDlg _dlg;
    private LuaFunction _lf;
    private System.Action _act;

    //no param, no return
    public void i1() {
        //0.25
    }

    //Transform test
    public void i2(Transform a) { 
        //0.285
        //0.34
    }

    public void i2(Transform a, Transform b) { 
        //0.32
        //0.42
    }

    public void i2(Transform a, Transform b, Transform c)
    {
        //0.35
    }

    public void i2(Transform a, Transform b, Transform c, Transform d) { 
        //0.38
    }

    public void i6(params Transform[] p) { 
        //1: 0.45
        //2: 0.53
        //3: 0.65
        //4: 0.75
    }

    public Transform i7() {
        //0.28
        return null;
    }

    public Transform i8(Transform a)
    {
        return null;
    }

    public Transform i9(Transform a, Transform b)
    {
        return null;
    }

    public Transform i10(Transform a, Transform b, Transform c)
    {
        return null;
    }

    public Transform i11(Transform a, Transform b, Transform c, Transform d)
    {
        return null;
    }

    public Transform i12(params Transform[] p)
    {
        return null;
    }

    public void oT1(LuaFunction func){
        //nothing: 0.69
        //call: 1.03
        //pushcall: 1.1

        //_lf = func;

        _act = () =>
        {
            func.BeginPCall();
            func.Push(gameObject);
            func.PCall();
            func.EndPCall();
        };
    }

    public void oT2(System.Action func) { 
        //nothing: 2.85
        //call: 3.4
        func();
    }

    public void oT3(TestDlg func) {
        //nothing: 2.8
        //call: 3.5
        //func();
        _dlg = func;
    }

    public void oT4() {
        //0.72
        _dlg(gameObject);
    }

    public void oT5() {
        //0.92
        //_lf.BeginPCall();
        //_lf.Push(gameObject);
        //_lf.PCall();
        //_lf.EndPCall();

        //0.92
        _act();
    }

    public void i13(Transform[] a) { 
        //1: 0.64
        //2: 0.79
        //3: 0.95
    }

    public void i14(Transform[] a, Transform[] b)
    {
        //1, 1: 1
        //1, 2: 1.2
        //2, 2: 1.32
    }

    public void i15(Transform[] a, Transform[] b, Transform[] c)
    {

    }

    public void i16(Transform[] a, Transform[] b, Transform[] c, Transform[] d)
    {

    }


    // 1 param
    public void TestVar(Transform v1) {
        //Debug.Log(v1.name);

        //blank 0.36
    }

    public void TestArr(Transform[] vs) {
        //Debug.Log(vs[0].name);

        //blank 0.86
    }

    public void TestTb(LuaTable[] tb) {
        //float r1 = Time.realtimeSinceStartup;
        //for (int i = 0; i < 1; i++) {
        //    //0.06
        //    //Transform trans = tb.RawGet<int, Transform>(1);
            
        //    //Transform trans = tb["a"] as Transform;
        //    //Debug.Log(tb["a"]);
        //    tb.Dispose();
        //}
        //Debug.Log(Time.realtimeSinceStartup - r1);

        //tb.Dispose();
        //tb = null;

        foreach (LuaTable lt in tb) {
            lt.Dispose();
        }
    }
}
