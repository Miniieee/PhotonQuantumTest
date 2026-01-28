namespace Quantum
{
  using Photon.Deterministic;

  public unsafe class BulletData : AssetObject
  {
    public FP Duration;
    public EntityPrototype Bullet;
    public FP Damage;
    public FP Speed;

    public virtual void CreateBullet(Frame f, WeaponBase weaponData, EntityRef owner)
    {
      var bulletEntity = f.Create(Bullet);
      var bulletTransfrom = f.Unsafe.GetPointer<Transform2D>(bulletEntity);
      var ownerTransform = f.Get<Transform2D>(owner);

      bulletTransfrom->Position = ownerTransform.Position + weaponData.Offset.XZ.Rotate(ownerTransform.Rotation);
      bulletTransfrom->Rotation = ownerTransform.Rotation;

      var bullet = f.Unsafe.GetPointer<Bullet>(bulletEntity);
      bullet->Speed = Speed;
      bullet->Damage = Damage;
      bullet->Owner = owner;
      bullet->Time = Duration;
      bullet->HeightOffset = weaponData.Offset.Y;
      bullet->Direction = ownerTransform.Up;
    }
  }
}
