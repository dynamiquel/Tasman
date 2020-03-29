/* Tasman
 *
 * Created by Liam HALL on 01/03/19.
 * Copyright © 2019 Liam HALL. All rights reserved.
 *
 * Coding Convention: Microsoft C# Coding Conventions
 * 
 * Contributors:
 *   Liam HALL
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item: MonoBehaviour
{
    public int Id { get; set; }
    public string AddonName { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public string KeyInfo { get; set; }
    public enum ItemType_
    {
        Block,
        EvolveBlock,
        UtilityBlock
    }
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public ItemType_ ItemType { get; set; }
    public int Value { get; set; }
    public int SmeltExperience { get; set; }
    public Sprite InventorySprite { get; set; }
    public int StackLimit { get; set; }

    [Newtonsoft.Json.JsonConstructor]
    public Item(int _id, string _addonName, string _itemName, string _description, string _keyInfo, int _value, int _smeltExperience, int _stackLimit)
    {
        Id = _id;
        AddonName = _addonName;
        ItemName = _itemName;
        Description = _description;
        KeyInfo = _keyInfo;
        Value = _value;
        SmeltExperience = _smeltExperience;
        StackLimit = _stackLimit;
    }

    public string GetName()
    {
        return ItemName;
    }

    public int GetStackLimit()
    {
        return StackLimit;
    }

    public void DropItem()
    {

    }

    public void PickupItem()
    {

    }

    public string GetInfo(string att)
    {
        switch(att)
        {
            case "id":
                return Id.ToString();
            case "addonName":
                return AddonName;
            case "name":
                return ItemName;
            case "description":
                return Description;
            case "keyInfo":
                return KeyInfo;
            case "itemType":
                return ItemType.ToString();
            case "value":
                return Value.ToString();
            case "smeltExperience":
                return SmeltExperience.ToString();
            default:
                return "Error";
        }
    }

    public string[] GetAllInfo()
    {
        string[] info = { Id.ToString(), AddonName, ItemName, Description, KeyInfo, ItemType.ToString(), Value.ToString(), SmeltExperience.ToString()};
        return info;
    }
}
