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
    bool AddItem(List<Item> slotItem, Item item, int index)
    {
        // difference between current slot's cap and slot's item amount
        int emptySlot = slotItem[index].stats.GetMaxStack() - slotItem[index].GetAmount();
        if (emptySlot <= 0)
        {
            return false;
        }
        // cap it to the max stack
        int amountToAdd = Math.Clamp(item.GetAmount(), 0, emptySlot);
        item.ChangeAmount(-amountToAdd);
        slotItem[index].ChangeAmount(amountToAdd);
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
            slotItem[i].SetItemType(item.GetItemType());
            AddItem(slotItem, item, i);
        }
        return item;
    }
    public Item RemoveFromStorage(int index, int amount)
    {
        Item toReturn = Item.EmptyItem();
        if (index < 0 || index > slotItem.Count || slotItem[index].IsEmpty())
        {
            return toReturn;
        }
        int amountToRemove = Math.Clamp(amount, 0, slotItem[index].GetAmount());
        if (amountToRemove <= 0)
        {
            return toReturn;
        }
        slotItem[index].ChangeAmount(amountToRemove);
        return toReturn;
    }

}
