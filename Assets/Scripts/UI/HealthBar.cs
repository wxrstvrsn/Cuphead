using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject[] bars;

    public void UpdateHealthBarUI(int currentHealth)
    {
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i].SetActive(i <= currentHealth);
        }
    }
}
