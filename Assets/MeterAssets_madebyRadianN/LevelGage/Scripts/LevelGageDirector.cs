using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// レベルゲージ。整数部をレベル、小数部をゲージの充填率として表示する。
/// </summary>
public class LevelGageDirector : MonoBehaviour
{
    /// <summary>レベルゲージを図示するImage</summary>
    [SerializeField] private Image _gageImage;

    /// <summary>レベルゲージの背部Image</summary>
    [SerializeField] private Image _backgroundImage;

    /// <summary>レベルを表示するText</summary>
    [SerializeField] private TextMeshProUGUI _levelText;

    /// <summary>レベルごとに表示するゲージの色（要素数 - 1 が最大レベル）</summary>
    [SerializeField] private Color32[] _gageColorList;

    private int _level;
    private float _gageProgress;

    void Start()
    {
        UpdateUIs();
    }

    /// <summary>
    /// レベル値をLevelGageに設定します
    /// </summary>
    /// <param name="value">レベル値（0.0以上。整数部がレベル、小数部がゲージ）</param>
    /// <returns>引数が有効かどうか</returns>
    public bool SetLevel(float value)
    {
        if (value < 0.0f)
        {
            Debug.LogWarning("[LevelGage] 不正な値です、引数valueは0.0以上で指定してください");
            return false;
        }
        (_level, _gageProgress) = Split(value, MaxLevel);
        UpdateUIs();
        return true;
    }

    /// <summary>設定できる最大レベル（色リストの最終要素）。色リスト未設定なら 0</summary>
    public int MaxLevel => _gageColorList == null ? 0 : Mathf.Max(0, _gageColorList.Length - 1);

    /// <summary>
    /// レベル値をレベル（整数部）とゲージ充填率（小数部）に分けます。最大レベル到達時はゲージ満タン。
    /// </summary>
    public static (int level, float progress) Split(float value, int maxLevel)
    {
        int level = Mathf.FloorToInt(value);
        if (level >= maxLevel) return (maxLevel, 1.0f);
        return (level, value - level);
    }

    private void UpdateUIs()
    {
        if (_gageColorList == null || _gageColorList.Length < 2 || !_gageImage || !_backgroundImage || !_levelText) return;
        _gageImage.color = _gageColorList[Mathf.Min(_level + 1, MaxLevel)];
        _backgroundImage.color = _gageColorList[_level];
        _gageImage.fillAmount = _gageProgress;
        _levelText.text = _level.ToString();
    }
}
