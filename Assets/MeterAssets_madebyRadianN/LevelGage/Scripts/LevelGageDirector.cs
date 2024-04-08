using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGageDirector : MonoBehaviour
{
    #region Prefab用変数定義
    /// <summary>
    /// レベルゲージを図示するImage
    /// </summary>
    [SerializeField]
    private Image _gageImage;

    /// <summary>
    /// レベルゲージの背部Image
    /// </summary>
    [SerializeField]
    private Image _backgroundImage;

    /// <summary>
    /// レベルを表示するText
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI _levelText;

    /// <summary>
    /// レベルごとに表示するゲージの色
    /// </summary>
    [SerializeField]
    private Color32[] _gageColorList;
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
        if(_gageColorList != null) {
            if (_gageColorList.Length > 1 && _gageImage && _levelText)
            {
                _UpdateUIs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gageColorList != null)
        {
            if (_gageColorList.Length > 1 && _gageImage && _levelText)
            {
                _UpdateUIs();
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
    private void _UpdateUIs()
    {
        if(_level >= _gageColorList.Length - 1)
        {
            _level = _gageColorList.Length - 1;
            _gageProgress = 100.0f;
            _gageImage.color = _gageColorList[_level];
        }
        else
        {
            _gageImage.color = _gageColorList[_level + 1];
        }
        _backgroundImage.color = _gageColorList[_level];
        _gageImage.fillAmount = _gageProgress;
        _levelText.text = _level.ToString();
    }
    #endregion
}
