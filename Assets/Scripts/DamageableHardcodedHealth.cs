using System;
using UnityEngine;

public class DamageableHardcodedHealth : DamageableBase
{
    [SerializeField] private float maxHealth = 100;
    public override float MaxHealth => maxHealth;
}
