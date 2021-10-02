using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAnimationDirector : MonoBehaviour
{
    public ProgressGageDirector progressGage;

    public LevelGageDirector levelGage;

    public GameObject targetObject;

    public TargetDistanceMeterDirector targetDistanceMeter;

    public RotaryMeterManager rotaryMeter;

    private float _t = 0.0f;

    /// <summary>
    /// アニメーション周期[秒]
    /// </summary>
    public float animationT = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (targetDistanceMeter && targetObject)
        {
            targetDistanceMeter.SetTarget(targetObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RunDemoAnimation();
    }

    private void RunDemoAnimation()
    {
        if (progressGage)
        {
            float progress = _t / animationT;
            progressGage.SetValue(progress);
        }
        if (levelGage)
        {
            float levelValue = Mathf.Sin(_t / animationT * 360.0f * Mathf.Deg2Rad) * 2.5f + 3.5f;
            levelGage.SetLevel(levelValue);
        }
        if (targetObject)
        {
            float positionZ = Mathf.Sin(_t / animationT * 360.0f * Mathf.Deg2Rad) * 13.5f;
            float positionX = Mathf.Sin(_t / animationT * 720.0f * Mathf.Deg2Rad) * 5.0f;
            float positionY = targetObject.transform.position.y;
            targetObject.transform.position = new Vector3(positionX, positionY, positionZ);
        }
        if (rotaryMeter)
        {
            rotaryMeter.Value += Time.deltaTime * (1.0f - _t / animationT);
        }
        _t += Time.deltaTime;
        _t = Mathf.Repeat(_t, animationT);
    }
}
