namespace Quantum {
  using UnityEngine;

  public class BulletView : QuantumEntityViewComponent
  {
      [SerializeField] private Transform _bulletVisual;

        public override void OnActivate(Frame frame)
        {
            var bullet = PredictedFrame.Get<Bullet>(EntityRef);
            var localPosition = _bulletVisual.localPosition;
            localPosition.y = bullet.HeightOffset.AsFloat;
            _bulletVisual.localPosition = localPosition;
        }
  }
}
