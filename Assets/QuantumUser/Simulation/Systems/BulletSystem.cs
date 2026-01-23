namespace Quantum {
    using System;
    using Photon.Deterministic;
  using UnityEngine.Scripting;

  [Preserve]
  public unsafe class BulletSystem : SystemMainThreadFilter<BulletSystem.Filter>, ISignalCreateBullet {
        

    public override void Update(Frame f, ref Filter filter)
    {
        var nextPosition = filter.Bullet->Direction * filter.Bullet->Speed * f.DeltaTime;;

        if(CheckForCollision(f, filter, nextPosition, out var entityHit))
        {
            if(f.Unsafe.TryGetPointer<Damageable>(entityHit, out var damageable))
            {
                f.Signals.DamageableHit(entityHit, filter.Bullet->Owner, filter.Bullet->Damage, damageable);
            }

            f.Destroy(filter.Entity);
            return;
        }

        CheckbulletForTimeExpiration(f, filter);

        filter.Transform->Position += nextPosition;
    }

        private void CheckbulletForTimeExpiration(Frame f, Filter filter)
        {
            filter.Bullet->Time -= f.DeltaTime;
            if (filter.Bullet->Time <= FP._0)
            {
                f.Destroy(filter.Entity);
            }
        }

        private bool CheckForCollision(Frame f, Filter filter, FPVector2 nextPosition, out EntityRef entityHit)
        {
            entityHit = EntityRef.None;
            var owner = filter.Bullet->Owner;
            var bulletTransfrom = f.Get<Transform2D>(filter.Entity);
            var collisions = f.Physics2D.LinecastAll(bulletTransfrom.Position, bulletTransfrom.Position + nextPosition, layerMask: int.MaxValue, QueryOptions.HitAll & ~QueryOptions.HitTriggers);

            for (int i = 0; i < collisions.Count; i++)
            {
                var collision = collisions[i];
                if (collision.Entity == filter.Entity || collision.Entity == owner)
                    continue;
                
                entityHit = collision.Entity;
                return true;
            }

            return false;
        }

        public void CreateBullet(Frame f, EntityRef owner, WeaponData weaponData)
    {
        var bulletData = weaponData.BulletData;
        var bulletEntity = f.Create(bulletData.Bullet);
        var bulletTransfrom = f.Unsafe.GetPointer<Transform2D>(bulletEntity);
        var ownerTransform = f.Get<Transform2D>(owner);

        bulletTransfrom->Position = ownerTransform.Position + weaponData.Offset.XZ.Rotate(ownerTransform.Rotation);
        bulletTransfrom->Rotation = ownerTransform.Rotation;

        var bullet = f.Unsafe.GetPointer<Bullet>(bulletEntity);
        bullet->Speed = bulletData.Speed;
        bullet->Damage = bulletData.Damage;
        bullet->Owner = owner;
        bullet->Time = bulletData.Duration;
        bullet->HeightOffset = weaponData.Offset.Y;
        bullet->Direction = ownerTransform.Up;
    }

    public struct Filter {
      public EntityRef Entity;
      public Bullet* Bullet;
      public Transform2D* Transform;
    }
  }
}
