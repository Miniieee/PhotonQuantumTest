namespace Quantum {
  using Photon.Deterministic;

    public unsafe class HealthPickupItem : PickupItemBase
    {
        public override void PickupItem(Frame f, EntityRef pickupItemEntity, EntityRef pickerEntity)
        {
          if(f.Unsafe.TryGetPointer<Damageable>(pickerEntity, out var damageable))
          {
            f.Signals.DamageableHealthRestored(pickerEntity, damageable);
          }
          f.Destroy(pickupItemEntity);
        }
    }
}
