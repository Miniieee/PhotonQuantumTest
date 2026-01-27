using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using Quantum.Prototypes;
using UnityEngine;

public class ShrinkingCircleConfig : AssetObject
{
    public ShrinkingCircleStatePrototype[] States;
    public FP DamageDealingPerSecond;
    public FPVector2 MinimumBounds, MaximumBounds;
}
