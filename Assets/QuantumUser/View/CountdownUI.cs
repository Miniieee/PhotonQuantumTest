using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : QuantumSceneViewComponent
{
    [SerializeField] TMP_Text timeRemainingText;
    [SerializeField] Image timeProgressImage;

    public override void OnLateUpdateView()
    {
        var f = PredictedFrame;
        var shrinkingCircle = f.GetSingleton<ShrinkingCircle>();
        var time = shrinkingCircle.CurrentTimeToNextState;
        var currentState = shrinkingCircle.CurrentState;

        timeRemainingText.text = time.AsFloat.ToString("F2", CultureInfo.InvariantCulture);
        timeProgressImage.fillAmount = 1 - (time.AsFloat / currentState.TimeToNextState.AsFloat);
    }
}
