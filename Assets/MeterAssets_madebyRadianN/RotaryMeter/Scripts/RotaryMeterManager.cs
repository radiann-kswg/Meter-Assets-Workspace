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

    private float _defaultRotZ = 180.0f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _defaultRotZ = roter[0].transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        float maxValue = Mathf.Pow(10.0f, roter.Count);

        if (Value < 0) Value = 0;
        if (_value < 0) _value = 0;
        if (Value >= maxValue) Value = maxValue;
        if (_value >= maxValue) _value = maxValue;

        float rotation = Value - _value;
        if ((_value <= 0 && rotation < 0) || (_value >= maxValue && rotation > 0))
        {
            for(int i = 0; i < roter.Count; ++i)
            {
                roter[i].transform.rotation = Quaternion.Euler(180.0f, -90.0f, _defaultRotZ);
            }
        }
        else if (rotation != 0)
        {
            float digitRotation = -36.0f * Time.deltaTime * rotation;
            _value -= digitRotation / 36.0f;
            float rotZ = -Mathf.Repeat(_value, 10) * 36.0f - _defaultRotZ;
            roter[0].transform.rotation = Quaternion.Euler(180.0f, -90.0f, rotZ);
            for (int i = 1; i < roter.Count; ++i)
            {
                float digitVal = Mathf.Repeat(_value, Mathf.Pow(10, i)) - (Mathf.Pow(10, i) - 1.0f);
                if (digitVal >= 0)
                {
                    rotZ = -(digitVal + Mathf.Repeat(Mathf.Floor(_value / Mathf.Pow(10, i)), 10)) * 36.0f - _defaultRotZ;
                }
                else
                {
                    rotZ = -Mathf.Repeat(Mathf.Floor(_value / Mathf.Pow(10, i)), 10) * 36.0f - _defaultRotZ;
                }
                roter[i].transform.rotation = Quaternion.Euler(180.0f, -90.0f, rotZ);
            }
        }
    }
}
