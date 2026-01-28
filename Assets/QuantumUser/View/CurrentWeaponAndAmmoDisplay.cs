using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponAndAmmoDisplay : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TMP_Text ammoText;


    void Awake()
    {
        QuantumEvent.Subscribe<EventOnWeaponChanged>(this, OnWeaponChanged);
        QuantumEvent.Subscribe<EventOnPlayerSpawned>(this, OnPlayerSpawnedChanged);
        QuantumEvent.Subscribe<EventOnAmmoChanged>(this, OnAmmoChanged);
    }

    private void OnAmmoChanged(EventOnAmmoChanged callback)
    {
        var f = callback.Game.Frames.Predicted;

        if (!callback.Game.PlayerIsLocal(f.Get<PlayerLink>(callback.Entity).Player))
            return;

        FillImageAndText(f, callback.Entity);
    }

    private void OnPlayerSpawnedChanged(EventOnPlayerSpawned callback)
    {
        if (!callback.Game.PlayerIsLocal(callback.PlayerLink.Player))
            return;

        var f = callback.Game.Frames.Verified;
        FillImageAndText(f, callback.Player);
    }

    private void FillImageAndText(Frame f, EntityRef entityRef)
    {
        var weapon = f.Get<Weapon>(entityRef);
        weaponIcon.sprite = f.FindAsset<WeaponBase>(weapon.WeaponData).WeaponSprite;
        ammoText.text = weapon.Ammo.ToString();
    }


    private void OnWeaponChanged(EventOnWeaponChanged callback)
    {
        var f = callback.Game.Frames.Predicted;

        if (!callback.Game.PlayerIsLocal(f.Get<PlayerLink>(callback.Entity).Player))
            return;

        FillImageAndText(f, callback.Entity);
    }

    private void OnDestroy()
    {
        QuantumEvent.UnsubscribeListener<EventOnWeaponChanged>(this);
        QuantumEvent.UnsubscribeListener<EventOnPlayerSpawned>(this);
    }
}
