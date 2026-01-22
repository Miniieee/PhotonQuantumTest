namespace Quantum {
  using Photon.Deterministic;
  using UnityEngine.Scripting;

  [Preserve]
  public unsafe class BulletSystem : SystemMainThreadFilter<BulletSystem.Filter>, ISignalCreateBullet {
        

    public override void Update(Frame f, ref Filter filter)
    {
        filter.Transform->Position += filter.Bullet->Direction * filter.Bullet->Speed * f.DeltaTime;
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
