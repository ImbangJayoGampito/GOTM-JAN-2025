using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int slot = 36;
    private List<Item> slotItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slotItem = new List<Item>();
        for (int i = 0; i < 36; i++)
        {
            slotItem.Add(Item.EmptyItem());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PrintStorage()
    {
        string result = "Storage Contents:\n";
        for (int i = 0; i < slotItem.Count; i++)
        {
            if (!slotItem[i].IsEmpty())
            {
                result += $"Slot {i}: {slotItem[i].GetAmount()} x {slotItem[i].stats.name}\n";
            }
            else
            {
                result += $"Slot {i}: Empty\n";
            }
        }
        Debug.Log(result);
    }
    bool AddItem(List<Item> slotItem, Item item, int index)
    {
        // difference between current slot's cap and slot's item amount
        int emptySlot = item.stats.GetMaxStack() - slotItem[index].GetAmount();
        // Debug.Log("Available slot for slot number" + index + " is: " + emptySlot);
        if (emptySlot <= 0 || item.IsEmpty())
        {
            return false;
        }
        // cap it to the max stack
        int amountToAdd = Math.Clamp(item.GetAmount(), 0, emptySlot);
        if (amountToAdd <= 0)
        {
            return false;
        }
        ItemStats toAdd = (ItemStats)item.stats.Clone();
        item.SetAmount(item.GetAmount() - amountToAdd);
        slotItem[index].SetAmount(slotItem[index].GetAmount() + amountToAdd);
        slotItem[index].stats = toAdd;
        return true;
    }
    public Item AddToStorage(Item item)
    {
        // Search for the same item in inventory
        for (int i = 0; i < slotItem.Count; i++)
        {

            if (!slotItem[i].IsEqual(item))
            {
                continue;
            }
            AddItem(slotItem, item, i);
        }
        // search for empty slot
        for (int i = 0; i < slotItem.Count; i++)
        {
            if (!slotItem[i].IsEmpty())
            {
                continue;
            }

            // change it to cooresponding type

            AddItem(slotItem, item, i);
        }
        return item;
    }
    public Tuple<Item, bool> AddToStorage(Item item, int index)
    {

        if (index < 0 || index >= slotItem.Count)
        {
            return Tuple.Create<Item, bool>(item, false);
        }

        bool success = false;
        if (slotItem[index].Equals(item) || slotItem[index].IsEmpty())
        {
            success = AddItem(slotItem, item, index);
        }
        return Tuple.Create<Item, bool>(item, success);
    }
    public Item GetItem(int index)
    {
        if (index < 0 || index >= slotItem.Count)
        {
            return Item.EmptyItem();
        }
        return slotItem[index];
    }
    public Item RemoveFromStorage(int index, int amount)
    {
        Item toReturn = Item.EmptyItem();
        if (index < 0 || index >= slotItem.Count || slotItem[index].IsEmpty())
        {
            return toReturn;
        }
        int amountToRemove = Math.Clamp(amount, 0, slotItem[index].GetAmount());
        if (amountToRemove <= 0)
        {
            return toReturn;
        }
        toReturn = slotItem[index].Clone();
        toReturn.SetAmount(amountToRemove);
        slotItem[index].SetAmount(slotItem[index].GetAmount() - amountToRemove);

        return toReturn;
    }
    private void OnCollisionEnter(Collision collision)

    {

        // Called when the collider/rigidbody enters the trigger

        Item other = collision.gameObject.GetComponent<Item>();

        if (other != null)
        {
            Item result = AddToStorage(other);
            // PrintStorage();
            // Debug.Log("item max stack = " + other.stats.type);
            if (result.IsEmpty())
            {
                Destroy(collision.gameObject);
            }
        }

    }
}
