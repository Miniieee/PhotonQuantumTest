using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum {
    public unsafe class WeaponPickupItem : PickupItemBase
    {
        public WeaponBase weaponBase;

        public override void PickupItem(Frame f, EntityRef pickupItemEntity, EntityRef pickerEntity)
        {
            var weapon = f.Unsafe.GetPointer<Weapon>(pickerEntity);
            weapon->WeaponData = weaponBase;
            weapon->CooldownTime = 0;
            weaponBase.OnInit(f, pickerEntity, weapon);
            f.Events.OnWeaponChanged(pickerEntity, weaponBase.WeaponType);

            f.Destroy(pickupItemEntity);
        }

    }
}
