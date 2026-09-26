using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回転式（オドメーター型）メーター。Value に向かって各桁のローターを滑らかに回す。
/// </summary>
public class RotaryMeterManager : MonoBehaviour
{
    /// <summary>ローターのGameObject（要素 0 が 1 の位）</summary>
    [SerializeField] private List<GameObject> roter;

    /// <summary>表示する数値(入力)。0 〜 10^桁数 に丸められる</summary>
    public float Value = 0.0f;

    /// <summary>1 桁ぶんの回転角[deg]（10 分割）</summary>
    private const float DegPerDigit = 36.0f;

    /// <summary>表示している数値(出力)</summary>
    private float _value;

    private float _defaultRotZ = 180.0f;

    void Start()
    {
        if (roter != null && roter.Count > 0) _defaultRotZ = roter[0].transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        if (roter == null || roter.Count == 0) return;
        float maxValue = Mathf.Pow(10.0f, roter.Count);
        Value = Mathf.Clamp(Value, 0.0f, maxValue);
        if (Mathf.Approximately(Value, _value)) return;

        // ponytail: フレームレート依存の指数追従（旧実装と同じ挙動）。厳密な速度が要るなら MoveTowards に変える
        _value = Mathf.Clamp(_value + (Value - _value) * Time.deltaTime, 0.0f, maxValue);
        for (int i = 0; i < roter.Count; ++i)
        {
            float rotZ = -DigitPosition(_value, i) * DegPerDigit - _defaultRotZ;
            roter[i].transform.rotation = Quaternion.Euler(180.0f, -90.0f, rotZ);
        }
    }

    /// <summary>
    /// 桁 <paramref name="digit"/>（0 = 1 の位）のローター位置を 0〜10 で返します。
    /// 下位桁がすべて 9 のあいだは繰り上がりに合わせて次の数字へ滑らかに進みます。
    /// </summary>
    public static float DigitPosition(float value, int digit)
    {
        float unit = Mathf.Pow(10.0f, digit);
        float shown = Mathf.Repeat(Mathf.Floor(value / unit), 10.0f);
        float carry = Mathf.Repeat(value, unit) - (unit - 1.0f);
        return carry >= 0.0f ? shown + carry : shown;
    }
}
