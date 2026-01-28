using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Quantum;
using UnityEngine;

namespace Quantum
{
  public class ShrinkingCircleView : QuantumEntityViewComponent
  {
    [SerializeField] private Transform redCircle, whiteCircle;

    public override void OnActivate(Frame frame)
    {
      QuantumEvent.Subscribe<EventOnShrinkingCircleChanged>(this, OnShrinkingCircleChanged);
      var shrinkingCircle = frame.GetSingleton<ShrinkingCircle>();
      whiteCircle.localScale = redCircle.localScale = new Vector3(shrinkingCircle.CurrentRadius.AsFloat, shrinkingCircle.CurrentRadius.AsFloat);
      redCircle.gameObject.SetActive(false);
    }

    private void OnShrinkingCircleChanged(EventOnShrinkingCircleChanged callback)
    {
      var shrinkingCircle = VerifiedFrame.GetSingleton<ShrinkingCircle>();
      var currentState = shrinkingCircle.CurrentState.CircleStateUnion.Field;
      redCircle.gameObject.SetActive(currentState is CircleStateUnion.SHRINKSTATE or CircleStateUnion.PRESHRINKSTATE);

      if (currentState == CircleStateUnion.PRESHRINKSTATE)
      {
        whiteCircle.DOScale(new Vector3(shrinkingCircle.TargetRadius.AsFloat, shrinkingCircle.TargetRadius.AsFloat), 1f);
      }
    }

    public override void OnLateUpdateView()
    {
      var shrinkingCircle = PredictedFrame.GetSingleton<ShrinkingCircle>();
      if (shrinkingCircle.CurrentState.CircleStateUnion.Field != CircleStateUnion.SHRINKSTATE)
        return;

      redCircle.localScale = new Vector3(shrinkingCircle.CurrentRadius.AsFloat, shrinkingCircle.CurrentRadius.AsFloat);
    }

    public override void OnDeactivate()
    {
      QuantumEvent.UnsubscribeListener<EventOnShrinkingCircleChanged>(this);
    }
  }
}