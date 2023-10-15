using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment")]
public class Equipment : ScriptableObject
{
    public enum upgradeType{
        Weapon,
        Shield,
        Hull,
        Power,
        weaponUpgrade
    };
    public upgradeType updatetype;
    public float attackSpeed;
    public int damage;
    public int turretRotationSpeed;
    public int shieldRechargeRate;
    public int shieldCapacity;
    public int hullHealthPoints;
    public int batterCapacity;
    public int powerOutput;
    public int weaponLevel;
}
