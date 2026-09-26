using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 進捗ゲージ。0.0〜1.0 の進捗値をスライダーと百分率テキストで表示する。
/// </summary>
public class ProgressGageDirector : MonoBehaviour
{
    /// <summary>進捗を図示するSlider（Max Value = 100 前提）</summary>
    [SerializeField] private Slider _progressSlider;

    /// <summary>進捗値を表示するText</summary>
    [SerializeField] private TextMeshProUGUI _progressText;

    /// <summary>進捗値(0.0~1.0)</summary>
    public float Progress { get; private set; }

    void Start()
    {
        UpdateUIs();
    }

    /// <summary>
    /// 進捗値をProgressGageに設定します
    /// </summary>
    /// <param name="value">進捗値(0.0~1.0)</param>
    /// <returns>引数が有効かどうか</returns>
    public bool SetValue(float value)
    {
        if (value < 0.0f || value > 1.0f)
        {
            Debug.LogWarning("[ProgressGage] 不正な値です、引数valueは0.0以上1.0以下で指定してください");
            return false;
        }
        Progress = value;
        UpdateUIs();
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
            Debug.LogWarning("[ProgressGage] 不正な値です、引数valueByPercentは0.0以上100.0以下で指定してください");
            return false;
        }
        return SetValue(valueByPercent / 100.0f);
    }

    private void UpdateUIs()
    {
        if (!_progressSlider || !_progressText) return;
        _progressSlider.value = Progress * 100.0f;
        _progressText.text = Progress.ToString("0.0%");
    }
}
