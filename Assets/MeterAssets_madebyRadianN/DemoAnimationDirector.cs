using UnityEngine;

/// <summary>
/// デモシーン用。各メーターに周期 AnimationT の値を流し込む。
/// </summary>
public class DemoAnimationDirector : MonoBehaviour
{
    [SerializeField] private ProgressGageDirector _progressGage;
    [SerializeField] private LevelGageDirector _levelGage;
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private TargetDistanceMeterDirector _targetDistanceMeter;
    [SerializeField] private RotaryMeterManager _rotaryMeter;

    /// <summary>アニメーション周期[秒]</summary>
    public float AnimationT = 2.5f;

    private float _t;

    void Start()
    {
        if (_targetDistanceMeter && _targetObject) _targetDistanceMeter.SetTarget(_targetObject);
    }

    void Update()
    {
        float phase = _t / AnimationT * 2.0f * Mathf.PI;
        if (_progressGage) _progressGage.SetValue(_t / AnimationT);
        if (_levelGage) _levelGage.SetLevel(Mathf.Sin(phase) * 2.5f + 3.5f);
        if (_targetObject)
        {
            var p = _targetObject.transform.position;
            _targetObject.transform.position = new Vector3(Mathf.Sin(phase * 2.0f) * 5.0f, p.y, Mathf.Sin(phase) * 13.5f);
        }
        if (_rotaryMeter)
        {
            const float amplitude = 3450000f;
            _rotaryMeter.Value = Mathf.Sin(phase) * amplitude + amplitude;
        }
        _t = Mathf.Repeat(_t + Time.deltaTime, AnimationT);
    }
}
