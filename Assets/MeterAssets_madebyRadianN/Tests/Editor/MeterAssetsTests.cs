using NUnit.Framework;
using UnityEngine;

/// <summary>メーター各種の純粋な計算部分の EditMode テスト（Test Runner > EditMode）</summary>
public class MeterAssetsTests
{
    [Test]
    public void LevelGage_Split_整数部と小数部に分ける()
    {
        Assert.AreEqual((3, 0.25f), LevelGageDirector.Split(3.25f, 5));
        Assert.AreEqual((0, 0f), LevelGageDirector.Split(0f, 5));
    }

    [Test]
    public void LevelGage_Split_最大レベル到達でゲージ満タン()
    {
        Assert.AreEqual((5, 1f), LevelGageDirector.Split(5.5f, 5));
        Assert.AreEqual((5, 1f), LevelGageDirector.Split(99f, 5));
    }

    [Test]
    public void LevelGage_SetLevel_負の値は拒否()
    {
        var go = new GameObject();
        try { Assert.IsFalse(go.AddComponent<LevelGageDirector>().SetLevel(-1f)); }
        finally { Object.DestroyImmediate(go); }
    }

    [Test]
    public void ProgressGage_SetPercent_範囲外は拒否し範囲内は進捗に変換()
    {
        var go = new GameObject();
        try
        {
            var gage = go.AddComponent<ProgressGageDirector>();
            Assert.IsFalse(gage.SetPercent(100.5f));
            Assert.IsFalse(gage.SetValue(-0.1f));
            Assert.IsTrue(gage.SetPercent(50f));
            Assert.AreEqual(0.5f, gage.Progress, 1e-6f);
        }
        finally { Object.DestroyImmediate(go); }
    }

    [Test]
    public void RotaryMeter_DigitPosition_1の位はそのまま()
    {
        Assert.AreEqual(7f, RotaryMeterManager.DigitPosition(7f, 0), 1e-5f);
        Assert.AreEqual(2.3f, RotaryMeterManager.DigitPosition(12.3f, 0), 1e-5f);
    }

    [Test]
    public void RotaryMeter_DigitPosition_上位桁は下位が9のときだけ繰り上がり途中になる()
    {
        Assert.AreEqual(1f, RotaryMeterManager.DigitPosition(12.3f, 1), 1e-5f);   // 十の位: 1 で静止
        Assert.AreEqual(1.5f, RotaryMeterManager.DigitPosition(19.5f, 1), 1e-5f); // 十の位: 1→2 の半分
        Assert.AreEqual(0.5f, RotaryMeterManager.DigitPosition(99.5f, 2), 1e-5f); // 百の位: 0→1 の半分
    }

    [Test]
    public void TargetDistance_FormatDistance_有効数字6桁と単位()
    {
        Assert.AreEqual("34.5678m", TargetDistanceMeterDirector.FormatDistance(34.5678f, "m"));
        Assert.AreEqual("1234.57km", TargetDistanceMeterDirector.FormatDistance(1234.5678f, "km"));
    }

    [Test]
    public void TargetDistance_IsInView_画面内かつ前方だけ()
    {
        Assert.IsTrue(TargetDistanceMeterDirector.IsInView(new Vector3(0.5f, 0.5f, 10f)));
        Assert.IsFalse(TargetDistanceMeterDirector.IsInView(new Vector3(0.5f, 0.5f, -1f)));
        Assert.IsFalse(TargetDistanceMeterDirector.IsInView(new Vector3(1.2f, 0.5f, 10f)));
    }

    [Test]
    public void TargetDistance_SetTarget_nullで追従解除しても落ちない()
    {
        var go = new GameObject();
        try { Assert.DoesNotThrow(() => go.AddComponent<TargetDistanceMeterDirector>().SetTarget(null)); }
        finally { Object.DestroyImmediate(go); }
    }
}
