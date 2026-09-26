// 参考URL: https://tama-lab.net/2018/08/%E3%80%90unity%E3%80%91%E3%82%AA%E3%83%96%E3%82%B8%E3%82%A7%E3%82%AF%E3%83%88%E3%81%8C%E7%94%BB%E9%9D%A2%E5%A4%96%E3%81%8B%E3%81%A9%E3%81%86%E3%81%8B%E3%82%92%E5%88%A4%E5%AE%9A%E3%81%99%E3%82%8B/
// (2021.09.06)

using UnityEngine;
using TMPro;

/// <summary>
/// ターゲット距離メーター。画面内かつ最大距離以内のターゲットに名前・距離・照準を出す。
/// </summary>
public class TargetDistanceMeterDirector : MonoBehaviour
{
    /// <summary>ターゲット名を表示するText</summary>
    [SerializeField] private TextMeshProUGUI _targetNameText;

    /// <summary>ターゲットとの距離を表示するText</summary>
    [SerializeField] private TextMeshProUGUI _targetDistanceText;

    /// <summary>照準アイコン</summary>
    [SerializeField] private FocusIconDirector _focusIcon;

    /// <summary>追従する最大距離値[Transform単位]</summary>
    [SerializeField] private float _maxDistance = 35.0f;

    /// <summary>Transform値から距離値への変換係数（0 以下でメーター停止）</summary>
    public float TransformUnit2DistanceUnit = 1.0f;

    /// <summary>距離値の単位表示</summary>
    public string DistanceUnitName = "m";

    private GameObject _targetObject;
    private string _targetName = "";
    private Camera _thisWorldCamera;

    void Start()
    {
        var canvas = GetComponentInParent<Canvas>();
        _thisWorldCamera = canvas ? canvas.worldCamera : null;
        if (!_thisWorldCamera) _thisWorldCamera = Camera.main;
        UpdateUIs();
    }

    void Update()
    {
        UpdateUIs();
    }

    /// <summary>
    /// ターゲットをTargetDistanceMeterに設定します
    /// </summary>
    /// <param name="target">ターゲットのGameObject（nullで追従解除）</param>
    /// <param name="name">ターゲットの名前指定（指定なしの場合: target.name）</param>
    /// <param name="isNullName">ターゲットの名前を表示しない（trueで非表示,デフォルト：false）</param>
    public void SetTarget(GameObject target, string name = "", bool isNullName = false)
    {
        _targetObject = target;
        if (_focusIcon) _focusIcon.TargetTfm = target ? target.transform : null;
        _targetName = isNullName || !target ? "" : (name == "" ? target.name : name);
    }

    /// <summary>距離の表示文字列（有効数字 6 桁＋単位）</summary>
    public static string FormatDistance(float distance, string unitName) => distance.ToString("G6") + unitName;

    /// <summary>ビューポート座標が画面内（カメラ前方）かどうか</summary>
    public static bool IsInView(Vector3 viewportPoint) =>
        viewportPoint.z > 0.0f && viewportPoint.x >= 0.0f && viewportPoint.x <= 1.0f && viewportPoint.y >= 0.0f && viewportPoint.y <= 1.0f;

    private void UpdateUIs()
    {
        if (TransformUnit2DistanceUnit <= 0.0f || !_thisWorldCamera || !_targetNameText || !_targetDistanceText || !_focusIcon) return;

        bool visible = _targetObject
            && IsInView(_thisWorldCamera.WorldToViewportPoint(_targetObject.transform.position));
        float distance = visible ? Vector3.Distance(_thisWorldCamera.transform.position, _targetObject.transform.position) : float.MaxValue;
        visible &= distance < _maxDistance;

        _targetNameText.gameObject.SetActive(visible);
        _targetDistanceText.gameObject.SetActive(visible);
        _focusIcon.gameObject.SetActive(visible);
        if (!visible) return;

        _targetNameText.text = _targetName;
        _targetDistanceText.text = FormatDistance(distance * TransformUnit2DistanceUnit, DistanceUnitName);
    }
}
