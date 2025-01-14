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
[CreateAssetMenu(fileName = "EntityStats", menuName = "Stats/Entity", order = 1)]
public class EntityStats : ScriptableObject, ICloneable
{
    public int maxHealth;
    public float movementSpeed;
    public string name;
    public float attackCooldown;
    public int damage;
    public float jumpStrength;
    int currentHealth;
    public int getHealth()
    {
        return this.currentHealth;
    }
    public void setHealth(int health)
    {
        this.currentHealth = health;
    }
    public bool isInvincible;
    public void Initialize()
    {
        this.currentHealth = maxHealth;

    }




    public object Clone()

    {
        EntityStats clone = ScriptableObject.CreateInstance<EntityStats>();
        clone.maxHealth = this.maxHealth;
        clone.movementSpeed = this.movementSpeed;
        clone.name = this.name;
        clone.damage = this.damage;
        clone.jumpStrength = this.jumpStrength;
        clone.currentHealth = this.currentHealth;
        clone.isInvincible = this.isInvincible;
        clone.attackCooldown = this.attackCooldown;
        return clone;
    }

}
public class Entity : MonoBehaviour, IHealth
{
    public EntityStats stats;
    public EntityType type;
    public void Initialize(EntityStats stats)

    {
        this.stats = (EntityStats)stats.Clone();
        this.stats.Initialize();
        InitiateProperties();
    }
    public void Awake()
    {
        EntityStats clonedStats = (EntityStats)stats.Clone();
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
        this.stats.setHealth(0);
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
    public IEnumerator invincibleCooldown(float timeSeconds)
    {
        yield return new WaitForSeconds(timeSeconds);
        stats.isInvincible = false;
    }
    public void Damage(int damage)
    {
        if (stats.isInvincible == true)
        {
            return;
        }
        this.stats.setHealth(this.stats.getHealth() - damage);
        if (stats.getHealth() <= 0)
        {
            stats.setHealth(0);

        }
    }
}