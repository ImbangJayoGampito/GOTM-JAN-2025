using System;
using UnityEngine;
using UnityEngine.UI;
public enum Element
{

    Earth,
    Air,
    Water,
    Fire,
    Nature
}

public enum EnemyType
{
    Rookie = 1,
    Trained = 2,
    Elite = 5,
    Boss = 20,
}
[SerializeField]
public class Loot
{
    public int gold;
    public int experience;

}


// Enums for Element, Origin, and EnemyType as earlier

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/Enemy")]
public class EnemyStats : ScriptableObject, ICloneable
{
    [TextArea]
    public string description;
    public float aggroRange;

    public Element element;
    public Loot loot;

    // Clone method
    public object Clone()
    {
        // Create a new instance of EnemyStats
        EnemyStats clonedStats = ScriptableObject.CreateInstance<EnemyStats>();

        // Copy the fields from the current instance to the new instance
        clonedStats.description = this.description;
        clonedStats.aggroRange = this.aggroRange;
        clonedStats.element = this.element;

        // Deep copy the Loot object, if it is a class and has to be cloned separately
        if (this.loot != null)
        {
            clonedStats.loot = new Loot
            {
                gold = this.loot.gold,
                experience = this.loot.experience
            };
        }
        else
        {
            clonedStats.loot = null; // Handle case if loot is null
        }

        return clonedStats;
    }
}
[Serializable]
public class Enemy : MonoBehaviour
{

    public EntityStats entityStats;

    public EnemyStats enemyStats;
    Entity entity;
    public EnemyType type;
    public void Awake()
    {
        this.enemyStats = (EnemyStats)enemyStats.Clone();
        Entity entity = gameObject.AddComponent<Entity>();
        entity.InitializeEnemy((EntityStats)entityStats.Clone());
        this.entity = entity;
    }
    public Color GetDisplayColor()
    {
        Color color = Color.gray;
        switch (enemyStats.element)
        {
            case Element.Earth:
                color = new Color(150, 75, 0);
                break;
            case Element.Fire:
                color = Color.red;
                break;
            case Element.Water:
                color = Color.blue;
                break;
            case Element.Air:
                color = Color.cyan;
                break;
            case Element.Nature:
                color = Color.green;
                break;
        }
        return color;
    }
    public string GetEnemyName()
    {

        return type.ToString() + " " + entity.stats.name;
    }

}
