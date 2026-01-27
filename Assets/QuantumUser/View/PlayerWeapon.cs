using System.Collections;
using System.Collections.Generic;
using Quantum;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeapon : MonoBehaviour
{
    [field:SerializeField] public WeaponType weaponType {get; private set;}
    [field: SerializeField] public Rig Rig { get; private set; }

}
