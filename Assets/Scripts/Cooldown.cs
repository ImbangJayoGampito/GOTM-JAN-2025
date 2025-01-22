using System;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float currentCooldown = 0.0f;

    float maxCooldown = 0.0f;
    public void SetCooldown(float maxCooldown)
    {
        this.maxCooldown = maxCooldown;
    }
    public void CooldownByRate(int rate)
    {
       // Debug.Log("mreow!, cooldown time = " + rate);
        this.maxCooldown = 1.0f / rate;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentCooldown = Math.Clamp(currentCooldown - Time.deltaTime, 0.0f, maxCooldown);
    }
    public bool IsCooldown()
    {
        if (currentCooldown > 0)
        {

            return true;
        }
        else
        {

            currentCooldown = maxCooldown;

            return false;
        }
    }

}
