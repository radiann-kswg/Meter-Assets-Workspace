//参考URL: https://tama-lab.net/2018/08/%E3%80%90unity%E3%80%91%E3%82%AA%E3%83%96%E3%82%B8%E3%82%A7%E3%82%AF%E3%83%88%E3%81%8C%E7%94%BB%E9%9D%A2%E5%A4%96%E3%81%8B%E3%81%A9%E3%81%86%E3%81%8B%E3%82%92%E5%88%A4%E5%AE%9A%E3%81%99%E3%82%8B/ 
// (2021.09.06)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetDistanceMeterDirector : MonoBehaviour
{
    #region Prefab用変数定義
    /// <summary>
    /// ターゲット名を表示するText
    /// </summary>
    [SerializeField]
    private Text _targetNameText;

    /// <summary>
    /// ターゲットとの距離を表示するText
    /// </summary>
    [SerializeField]
    private Text _targetDistanceText;

    /// <summary>
    /// 照準アイコン
    /// </summary>
    [SerializeField]
    private FocusIconDirector _focusIcon;

    /// <summary>
    /// Transform値から距離値への変換係数
    /// </summary>
    public float TransformUnit2DistanceUnit = 1.0f;

    /// <summary>
    /// 距離値の単位表示
    /// </summary>
    public string DistanceUnitName = "m";
    #endregion
    
    #region private変数定義
    /// <summary>
    /// 追従する最大距離値[Transform単位]
    /// </summary>
    [SerializeField]
    private float _maxDistance = 35.0f;

    /// <summary>
    /// 距離値[表示単位]
    /// </summary>
    private float _distance = 34.5678f;

    /// <summary>
    /// 距離値[Transform単位]
    /// </summary>
    private float _distanceByTransformUnit;
        
    /// <summary>
    /// ターゲット
    /// </summary>
    private GameObject _targetObject;

    /// <summary>
    /// ターゲット名（無指定の場合: gameObject.name）
    /// </summary>
    private string _targetName = "";

    /// <summary>
    /// このUIを表示しているCanvas(親オブジェクト)
    /// </summary>
    private Canvas _thisCanvas;

    /// <summary>
    /// このUIを投影しているCamera（thisCanvasから参照）
    /// </summary>
    private Camera _thisWorldCamera;

    /// <summary>
    /// 接近フラグ（trueでUI表示）
    /// </summary>
    private bool _isTargetCloth = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _distance = _maxDistance * 2.0f;

        if (_ReturnFocusFlag())
        {
            _thisCanvas = gameObject.GetComponentInParent<Canvas>();
            _thisWorldCamera = _thisCanvas.worldCamera;
            if (!_thisWorldCamera) _thisWorldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            if (_ReturnUpdateFlag())
            {
                _UpdateUIs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_ReturnFocusFlag())
        {
            if (_ReturnUpdateFlag())
            {
                _UpdateUIs();
            }
        }
    }

    /// <summary>
    /// ターゲットをTargetDistanceMeterに設定します
    /// </summary>
    /// <param name="target">ターゲットのGameObject（nullで追従解除）</param>
    /// <param name="name">ターゲットの名前指定（指定なしの場合: target(.gameObject).name）</param>
    /// <param name="isNullName">ターゲットの名前を表示しない（trueで非表示,デフォルト：false）</param>
    public void SetTarget(GameObject target, string name = "", bool isNullName = false)
    {
        _targetObject = target;
        _focusIcon.TargetTfm = target.transform;
        if (isNullName) _targetName = "";
        else _targetName = name == "" ? target.name : name;
    }

    #region private関数
    /// <summary>
    /// UIを更新します
    /// </summary>
    private void _UpdateUIs()
    {
        if (!_targetObject)
        {
            _SetUIsActive(false);
            return;
        }

        Rect rect = new Rect(0, 0, 1, 1);
        Vector3 vp = _thisWorldCamera.WorldToViewportPoint(_targetObject.transform.position);
        if (!(rect.Contains(vp) && vp.z > 0.0f))
        {
            _SetUIsActive(false);
            return;
        }

        _distanceByTransformUnit = Vector3.Distance(
            _thisWorldCamera.gameObject.transform.position, _targetObject.transform.position);
        _distance = _distanceByTransformUnit * TransformUnit2DistanceUnit;
        _isTargetCloth = _distanceByTransformUnit < _maxDistance;

        if (!_isTargetCloth)
        {
            _SetUIsActive(false);
            return;
        }

        _SetUIsActive(true);

        _targetNameText.text = _targetName;
        _targetDistanceText.text = _distance.ToString("G6") + DistanceUnitName;
    }

    /// <summary>
    /// UIのgameObject.SetActive関数を一括操作します
    /// </summary>
    /// <param name="isActive">gameObject.SetActive関数の引数</param>
    private void _SetUIsActive(bool isActive)
    {
        _targetNameText.gameObject.SetActive(isActive);
        _targetDistanceText.gameObject.SetActive(isActive);
        _focusIcon.gameObject.SetActive(isActive);
    }

    private bool _ReturnFocusFlag()
    {
        return TransformUnit2DistanceUnit > 0.0f;
    }
    private bool _ReturnUpdateFlag()
    {
        return _thisWorldCamera && _targetNameText && _targetDistanceText && _focusIcon;
    }
    #endregion
}