namespace Quantum
{
    using System;
    using System.Collections;
    using DG.Tweening;
    using Photon.Deterministic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DamageableView : QuantumEntityViewComponent
    {
        [SerializeField] private Image _healthBarImage;
        private Tween _tween;

        public override void OnActivate(Frame frame)
        {
            QuantumEvent.Subscribe<EventOnDamageableHealthUpdate>(this, OnDamageableHit);
        }

        private void OnDamageableHit(EventOnDamageableHealthUpdate callback)
        {
            if (callback.entityRef != EntityRef)
                return;

            _tween?.Kill();
            _tween = _healthBarImage.DOFillAmount((callback.CurrentHealth / callback.MaxHealth).AsFloat, 1f);
        }



        public override void OnDeactivate()
        {
            QuantumEvent.UnsubscribeListener<EventOnDamageableHealthUpdate>(this);
        }
    }
}
