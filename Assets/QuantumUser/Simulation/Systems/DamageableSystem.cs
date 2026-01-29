namespace Quantum {
  using Photon.Deterministic;
    using UnityEngine.Scripting;

  [Preserve]
  public unsafe class DamageableSystem : SystemMainThreadFilter<DamageableSystem.Filter>, ISignalOnComponentAdded<Damageable>, ISignalDamageableHit, ISignalDamageableHealthRestored
  {
    public unsafe void DamageableHealthRestored(Frame f, EntityRef entity, Damageable* damageable)
    {
      FP maxHealth = f.FindAsset<DamageableBase>(damageable->DamageableData).MaxHealth;
      damageable->Health = maxHealth;
      f.Events.OnDamageableHealthUpdate(entity, maxHealth, damageable->Health);
    }

    public unsafe void DamageableHit(Frame f, EntityRef victim, EntityRef hitter, FP damage, Damageable* damageable)
    {
      var damageableBase = f.FindAsset(damageable->DamageableData);
      damageableBase.DamageableHit(f, victim, hitter, damage, damageable);
    }

    public unsafe void OnAdded(Frame f, EntityRef entity, Damageable* component)
    {
      var damageableData = f.FindAsset(component->DamageableData);
      var characterStats = f.Get<CharacterStats>(entity);
      component->Health = damageableData.MaxHealth * f.FindAsset<CharacterStatsConfig>(characterStats.CharacterStatConfig).HealthMultiplyer;
    }

    public override void Update(Frame f, ref Filter filter)
    {
      if (!f.TryGet<PlayerLink>(filter.Entity, out _))
        return;

      var shrinkingCircle = f.GetSingleton<ShrinkingCircle>();

      if (CheckIfEntityIsOutsideCircle(f, filter, shrinkingCircle))
      {
        var damageableAsset = f.FindAsset<DamageableBase>(filter.Damageable->DamageableData);
        var shrinkingCircleConfig = f.FindAsset<ShrinkingCircleConfig>(shrinkingCircle.ShrinkingCircleConfig);

        damageableAsset.DamageableHit(f, filter.Entity, filter.Entity, shrinkingCircleConfig.DamageDealingPerSecond * f.DeltaTime, filter.Damageable);
      }
    }

    public bool CheckIfEntityIsOutsideCircle(Frame f, Filter filter, ShrinkingCircle shrinkingCircle)
    {
      Transform2D transform = f.Get<Transform2D>(filter.Entity);

      return FPVector2.Distance(transform.Position, shrinkingCircle.Position) >= shrinkingCircle.CurrentRadius / 2;
    }

    public struct Filter
    {
        public EntityRef Entity;
        public Damageable* Damageable;
    }
  }
}
