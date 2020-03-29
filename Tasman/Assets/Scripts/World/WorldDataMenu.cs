/* Tasman
 *
 * Created by Liam HALL on 20/06/19.
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
public class WorldDataMenu
{
    // Details
    public string name;
    public bool cheatsEnabled;
    public DateTime creationDate, lastSaved;
    public int playtime;
    //public Sprite screenshot;

    // Functional data
    public int season, day, second;
    public int worldX, worldY, worldZ;

    public enum WorldType
    {
        Flat,
        Normal
    };
    public WorldType worldType;

    public enum WorldGameMode
    {
        Survival,
        Creative
    };
    public WorldGameMode worldGameMode;

    public enum WorldDifficulty
    {
        Peaceful,
        Easy,
        Normal,
        Hard
    };
    public WorldDifficulty worldDifficulty;
}
