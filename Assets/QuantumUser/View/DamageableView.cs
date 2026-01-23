namespace Quantum {
    using System;
    using System.Collections;
    using Photon.Deterministic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DamageableView : QuantumEntityViewComponent
  {
      [SerializeField] private Image healthBarImage;

      public override void OnActivate(Frame frame)
      {
            QuantumEvent.Subscribe<EventOnDamageableHealthUpdate>(this, OnDamageableHit);
        }

        private void OnDamageableHit(EventOnDamageableHealthUpdate callback)
        {
          if (callback.entityRef != EntityRef)
              return;

          StartCoroutine(UpdateHealthUI(callback.CurrentHealth, callback.MaxHealth));

          var healthPercentage = (float)(callback.CurrentHealth / callback.MaxHealth);
          healthBarImage.fillAmount = healthPercentage;
      }

        private IEnumerator UpdateHealthUI(FP currentHealth, FP maxHealth)
        {
            var healthPercentage = (float)(currentHealth / maxHealth);

            while(!Mathf.Approximately(healthBarImage.fillAmount, healthPercentage))
            {
                healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, healthPercentage, 0.1f);
                yield return null;
            }
        }

        public override void OnDeactivate()
      {
            QuantumEvent.UnsubscribeListener<EventOnDamageableHealthUpdate>(this);
        }
    }
}
