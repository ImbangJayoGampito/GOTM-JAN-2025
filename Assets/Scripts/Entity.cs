using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using
    System.Collections.Generic;
using Unity.Collections;
public interface IHealth
{
    public void Damage(int amount);


}
[CreateAssetMenu(fileName = "EntityStats", menuName = "ScriptableObjects/EntityStats", order = 1)]

public class EntityStats : ScriptableObject, IHealth
{
    public int maxHealth;
    public float movementSpeed;
    public string name;
    public int damage;
    public float jumpStrength;
    public int currentHealth;
    public bool isInvincible;
    public void Initialize()
    {
        this.currentHealth = maxHealth;

    }

    public IEnumerator invincibleCooldown(float timeSeconds)
    {
        yield return new WaitForSeconds(timeSeconds);
        isInvincible = false;
    }
    public void Damage(int damage)
    {
        if (isInvincible == true)
        {
            return;
        }
        this.currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;
    }


    public EntityStats Clone()

    {
        EntityStats clone = ScriptableObject.CreateInstance<EntityStats>();
        clone.maxHealth = this.maxHealth;
        clone.movementSpeed = this.movementSpeed;
        clone.name = this.name;
        clone.damage = this.damage;
        clone.jumpStrength = this.jumpStrength;
        clone.currentHealth = this.currentHealth;
        clone.isInvincible = this.isInvincible;
        return clone;
    }

}
public class Entity : MonoBehaviour
{
    public EntityStats stats;
    public void Initialize(EntityStats stats)

    {
        this.stats = stats.Clone(); 
        this.stats.Initialize();
        InitiateProperties();
    }
    public void Awake()
    {
        EntityStats clonedStats = stats.Clone();
        clonedStats.Initialize(); 
        this.stats = clonedStats; 
        InitiateProperties();
    }
    public void InitiateProperties()
    {
        conditions = new Dictionary<Condition, bool>();
        this.DefaultConditions();
    }
    public void Kill()
    {
        this.stats.currentHealth = 0;
    }
    public enum Condition
    {
        Frozen,
        Burning,
        Dead
    }
    public void DefaultConditions()
    {
        foreach (Condition condition in Enum.GetValues(typeof(Condition)))
        {
            conditions.Add(condition, false);
        }
    }
    public void CloneCondition(Dictionary<Condition, bool> conditionToClone)
    {
        this.conditions = new Dictionary<Condition, bool>();
        foreach (KeyValuePair<Condition, bool> entry in conditionToClone)
        {
            this.conditions.Add(entry.Key, entry.Value);
        }
    }
    public Dictionary<Condition, bool> conditions;
}