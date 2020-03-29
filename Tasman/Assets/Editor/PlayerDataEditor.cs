/* Tasman
 *
 * Created by Liam HALL on 01/05/19.
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
using UnityEditor;

public class PlayerDataEditor : EditorWindow
{
    int playersIndex = 0;
    string[] players = new string[] { "Player 0" };
    int currentTabIndex = 0;
    Vector2 scrollPosition;
    bool inventoryApplyClicked;
    bool inventoryRefreshClicked;

    [MenuItem ("Tasman/Windows/Player Data Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PlayerDataEditor));
    }

    void ShowInventory()
    {
        //Clear other stuff
        UpdateInventory();
    }

    void ShowGeneral()
    {
        UpdateGeneral();
    }

    void UpdateInventory()
    {
        List<InventorySlot> inventorySlot = Players.instance.players[0].gameObject.GetComponentInChildren<PlayerInventory>().inventorySlot;

        GUILayout.BeginVertical("Inventory Slots");
       
        for (int i = 0; i < inventorySlot.Count; i++)
        {
            GUILayout.BeginHorizontal($"Slot {i}");
            GUILayout.Label($"Slot {i}");
            GUILayout.Space(25);
            GUILayout.Label("Item ID");
            inventorySlot[i].ItemID = EditorGUILayout.IntField(inventorySlot[i].ItemID);
            if (GUILayout.Button("+"))
            {
                inventorySlot[i].ItemID++;
                InventoryChanged();
            }
            if (GUILayout.Button("-"))
            {
                inventorySlot[i].ItemID--;
                InventoryChanged();
            }
            GUILayout.Space(10);
            GUILayout.Label("Quantity");
            inventorySlot[i].Quantity = EditorGUILayout.IntField(inventorySlot[i].Quantity);
            if (GUILayout.Button("+"))
            {
                inventorySlot[i].Quantity++;
                InventoryChanged();
            }
            if (GUILayout.Button("-"))
            {
                inventorySlot[i].Quantity--;
                InventoryChanged();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

    void InventoryChanged()
    {
        Players.instance.players[0].gameObject.GetComponentInChildren<PlayerInventory>().InventoryChanged();
    }

    void ApplyButtonClick(List<InventorySlot> inventorySlot)
    {
        inventoryApplyClicked = false;
        Players.instance.players[0].gameObject.GetComponentInChildren<PlayerInventory>().inventorySlot = inventorySlot;
    }

    void RefreshButtonClick()
    {
        inventoryRefreshClicked = false;
        ShowInventory();
    }

    void UpdateGeneral()
    {
        PlayerInventory pd = Players.instance.players[0].gameObject.GetComponent<PlayerInventory>();

        GUILayout.BeginVertical("Inventory Slots");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Health");
        pd.SetHealth(EditorGUILayout.FloatField(pd.GetHealth()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Food Level");
        pd.SetFoodLevel(EditorGUILayout.FloatField(pd.GetFoodLevel()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Food Timer");
        pd.SetFoodTimer(EditorGUILayout.FloatField(pd.GetFoodTimer()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Sleep Level");
        pd.SetSleepLevel(EditorGUILayout.FloatField(pd.GetSleepLevel()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Sleep Timer");
        pd.SetSleepTimer(EditorGUILayout.FloatField(pd.GetSleepTimer()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enable Creative Mode");
        pd.SetCreativeMode(EditorGUILayout.Toggle(pd.GetCreativeMode()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Current Spawn Location");
        pd.SetCurrentSpawnLocation(EditorGUILayout.FloatField(pd.GetCurrentSpawnLocation().x), EditorGUILayout.FloatField(pd.GetCurrentSpawnLocation().y), EditorGUILayout.FloatField(pd.GetCurrentSpawnLocation().z));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Walk Speed");
        pd.SetWalkSpeed(EditorGUILayout.FloatField(pd.GetWalkSpeed()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sprint Speed");
        pd.SetSprintSpeed(EditorGUILayout.FloatField(pd.GetSprintSpeed()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Fly Speed");
        pd.SetFlySpeed(EditorGUILayout.FloatField(pd.GetFlySpeed()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Can Fly");
        pd.SetCanFly(EditorGUILayout.Toggle(pd.GetCanFly()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enable Invulnerability");
        pd.SetInvulnerability(EditorGUILayout.Toggle(pd.GetInvulnerability()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Enable Insta-destroy");
        pd.SetInstaDestroy(EditorGUILayout.Toggle(pd.GetInstaDestroy()));
        GUILayout.EndHorizontal();
        //EditorGUILayout.FloatField(Players.instance.players[0]);

    }

    void OnGUI()
    {
        if (!Players.instance)
        {
            GUILayout.Label("Players not present. Load into world?", EditorStyles.boldLabel);
            return;
        }

        playersIndex = EditorGUILayout.Popup(playersIndex, players);
        currentTabIndex = GUILayout.Toolbar(currentTabIndex, new string[] { "Inventory", "General" });

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        switch (currentTabIndex)
        {
            case 0:
                UpdateInventory();
                break;
            case 1:
                ShowGeneral();
                break;
        }

        GUILayout.EndScrollView();

    }

    public void InventoryUpdated()
    {
        UpdateInventory();
    }
}
