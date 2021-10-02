using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotaryMeterManager : MonoBehaviour
{
    #region Prefab用変数定義
    /// <summary>
    /// ローターのGameObject
    /// </summary>
    [SerializeField]
    private List<GameObject> roter;

    /// <summary>
    /// 表示する数値(入力)
    /// </summary>
    public float Value = 0.0f;
    #endregion

    #region private変数定義
    /// <summary>
    /// 表示している数値(出力)
    /// </summary>
    private float _value = 0.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float maxValue = Mathf.Pow(10.0f, roter.Count);

        if (Value < 0) Value = 0;
        if (Value < 0) Value = 0;
        if (_value >= maxValue) _value = maxValue;
        if (_value >= maxValue) _value = maxValue;

        float rotation = Value - _value;
        if (_value <= 0 && rotation < 0) ;
        else if (_value >= maxValue && rotation > 0) ;
        else if (rotation != 0)
        {
            float digitRotation = -36.0f * Time.deltaTime * Mathf.Sign(rotation) * rotation;
            roter[0].transform.Rotate(0.0f, 0.0f, digitRotation);
            for (int i = 1; i < roter.Count; ++i)
            {
                if ((Mathf.Repeat(_value, Mathf.Pow(10.0f, i)) >= Mathf.Pow(10.0f, i) - 1.0f))
                {
                    roter[i].transform.Rotate(0.0f, 0.0f, digitRotation);
                }
                else break;
            }
            _value -= digitRotation / 36.0f * Mathf.Sign(rotation);
        }
    }
}
