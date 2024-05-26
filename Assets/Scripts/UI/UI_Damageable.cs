using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Damageable : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private DamageableBase damageable;

    private void Awake()
    {
        damageable.OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        damageable.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        if (image == null) return;
        image.DOFillAmount(current / max, 0.5f);
    }
}