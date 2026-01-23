using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum {
    public class PlayerDamageable : DamageableBase
    {
        public unsafe override void DamageableHit(Frame f, EntityRef victim, EntityRef hitter, FP damage, Damageable* damageable)
        {
            damageable->Health -= damage;

            if (damageable->Health <= FP._0)
            {
                f.Destroy(victim);
                return;
            }

            f.Events.OnDamageableHit(victim, MaxHealth, damageable->Health);
        }
    }
}
