using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAid : MonoBehaviour {
    public bool isRecycle;

    private Slider _sld;
    private float _target;
    private float _recoard;
    private bool _grading;

    private void Awake()
    {
        _sld = GetComponent<Slider>();
    }

    private void Update()
    {
        if (_grading)
        {
            float delta = 0.01f;
            if (_recoard + 0.01f >= _target) {
                delta = _target - _recoard;
                _grading = false;
            }
            _recoard += delta;

            float value = (_sld.value + delta) % 1;
            if (AlmostEqual(value, 0)) {
                value = 1;
            }
            _sld.value = value;
        }
    }

    public void SetValue(float val, bool isGrad) {
        if (isGrad)
        {
            _target = val;
            _recoard = _sld.value;
            _grading = true;
        }
        else {
            float v = val % 1;
            if (AlmostEqual(v, 0)) {
                v = AlmostEqual(val, 0) ? 0 : 1;
            }
            _sld.value = v;
        }
    }

    public void AddValue(float val, bool isGrad) {
        if (AlmostEqual(val, 0)) {
            return;
        }

        if (isGrad)
        {
            if (_grading)
            {
                _target += val;
            }
            else {
                _target = val + _sld.value;
                _recoard = _sld.value;
                _grading = true;
            }
        }
        else {
            float v = (_sld.value + val) % 1;
            if (AlmostEqual(v, 0)) {
                v = 1;
            }
            _sld.value = v;
        }
    }

    public static float PRECISION = 0.00001f;
    public static bool AlmostEqual(float f1, float f2)
    {
        return Mathf.Abs(f1 - f2) <= PRECISION;
    }
}
