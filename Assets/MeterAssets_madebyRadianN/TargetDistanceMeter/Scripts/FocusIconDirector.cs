// 引用URL: https://tech.pjin.jp/blog/2017/07/14/unity_ugui_sync_rendermode/ (2021.09.06)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusIconDirector : MonoBehaviour
{
    /*[SerializeField]
    private Transform targetTfm;*/
    public Transform TargetTfm;

    private RectTransform _myRectTfm;
    private Vector3 _offset = new Vector3(0, 0, 0);

    void Start()
    {
        //myRectTfm = GetComponent<RectTransform>();
        _myRectTfm = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (TargetTfm)
        {
            _myRectTfm.position
                = RectTransformUtility.WorldToScreenPoint(Camera.main, TargetTfm.position + _offset);
        }
    }
}