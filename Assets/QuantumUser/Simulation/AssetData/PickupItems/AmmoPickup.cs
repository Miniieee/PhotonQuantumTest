using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;


namespace Quantum
{
    public unsafe class AmmoPickup : PickupItemBase
    {
        public override void PickupItem(Frame f, EntityRef pickupItemEntity, EntityRef pickerEntity)
        {
            var weapon = f.Unsafe.GetPointer<Weapon>(pickerEntity);
            var weaponData = f.FindAsset<WeaponBase>(weapon->WeaponData);

            if (weaponData is not FiringWeapon firingWeapon)
                return;

            weapon->Ammo += firingWeapon.MaxAmmo;
            f.Events.OnAmmoChanged(pickerEntity, weapon->Ammo);
        }

    }
}

