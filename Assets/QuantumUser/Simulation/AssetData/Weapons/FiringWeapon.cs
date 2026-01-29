using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum {
    public abstract unsafe class FiringWeapon : WeaponBase
    {
        public BulletData BulletData;
        public byte MaxAmmo;

        public override void OnInit(Frame f, EntityRef entity, Weapon* weapon)
        {
            weapon->Ammo = MaxAmmo;
        }

        protected void FireWeapon(Frame f, WeaponSystem.Filter filter)
        {
            if(filter.Weapon->Ammo <= 0)
                return;

            var characterStats = f.Get<CharacterStats>(filter.Entity);
            var characterStatsConfig = f.FindAsset<CharacterStatsConfig>(characterStats.CharacterStatConfig);

            filter.Weapon->CooldownTime = Cooldown * characterStatsConfig.FireRateMultiplyer;
            filter.Weapon->Ammo--;
            f.Signals.CreateBullet(filter.Entity, this);

            f.Events.OnAmmoChanged(filter.Entity, filter.Weapon->Ammo);
        }
    }
}
