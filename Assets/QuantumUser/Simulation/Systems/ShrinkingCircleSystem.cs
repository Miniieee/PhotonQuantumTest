using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class ShrinkingCircleSystem : SystemMainThread, ISignalOnComponentAdded<ShrinkingCircle>
    {
        public override void Update(Frame f)
        {
            ShrinkingCircle* shrinkingCircle = f.Unsafe.GetPointerSingleton<ShrinkingCircle>();
            ShrinkingCircleConfig config = f.FindAsset(shrinkingCircle->ShrinkingCircleConfig);
            shrinkingCircle->CurrentState.UpdateState(f, shrinkingCircle);

            if (shrinkingCircle->CurrentTimeToNextState <= 0)
            {
                if (shrinkingCircle->CurrentStateIndex >= config.States.Length - 1)
                    return;

                shrinkingCircle->CurrentStateIndex++;
                config.States[shrinkingCircle->CurrentStateIndex].Materialize(f, ref shrinkingCircle->CurrentState);
                shrinkingCircle->CurrentState.EnterState(shrinkingCircle);
                f.Events.OnShrinkingCircleChanged();
            }
        }

        public void OnAdded(Frame f, EntityRef entity, ShrinkingCircle* shrinkingCircle)
        {
            shrinkingCircle->CurrentStateIndex = 0;
            ShrinkingCircleConfig config = f.FindAsset(shrinkingCircle->ShrinkingCircleConfig);
            config.States[0].Materialize(f, ref shrinkingCircle->CurrentState);
            shrinkingCircle->CurrentState.EnterState(shrinkingCircle);

            Transform2D* transform = f.Unsafe.GetPointer<Transform2D>(entity);
            FP randomx = f.RNG->Next(config.MinimumBounds.X, config.MaximumBounds.X);
            FP randomy = f.RNG->Next(config.MinimumBounds.Y, config.MaximumBounds.Y);
            FPVector2 pos = new FPVector2(randomx, randomy);
            transform->Position = pos;
            shrinkingCircle->Position = pos;
        }
    }
}
