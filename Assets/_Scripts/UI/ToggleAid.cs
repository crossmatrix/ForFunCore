using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleAid : MonoBehaviour {
    public GameObject hlPart, dimPart;

    private void Awake()
    {
        Toggle tog = gameObject.GetComponent<Toggle>();
        tog.onValueChanged.AddListener(OnTogValChanged);
    }

    private void OnTogValChanged(bool state) {
        if (hlPart != null) {
            hlPart.SetActive(state);
        }
        if (dimPart != null) {
            dimPart.SetActive(!state);
        }
    }
}
