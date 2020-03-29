/* Tasman
 *
 * Created by Liam HALL on 30/06/19.
 * Copyright © 2019 Liam HALL. All rights reserved.
 *
 * Coding Convention: Microsoft C# Coding Conventions
 * 
 * Contributors:
 *   Liam HALL
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public bool newPlayer;
    public string profileID;
    public DateTime dateCreated = DateTime.UtcNow;
    public string versionCreated;
    public float secondsPlayed;
    public float secondsSinceLastDeath;

    public float health;
    public float foodLevel;
    public float foodSaturationLevel;
    public float foodExhaustionLevel;
    public float sleepLevel;
    public float sleepTickTimer;

    public bool isCreative;

    public float exp;

    public Vector3Ser originalSpawnLocation;
    public Vector3Ser currentSpawnLocation;
    public bool useOriginalSpawnLocation;
    public Vector3Ser currentLocation;
    public bool isDead;

    public List<InventorySlot> inventorySlot = new List<InventorySlot>(capacity: 32);

    public List<Vector3IntSer> previousDeaths = new List<Vector3IntSer>();

    public PlayerData(bool newPlayer, string profileID, DateTime dateCreated, string versionCreated, float secondsPlayed, float secondsSinceLastDeath, float health, float foodLevel, float foodSaturationLevel, float foodExhaustionLevel, float sleepLevel, float sleepTickTimer, bool isCreative, float exp, Vector3Ser originalSpawnLocation, Vector3Ser currentSpawnLocation, bool useOriginalSpawnLocation, Vector3Ser currentLocation, bool isDead, List<InventorySlot> inventorySlots, List<Vector3IntSer> previousDeaths)
    {
        this.newPlayer = newPlayer;
        this.profileID = profileID;
        this.dateCreated = dateCreated;
        this.versionCreated = versionCreated;
        this.secondsPlayed = secondsPlayed;
        this.secondsSinceLastDeath = secondsSinceLastDeath;
        this.health = health;
        this.foodLevel = foodLevel;
        this.foodSaturationLevel = foodSaturationLevel;
        this.foodExhaustionLevel = foodExhaustionLevel;
        this.sleepLevel = sleepLevel;
        this.sleepTickTimer = sleepTickTimer;
        this.exp = exp;
        this.isCreative = isCreative;
        this.originalSpawnLocation = originalSpawnLocation;
        this.currentSpawnLocation = currentSpawnLocation;
        this.useOriginalSpawnLocation = useOriginalSpawnLocation;
        this.currentLocation = currentLocation;
        this.isDead = isDead;
        this.inventorySlot = inventorySlots;
        this.previousDeaths = previousDeaths;
    }

    [Serializable]
    public class Vector3Ser
    {
        public float x, y, z;

        public Vector3Ser(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public class Vector3IntSer
    {
        public int x, y, z;

        public Vector3IntSer(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
