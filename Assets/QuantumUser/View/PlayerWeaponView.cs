namespace Quantum {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

  public class PlayerWeaponView : QuantumEntityViewComponent
  {
      private PlayerWeapon _currentPlayerWeapon;
      private Dictionary<WeaponType, PlayerWeapon> _playerWeapons;

        void Awake()
        {
            _playerWeapons = GetComponentsInChildren<PlayerWeapon>(true).ToDictionary(x=> x.weaponType, x=> x);
        }

        public override void OnActivate(Frame frame)
        {
            foreach (var pw in _playerWeapons.Values)
            {
                pw.gameObject.SetActive(false);
            }

            _currentPlayerWeapon = _playerWeapons[WeaponType.Pistol];
            _currentPlayerWeapon.gameObject.SetActive(true);

            QuantumEvent.Subscribe<EventOnWeaponChanged>(this, OnWeaponChanged);
        }

        private void OnWeaponChanged(EventOnWeaponChanged callback)
        {
            if(callback.Entity != EntityRef)
                return;
            if(callback.WeaponType == _currentPlayerWeapon.weaponType)
                return;

            _currentPlayerWeapon.gameObject.SetActive(false);
            _currentPlayerWeapon.Rig.weight = 0f;

            _currentPlayerWeapon = _playerWeapons[callback.WeaponType];
            _currentPlayerWeapon.gameObject.SetActive(true);
            _currentPlayerWeapon.Rig.weight = 1f;
        }

        public override void OnDeactivate()
        {
            QuantumEvent.UnsubscribeListener<EventOnWeaponChanged>(this);
        }
    }
}
