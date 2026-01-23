namespace Quantum
{
  using Photon.Deterministic;
  using UnityEngine.Scripting;

  [Preserve]
  public unsafe class PickupSystem : SystemMainThreadFilter<PickupSystem.Filter>, ISignalOnTriggerEnter2D, ISignalOnTriggerExit2D
  {
    public void OnTriggerEnter2D(Frame f, TriggerInfo2D info)
    {
        if (!f.TryGet(info.Entity, out PlayerLink playerLink))
          return;
        if (!f.TryGet<PickupItem>(info.Other, out var pickupItemComponent))
          return;
    }

    public void OnTriggerExit2D(Frame f, ExitInfo2D info)
    {
        if (!f.TryGet(info.Entity, out PlayerLink playerLink))
          return;
        if (!f.TryGet<PickupItem>(info.Other, out var pickupItemComponent))
          return;
    }

    public override void Update(Frame f, ref Filter filter)
    {

    }

    public struct Filter
    {
      public EntityRef Entity;
    }
  }
}
