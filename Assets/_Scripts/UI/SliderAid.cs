using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAid : MonoBehaviour {
    const float GRAD_VAL = 0.01f;
    const float PRECISION = 1e-05f;

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
            
            if (_recoard + GRAD_VAL >= _target)
            {
                _grading = false;
                _recoard = _target;
                DoSetSlider(_target);
            }
            else {
                _recoard += GRAD_VAL;
                DoSetSlider(_sld.value + GRAD_VAL);
            }
        }
    }

    public void SetValue(float val, bool isGrad) {
        if (AlmostEqual(val, 0)) {
            _sld.value = 0;
            return;
        }

        if (isGrad)
        {
            _target = val;
            _recoard = _sld.value;
            _grading = true;
        }
        else {
            DoSetSlider(val);
        }
    }

    public static bool AlmostEqual(float f1, float f2)
    {
        return Mathf.Abs(f1 - f2) <= PRECISION;
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
            DoSetSlider(_sld.value + val);
        }
    }

    private void DoSetSlider(float val) {
        float v = val % 1;
        if (AlmostEqual(v, 0)) {
            v = 1;
        }
        _sld.value = v;
    }
}
