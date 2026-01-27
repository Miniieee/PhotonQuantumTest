using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Quantum {
public unsafe partial struct ShrinkingCircleState
    {
        public void EnterState(ShrinkingCircle* shrinkingCircle)
        {
            shrinkingCircle->CurrentTimeToNextState = TimeToNextState;
        }

        public void UpdateState(Frame f, ShrinkingCircle* shrinkingCircle)
        {
            if(shrinkingCircle->CurrentTimeToNextState <= 0)
                return;

            shrinkingCircle->CurrentTimeToNextState -= f.DeltaTime;
        }
    }
}
