using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProxy : MonoBehaviour {
    public static GameProxy instance { get; set; }

    private List<IGeneral> _ctrlList;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
        _ctrlList = new List<IGeneral>();

        _ctrlList.Add(new LuaRun());
        _ctrlList.Add(new RunInfo());

        ObjectCache objCache = new ObjectCache();
        ResourceCtrl resCtrl  = new ResourceCtrl();
        UICtrl uiCtrl = new UICtrl();

        objCache.NewPtr(gameObject);

        _Init();
    }

    void Start()
    {
        _Start();
    }

    void Update()
    {
        _Update();
    }

    void OnApplicationQuit()
    {
        _Close();
    }

    private void _Init()
    {
        for (int i = 0, max = _ctrlList.Count; i < max; i++) {
            _ctrlList[i]._Init(gameObject);
        }
    }

    private void _Start()
    {
        for (int i = 0, max = _ctrlList.Count; i < max; i++)
        {
            _ctrlList[i]._Start();
        }
    }

    private void _Update()
    {
        for (int i = 0, max = _ctrlList.Count; i < max; i++)
        {
            _ctrlList[i]._Update();
        }
    }

    private void _Close()
    {
        for (int i = 0, max = _ctrlList.Count; i < max; i++)
        {
            _ctrlList[i]._Close();
        }
    }

    //public void Reload() {
    //    _Close();
    //    _Init();
    //    _Start();
    //}

    public void StartCo(IEnumerator func) {
        StartCoroutine(func);
    }
}
