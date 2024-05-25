using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float health = 100;
    public Action OnDeath;
    public Action<float,float> OnHealthChanged { get; set; }

    private float _health;

    private void Awake()
    {
        _health = health;
    }

    public void TakeDamage(float damage)
    {   
        _health -= damage;
        OnHealthChanged?.Invoke(_health, health);
        if (_health <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
