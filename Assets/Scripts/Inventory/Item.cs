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
    public string itemName;
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
        clone.itemName = this.itemName;
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
    private int amount = 0;
    public void Awake()
    {
        this.stats = (ItemStats)this.stats.Clone();
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
        Item item = new Item();
        item.stats.name = "Empty";
        item.stats.value = 0;
        item.stats.type = Type.Empty;
        item.stats.damage = 0;
        item.stats.SetMaxStack();
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
        return this.stats.itemName == other.stats.itemName &&
               this.stats.description == other.stats.description &&
               this.stats.value == other.stats.value &&
               this.stats.type == other.stats.type &&
               this.stats.damage == other.stats.damage &&
               this.stats.cooldownSecond == other.stats.cooldownSecond &&
               this.stats.icon == other.stats.icon && // Compare icon if necessary
               this.stats.equipSound == other.stats.equipSound; // Compare equipSound if necessary
    }
    public void ChangeAmount(int amount)
    {
        this.amount += amount;
        if (this.amount <= 0)
        {
            SetItemType(Type.Empty);
        }
    }
    public int GetAmount()
    {
        return amount;
    }
}
