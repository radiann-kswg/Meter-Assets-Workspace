using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGageDirector : MonoBehaviour
{
    #region Prefab用変数定義
    /// <summary>
    /// 進捗を図示するSlider
    /// </summary>
    [SerializeField]
    private Slider _progressSlider;

    /// <summary>
    /// 進捗値を表示するText
    /// </summary>
    [SerializeField]
    private Text _progressText;
    #endregion

    #region private変数定義
    /// <summary>
    /// 進捗値[%]
    /// </summary>
    private float _percent = 0.0f;

    /// <summary>
    /// 進捗値(0.0~1.0)
    /// </summary>
    private float _progress = 0.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(_progressSlider && _progressText)
        {
            UpdateUIs();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_progressSlider && _progressText)
        {
            UpdateUIs();
        }
    }

    /// <summary>
    /// 進捗値をProgressGageに設定します
    /// </summary>
    /// <param name="value">進捗値(0.0~1.0)</param>
    /// <returns>引数が有効かどうか</returns>
    public bool SetValue(float value)
    {
        if(value < 0.0f || value > 1.0f)
        {
            Debug.Log("[ProgressGage]" +
                "不正な値です、引数valueは0.0以上1.0以下で指定してください");
            return false;
        }
        _percent = value * 100.0f;
        _progress = value;
        return true;
    }

    /// <summary>
    /// 進捗値[%]をProgressGageに設定します
    /// </summary>
    /// <param name="valueByPercent">進捗値[%](0.0~100.0)</param>
    /// <returns>引数が有効かどうか</returns>
    public bool SetPercent(float valueByPercent)
    {
        if (valueByPercent < 0.0f || valueByPercent > 100.0f)
        {
            Debug.Log("[ProgressGage]" +
                "不正な値です、引数valueByPercentは0.0以上1.0未満で指定してください");
            return false;
        }
        _percent = valueByPercent;
        _progress = valueByPercent / 100.0f;
        return true;
    }

    #region private関数
    /// <summary>
    /// UIを更新します
    /// </summary>
    private void UpdateUIs()
    {
        _progressSlider.value = _percent;
        _progressText.text = _progress.ToString("0.0%");
    }
    #endregion
}
