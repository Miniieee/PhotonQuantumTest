using System.Collections.Generic;
using System.Linq;

namespace Quantum
{
  public partial class SimulationConfig : AssetObject
  {
    public AssetRef<EntityPrototype> HealthPickupItem;
    public List<WeaponTypeAndEntityPrototype> WeaponTypeAndEntityPrototypes;

    private Dictionary<WeaponType, EntityPrototype> _dictionary;

    public EntityPrototype GetEntityPrototypeFromWeaponType(WeaponType weaponType)
    {
      if (_dictionary == null)
      {
        _dictionary = WeaponTypeAndEntityPrototypes.ToDictionary(x => x.WeaponType, x => x.EntityPrototype);

      }

      return _dictionary[weaponType];
    }

  }
}