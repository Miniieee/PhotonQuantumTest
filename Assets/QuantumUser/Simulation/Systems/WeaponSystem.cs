namespace Quantum {
  using Photon.Deterministic;
    using UnityEngine.Scripting;

  [Preserve]
  public unsafe class WeaponSystem : SystemMainThreadFilter<WeaponSystem.Filter>, ISignalOnComponentAdded<Weapon>
  {
    public unsafe void OnAdded(Frame f, EntityRef entity, Weapon* component)
    {
      var weaponData = f.FindAsset<WeaponBase>(component->WeaponData);
      weaponData.OnInit(f, entity, component);
    }
    public override void Update(Frame f, ref Filter filter)
    {
      var weaponData = f.FindAsset<WeaponBase>(filter.Weapon->WeaponData);
      weaponData.OnUpdate(f, filter);
    }

    public struct Filter {
      public EntityRef Entity;
      public PlayerLink* Player;
      public Weapon* Weapon;
    }
  }
}
