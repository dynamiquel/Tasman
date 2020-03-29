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
public class WorldData
{
    public byte[,,] data;

    public List<PlayerData> playerData = new List<PlayerData>();
}
