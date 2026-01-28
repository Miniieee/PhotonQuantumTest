namespace Quantum
{
  using Photon.Deterministic;

  public unsafe class ShotgunBullet : BulletData
  {
    public int NumberOfBullets;
    public FP SpreadAngle;

    public override void CreateBullet(Frame f, WeaponBase weaponData, EntityRef owner)
    {
      var ownerTransform = f.Get<Transform2D>(owner);
      var SpreadAngleRad = SpreadAngle * FP.Deg2Rad;

      for (int i = 0; i < NumberOfBullets; i++)
      {
        var bulletEntity = f.Create(Bullet);
        var bullet = f.Unsafe.GetPointer<Bullet>(bulletEntity);
        var bulletTransfrom = f.Unsafe.GetPointer<Transform2D>(bulletEntity);

        bulletTransfrom->Position = ownerTransform.Position + weaponData.Offset.XZ.Rotate(ownerTransform.Rotation);
        bulletTransfrom->Rotation = ownerTransform.Rotation + FPMath.Lerp(-SpreadAngleRad, SpreadAngleRad, (FP)i / (NumberOfBullets - 1));

        bullet->Speed = Speed;
        bullet->Damage = Damage;
        bullet->Owner = owner;
        bullet->Time = Duration;
        bullet->HeightOffset = weaponData.Offset.Y;
        bullet->Direction = bulletTransfrom->Up;
      }
    }
  }
}
