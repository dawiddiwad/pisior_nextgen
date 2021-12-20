using UnityEngine;
using System;

public class HealthManager : MonoBehaviour
{
    private int currentHealth = 0;

    public int intialHealth = 100;
    public event Action OnHealthChange = delegate { };

    void Start()
    {
        currentHealth = intialHealth;
    }

    public int changeBy(int points)
    {
        currentHealth += points;
        if (getHealthPrcnt() > 1)
        {
            currentHealth = intialHealth;
        }
        OnHealthChange();
        return getHealth();
    }

    public int getHealth()
    {
        return currentHealth >= 0 ? currentHealth : 0;
    }

    public decimal getHealthPrcnt()
    {
        return (decimal)currentHealth / (decimal)intialHealth;
    }
}
