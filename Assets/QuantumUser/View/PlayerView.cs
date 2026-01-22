using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;

public unsafe class PlayerView : QuantumEntityViewComponent
{
    [SerializeField] private Animator animator;

    private static readonly int MoveXHash = Animator.StringToHash("moveX");
    private static readonly int MoveZHash = Animator.StringToHash("moveZ");

    public override void OnUpdateView()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        var input = PredictedFrame.GetPlayerInput(PredictedFrame.Get<PlayerLink>(EntityRef).Player);
        var kcc = PredictedFrame.Get<KCC>(EntityRef);
        var velocity = kcc.Velocity;

        animator.SetFloat(MoveXHash, velocity.X.AsFloat);
        animator.SetFloat(MoveZHash, velocity.Y.AsFloat);
    }
}
