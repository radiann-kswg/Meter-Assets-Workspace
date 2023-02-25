using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAnimationDirector : MonoBehaviour
{
    [SerializeField]
    private ProgressGageDirector _progressGage;
    [SerializeField]
    private LevelGageDirector _levelGage;
    [SerializeField]
    private GameObject _targetObject;
    [SerializeField]
    private TargetDistanceMeterDirector _targetDistanceMeter;
    [SerializeField]
    private RotaryMeterManager _rotaryMeter;

    private float _t = 0.0f;

    /// <summary>
    /// アニメーション周期[秒]
    /// </summary>
    public float AnimationT = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (_targetDistanceMeter && _targetObject)
        {
            _targetDistanceMeter.SetTarget(_targetObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _RunDemoAnimation();
    }

    private void _RunDemoAnimation()
    {
        if (_progressGage)
        {
            float progress = _t / AnimationT;
            _progressGage.SetValue(progress);
        }
        if (_levelGage)
        {
            float levelValue = Mathf.Sin(_t / AnimationT * 360.0f * Mathf.Deg2Rad) * 2.5f + 3.5f;
            _levelGage.SetLevel(levelValue);
        }
        if (_targetObject)
        {
            float positionZ = Mathf.Sin(_t / AnimationT * 360.0f * Mathf.Deg2Rad) * 13.5f;
            float positionX = Mathf.Sin(_t / AnimationT * 720.0f * Mathf.Deg2Rad) * 5.0f;
            float positionY = _targetObject.transform.position.y;
            _targetObject.transform.position = new Vector3(positionX, positionY, positionZ);
        }
        if (_rotaryMeter)
        {
            float valanp = 3450000f;
            _rotaryMeter.Value = Mathf.Sin(_t / AnimationT * 2.0f * Mathf.PI) * valanp + valanp;
        }
        _t += Time.deltaTime;
        _t = Mathf.Repeat(_t, AnimationT);
    }
}
