// 引用URL: https://tech.pjin.jp/blog/2017/07/14/unity_ugui_sync_rendermode/ (2021.09.06)

using UnityEngine;

/// <summary>
/// 照準アイコン。TargetTfm のワールド座標をスクリーン座標へ写して追従する。
/// </summary>
public class FocusIconDirector : MonoBehaviour
{
    /// <summary>追従先（null で停止）</summary>
    public Transform TargetTfm;

    private RectTransform _myRectTfm;

    void Start()
    {
        _myRectTfm = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!TargetTfm || !Camera.main) return;
        _myRectTfm.position = RectTransformUtility.WorldToScreenPoint(Camera.main, TargetTfm.position);
    }
}
