using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Damageable : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Damageable damageable;
    
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
        image.DOFillAmount(current / max, 0.5f);
    }
}
