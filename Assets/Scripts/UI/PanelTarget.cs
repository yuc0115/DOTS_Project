using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelTarget : MonoBehaviour
{
    [SerializeField] private Image _targetImg = null;
    [SerializeField] private Camera _uiCam = null;
    [SerializeField] private float _targetImgDist = 200;

    private void LateUpdate()
    {
        Vector3 vCenter = _uiCam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        Vector3 vMouse = _uiCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 vc = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        Vector3 vm = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 v = (vMouse - vCenter).normalized;
        v.z = 0;
        _targetImg.transform.localPosition = v * _targetImgDist;
    }
}
