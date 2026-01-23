namespace Quantum {
  using Photon.Deterministic;

  public abstract class DamageableBase : AssetObject
  {
      public FP MaxHealth;
      public unsafe abstract void DamageableHit(Frame f, EntityRef victim, EntityRef hitter, FP damage, Damageable* damageable);
  }
}
