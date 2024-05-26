using System;
using UnityEngine;

public abstract class DamageableBase : MonoBehaviour
{
    [field: SerializeField] public Collider Collider { get; private set; }
    
    public event Action OnDeath;
    protected float currentHealth;
    public abstract float MaxHealth { get; }
    public Action<float, float> OnHealthChanged { get; set; }


    private void Awake()
    {
        currentHealth = MaxHealth;
        Collider = GetComponent<Collider>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        if (currentHealth <= 0) OnDeath?.Invoke();
    }
}