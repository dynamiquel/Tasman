/* Tasman
 *
 * Created by Liam HALL on 03/05/19.
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
using UnityEngine.UI;
using TMPro;

public class HUDUI : MonoBehaviour
{
    float hudOpacity = 1f;
    float switchingHudOpacity = 1f;
    public bool showNumbers;

    bool crossHairEnabled = true;

    bool inventoryBarEnabled;
    byte inventorySelectedSlot;

    public Image[] inventorySelectImage = new Image[9];
    public RawImage[] inventoryItemImage = new RawImage[9];
    public TextMeshProUGUI[] inventoryQuantity = new TextMeshProUGUI[9];
    public GameObject crossHair;
    public GameObject healthBar;
    public GameObject hungerBar;
    public GameObject sleepBar;
    public GameObject itemNameText;
    public GameObject itemQuantityText;
    public GameObject blockObserverOverlay;
    public TextMeshProUGUI blockObserverNameText;

    bool isCreative;

    float health;
    float foodLevel;
    float sleepLevel;
    float durability;
    string itemName;
    int level;

    private void Start()
    {
        ChangeHUDOptions();
    }

    public void GameModeChanged(bool _isCreative)
    {
        isCreative = _isCreative;

        healthBar.SetActive(!isCreative);
        hungerBar.SetActive(!isCreative);
        sleepBar.SetActive(!isCreative);
    }

    void ChangeHUDOptions()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = hudOpacity;
        crossHair.SetActive(crossHairEnabled);
        healthBar.GetComponentInChildren<TextMeshProUGUI>().enabled = showNumbers;
        hungerBar.GetComponentInChildren<TextMeshProUGUI>().enabled = showNumbers;
        sleepBar.GetComponentInChildren<TextMeshProUGUI>().enabled = showNumbers;
    }

    public void HealthChanged(float _health)
    {
        health = _health;

        if (healthBar)
        {
            healthBar.GetComponentInChildren<TextMeshProUGUI>().text = health.ToString();
            healthBar.GetComponentInChildren<Slider>().value = health / 20;
        }
    }

    public void FoodLevelChanged(float _foodLevel)
    {
        foodLevel = _foodLevel;

        if (hungerBar)
        {
            hungerBar.GetComponentInChildren<TextMeshProUGUI>().text = foodLevel.ToString();
            hungerBar.GetComponentInChildren<Slider>().value = foodLevel / 20;
        }
    }

    public void SleepLevelChanged(float _sleepLevel)
    {
        sleepLevel = _sleepLevel;

        if (sleepBar)
        {
            sleepBar.GetComponentInChildren<TextMeshProUGUI>().text = sleepLevel.ToString();
            sleepBar.GetComponentInChildren<Slider>().value = sleepLevel / 20;
        }
    }

    public void ItemChanged(string _itemName)
    {
        itemName = _itemName;
    }

    public void LevelChanged(int _level)
    {
        level = _level;
    }

    public void SelectedInventorySlotChanged(byte _inventorySelectedSlot, int _inventorySelectedSlotItemID, int _inventorySelectedSlotItemTotalQuantity)
    {
        inventorySelectedSlot = _inventorySelectedSlot;
        ChangeSelectedInventorySlot();
        ChangeSelectedInventorySlotItemName(_inventorySelectedSlotItemID);
        ChangeSelectedInventorySlotItemQuantity(_inventorySelectedSlotItemTotalQuantity);
    }

    void ChangeSelectedInventorySlot()
    {
        for (int i = 0; i < inventorySelectImage.Length; i++)
        {
            inventorySelectImage[i].enabled = false;
        }

        inventorySelectImage[inventorySelectedSlot].enabled = true;
    }

    public void ChangeSelectedInventorySlotItemName(int _inventorySelectedSlotItemID)
    {
        Item item = ItemDatabase.instance.items[_inventorySelectedSlotItemID];
        itemName = item.GetName();

        if (itemNameText)
        {
            if (item.Id > 0)
            {
                itemNameText.GetComponentInChildren<TextMeshProUGUI>().text = itemName;
            }
            else
            {
                itemNameText.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void ChangeSelectedInventorySlotItemQuantity(int quantity)
    {
        if (itemQuantityText)
        {
            if (quantity > 0)
            {
                itemQuantityText.GetComponent<TextMeshProUGUI>().text = quantity.ToString();
            }
            else
            {
                itemQuantityText.GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void ChangeInventoryBarItems(int[,] inventorySlots)
    {
        double tUnit = 0.0555555556d;

        for (int i = 0; i < inventorySlots.GetLength(0); i++)
        {
            Item item = ItemDatabase.instance.items[inventorySlots[i, 0]];

            if (item.Id > 0)
            {
                inventoryItemImage[i].enabled = true;
                Vector2 texturePos = ItemDatabase.instance.blocks[item.Id].TexturePos.North;
                inventoryItemImage[i].uvRect = new Rect((float)tUnit * texturePos.x, (float)tUnit * texturePos.y, (float)tUnit, (float)tUnit);
            }
            else
            {
                inventoryItemImage[i].enabled = false;
            }

            if (item.GetStackLimit() < 2 || item.Id == 0)
            {            
                inventoryQuantity[i].text = "";
            }
            else
            {
                inventoryQuantity[i].text = inventorySlots[i, 1].ToString();
            }
        }
    }

    public void ChangeBlockObserver(byte blockID)
    {
        // If the block is not air and the Block Observer is enabled in the player's settings, then display the block's name.
        if (blockID > 0 && GameManager.instance.GetComponent<OptionsManager>().options.GetBlockObserver() > 0)
        {        
            blockObserverOverlay.SetActive(true);
            blockObserverNameText.text = ItemDatabase.instance.GetItem(blockID).ItemName;
        }
        else
        {
            DisableBlockObserver();
        }
    }

    void DisableBlockObserver()
    {
        blockObserverOverlay.SetActive(false);
        blockObserverNameText.text = string.Empty;
    }
}
