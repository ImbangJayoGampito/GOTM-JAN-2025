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
    public float sprintMultiplier;
    public string name;
    public float attackCooldown;
    public int healthRegenerationRate;
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
        clone.sprintMultiplier = this.sprintMultiplier;
        return clone;
    }

}
public class Entity : MonoBehaviour, IHealth
{
    public EntityStats stats;
    public EntityType type;
    Cooldown healthCooldown;
    public void Initialize(EntityStats stats)
    {
        this.stats = stats;
        this.stats.Initialize();
        InitiateProperties();
        healthCooldown = gameObject.AddComponent<Cooldown>();
        healthCooldown.CooldownByRate(Math.Max(stats.maxHealth / 100, 1));
    }
    public void InitializeEnemy(EntityStats stats)
    {
        Initialize(stats);
        this.type = EntityType.Enemy;
    }
    public void Awake()
    {
        if (stats != null)
        {
            Initialize(stats);
        }
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
    void RegenerateHealth()
    {

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
    public void Update()
    {
        if (!healthCooldown.IsCooldown())
        {
            stats.setHealth(Math.Min(stats.getHealth() + 1, stats.maxHealth));
        }
    }
    public float GetSprintingMultiplier()
    {
        return Math.Max(stats.sprintMultiplier, 1.0f);
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

        if (stats.isInvincible == true || this.conditions[Condition.Dead])
        {
            return;
        }
        StartCoroutine(invincibleCooldown(Global.Instance.mechanic.iFrame));
        this.stats.setHealth(this.stats.getHealth() - damage);
        if (stats.getHealth() <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        stats.setHealth(0);
        this.conditions[Condition.Dead] = true;
        PlayerEntity playerEntity = gameObject.GetComponent<PlayerEntity>();
        if (playerEntity != null)
        {
            return;
        }

        this.gameObject.SetActive(false);
    }
    public void Respawn()
    {
        this.Initialize(this.stats);
        this.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {

        Entity other = collision.gameObject.GetComponent<Entity>();
        if (other != null)
        {

            Attacked(other);

        }
    }
    void Attacked(Entity other)
    {
        if (other.conditions[Condition.Dead] || other.type == this.type)
        {
            return;
        }
        Damage(Math.Max(0, other.stats.damage));
        Debug.Log("Ouch! I got " + other.stats.damage + " damage from " + other.stats.name);


    }

    void AttackedByWeapon(Item weapon, Entity attacker)
    {
        if (attacker.type == this.type || weapon.GetType().Equals(Type.Consumable) || weapon.GetType().Equals(Type.Material) || weapon.IsEmpty())
        {
            return;
        }
        Damage(Math.Min(0, weapon.stats.damage));
    }

}