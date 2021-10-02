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
    private Text targetNameText;

    /// <summary>
    /// ターゲットとの距離を表示するText
    /// </summary>
    [SerializeField]
    private Text targetDistanceText;

    /// <summary>
    /// 照準アイコン
    /// </summary>
    [SerializeField]
    private FocusIconDirector focusIcon;

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
    private const float maxDistanceOfFocusToTarget = 25.0f;

    /// <summary>
    /// 距離値[表示単位]
    /// </summary>
    private float distance = 34.5678f;

    /// <summary>
    /// 距離値[Transform単位]
    /// </summary>
    private float distanceByTransformUnit = maxDistanceOfFocusToTarget * 2.0f;
        
    /// <summary>
    /// ターゲット
    /// </summary>
    private GameObject targetObject;

    /// <summary>
    /// ターゲット名（無指定の場合: gameObject.name）
    /// </summary>
    private string targetName = "";

    /// <summary>
    /// このUIを表示しているCanvas(親オブジェクト)
    /// </summary>
    private Canvas thisCanvas;

    /// <summary>
    /// このUIを投影しているCamera（thisCanvasから参照）
    /// </summary>
    private Camera thisWorldCamera;

    /// <summary>
    /// 接近フラグ（trueでUI表示）
    /// </summary>
    private bool isTargetCloth = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (TransformUnit2DistanceUnit > 0.0f)
        {
            thisCanvas = gameObject.GetComponentInParent<Canvas>();
            thisWorldCamera = thisCanvas.worldCamera;
            if (!thisWorldCamera) thisWorldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            if (thisWorldCamera && targetNameText && targetDistanceText && focusIcon)
            {
                UpdateUIs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TransformUnit2DistanceUnit > 0.0f)
        {
            if (thisWorldCamera && targetNameText && targetDistanceText && focusIcon)
            {
                UpdateUIs();
            }
        }
    }

    /// <summary>
    /// ターゲットをTargetDistanceMeterに設定します
    /// </summary>
    /// <param name="target">ターゲットのGameObject（nullで追従解除）</param>
    /// <param name="name">ターゲットの名前指定（指定なしの場合: target(.gameObject).name）</param>
    public void SetTarget(GameObject target, string name = "")
    {
        targetObject = target;
        focusIcon.TargetTfm = target.transform;
        targetName = name;
    }

    #region private関数
    /// <summary>
    /// UIを更新します
    /// </summary>
    private void UpdateUIs()
    {
        if (targetObject)
        {
            Rect rect = new Rect(0, 0, 1, 1);
            Vector3 vp = thisWorldCamera.WorldToViewportPoint(targetObject.transform.position);
            if (rect.Contains(vp) && vp.z > 0.0f)
            {
                distanceByTransformUnit = Vector3.Distance(
                    thisWorldCamera.gameObject.transform.position, targetObject.transform.position);
                distance = distanceByTransformUnit * TransformUnit2DistanceUnit;
                isTargetCloth = distanceByTransformUnit < maxDistanceOfFocusToTarget;

                if (isTargetCloth)
                {
                    SetUIsActive(true);

                    if (targetName == "") targetNameText.text = targetObject.name;
                    else targetNameText.text = targetName;
                    targetDistanceText.text = distance.ToString("G6") + DistanceUnitName;
                }
                else
                {
                    SetUIsActive(false);
                }
            }
            else
            {
                SetUIsActive(false);
            }
        }
        else
        {
            SetUIsActive(false);
        }
    }

    /// <summary>
    /// UIのgameObject.SetActive関数を一括操作します
    /// </summary>
    /// <param name="isActive">gameObject.SetActive関数の引数</param>
    private void SetUIsActive(bool isActive)
    {
        targetNameText.gameObject.SetActive(isActive);
        targetDistanceText.gameObject.SetActive(isActive);
        focusIcon.gameObject.SetActive(isActive);
    }
    #endregion
}