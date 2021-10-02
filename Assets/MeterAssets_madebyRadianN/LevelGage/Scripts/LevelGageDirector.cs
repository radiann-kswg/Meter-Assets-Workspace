using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGageDirector : MonoBehaviour
{
    #region Prefab用変数定義
    /// <summary>
    /// レベルゲージを図示するImage
    /// </summary>
    [SerializeField]
    private Image gageImage;

    /// <summary>
    /// レベルゲージの背部Image
    /// </summary>
    [SerializeField]
    private Image backgroundImage;

    /// <summary>
    /// レベルを表示するText
    /// </summary>
    [SerializeField]
    private Text levelText;

    /// <summary>
    /// レベルごとに表示するゲージの色
    /// </summary>
    [SerializeField]
    private Color32[] gageColorList;
    #endregion

    #region private変数定義
    /// <summary>
    /// レベルゲージ値(レベルの小数部)
    /// </summary>
    private float _gageProgress = 0.0f;

    /// <summary>
    /// レベル(整数部)
    /// </summary>
    private int _level = 0;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if(gageColorList != null) {
            if (gageColorList.Length > 1 && gageImage && levelText)
            {
                UpdateUIs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gageColorList != null)
        {
            if (gageColorList.Length > 1 && gageImage && levelText)
            {
                UpdateUIs();
            }
        }
    }

    /// <summary>
    /// レベル値をLevelGageに設定します
    /// </summary>
    /// <param name="value">レベル値</param>
    /// <returns>引数が有効かどうか</returns>
    public bool SetLevel(float value)
    {
        if (value < 0.0f)
        {
            Debug.Log("[LevelGage]" +
                "不正な値です、引数valueは0.0以上で指定してください");
            return false;
        }
        _gageProgress = Mathf.Repeat(value, 1.0f);
        _level = Mathf.FloorToInt(value);
        return true;
    }

    #region private関数
    /// <summary>
    /// UIを更新します
    /// </summary>
    private void UpdateUIs()
    {
        if(_level >= gageColorList.Length - 1)
        {
            _level = gageColorList.Length - 1;
            _gageProgress = 100.0f;
            gageImage.color = gageColorList[_level];
        }
        else
        {
            gageImage.color = gageColorList[_level + 1];
        }
        backgroundImage.color = gageColorList[_level];
        gageImage.fillAmount = _gageProgress;
        levelText.text = _level.ToString();
    }
    #endregion
}
