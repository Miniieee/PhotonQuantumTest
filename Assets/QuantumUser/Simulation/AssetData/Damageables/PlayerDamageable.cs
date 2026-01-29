using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public class PlayerDamageable : DamageableBase
    {
        public unsafe override void DamageableHit(Frame f, EntityRef victim, EntityRef hitter, FP damage, Damageable* damageable)
        {
            damageable->Health -= damage;

            if (damageable->Health <= FP._0)
            {
                DropLoot(f, victim);
                f.Destroy(victim);
                f.Signals.PlayerKilled();
                return;
            }

            f.Events.OnDamageableHealthUpdate(victim, MaxHealth, damageable->Health);
        }

        private unsafe void DropLoot(Frame f, EntityRef victim)
        {
            var transform = f.Get<Transform2D>(victim);
            var healthloot = f.Create(f.SimulationConfig.HealthPickupItem);
            f.Unsafe.GetPointer<Transform2D>(healthloot)->Position = transform.Position + transform.Right * 2;
            if (!f.TryGet<Weapon>(victim, out var weapon))
                return;

            var weaponData = f.FindAsset<WeaponBase>(weapon.WeaponData);
            var weaponLoot = f.Create(f.SimulationConfig.GetEntityPrototypeFromWeaponType(weaponData.WeaponType));
            f.Unsafe.GetPointer<Transform2D>(weaponLoot)->Position = transform.Position + transform.Left * 2;

        }
    }
}
