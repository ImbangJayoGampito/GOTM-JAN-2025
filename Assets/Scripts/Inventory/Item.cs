using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum EntityType
{
    Friendly,
    Enemy
}
public enum Type
{
    Melee,
    Ranged,
    Mage,
    Consumable,
    Material,
    Empty
}
[CreateAssetMenu(fileName = "ItemStats", menuName = "Stats/Item")]
public class ItemStats : ScriptableObject, ICloneable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public new string name;
    [TextArea]
    public string description;
    public int value;
    public Type type;
    public int damage;
    public float cooldownSecond;
    public Sprite icon;
    public AudioClip equipSound;
    private int maxStack;

    public void Awake()
    {
        SetMaxStack();

    }
    public int GetMaxStack()
    {
        return maxStack;
    }
    public void SetMaxStack()
    {
        switch (type)
        {
            case Type.Melee:
                maxStack = 1;
                break;
            case Type.Ranged:
                maxStack = 1;
                break;
            case Type.Mage:
                maxStack = 1;
                break;
            case Type.Consumable:
                maxStack = 16;
                break;
            case Type.Material:
                maxStack = 64;
                break;
            case Type.Empty:
                maxStack = 0;
                break;
        }
    }
    public object Clone()
    {
        ItemStats clone = ScriptableObject.CreateInstance<ItemStats>();
        clone.name = this.name;
        clone.description = this.description;
        clone.value = this.value;
        clone.type = this.type;
        clone.equipSound = this.equipSound;
        clone.maxStack = this.maxStack;
        clone.cooldownSecond = this.cooldownSecond;
        clone.icon = this.icon;
        clone.equipSound = this.equipSound;
        return clone;
    }
}
public class Item : MonoBehaviour
{
    public ItemStats stats;
    private int currentAmount = 0;
    public int startingAmount;
    public void Awake()
    {
        if (this.stats == null)
        {
            this.stats = ScriptableObject.CreateInstance<ItemStats>();
        }
        else
        {
            this.stats = (ItemStats)this.stats.Clone();
        }


        this.stats.SetMaxStack();
        this.currentAmount = startingAmount;
        // Debug.Log("Max stack for " + stats.type + " is now: " + stats.GetMaxStack());
    }
    public Type GetItemType()
    {
        return this.stats.type;
    }
    public void SetItemType(Type type)
    {
        this.stats.type = type;
    }
    public static Item EmptyItem()

    {

        GameObject gameObject = new GameObject("EmptyItem");

        Item item = gameObject.AddComponent<Item>();

        item.stats.name = "Empty";
        item.stats.description = "Such emptiness!";
        item.stats.value = 0;
        item.stats.type = Type.Empty;
        item.stats.damage = 0;
        item.stats.SetMaxStack();

        return item;

    }
    public void SetAmount(int amount)
    {
        this.currentAmount = amount;
        if (this.currentAmount <= 0)
        {
            SetItemType(Type.Empty);
        }
    }
    public Item Clone()
    {
        GameObject gameObject = new GameObject("ClonedItem");
        Item item = gameObject.AddComponent<Item>();
        item.stats = (ItemStats)this.stats.Clone();
        item.currentAmount = this.currentAmount;
        return item;
    }
    public bool IsEmpty()
    {
        return stats.type == Type.Empty;
    }
    public bool IsEqual(Item other)

    {
        if (other == null)
        {
            return false; // If the other item is null, they are not equal
        }
        return this.stats.name == other.stats.name &&
               this.stats.description == other.stats.description &&
               this.stats.value == other.stats.value &&
               this.stats.type == other.stats.type &&
               this.stats.damage == other.stats.damage &&
               this.stats.cooldownSecond == other.stats.cooldownSecond &&
               this.stats.icon == other.stats.icon && // Compare icon if necessary
               this.stats.equipSound == other.stats.equipSound; // Compare equipSound if necessary
    }

    public int GetAmount()
    {
        return this.currentAmount;
    }
}
