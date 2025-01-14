using System;
using UnityEngine;
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
    public EnemyType type;

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
        clonedStats.type = this.type;
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
public class Enemy : MonoBehaviour
{
    public EnemyStats enemyStats;
    Entity entity;
    public void Awake()
    {
        this.enemyStats = (EnemyStats)enemyStats.Clone();
        this.entity = gameObject.GetComponent<Entity>();
    }


}
