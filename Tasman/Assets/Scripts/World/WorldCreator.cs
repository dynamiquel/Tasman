/* Tasman
 *
 * Created by Liam HALL on 22/06/19.
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WorldCreator : MonoBehaviour
{
    public string worldName;

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

    public int worldX = 128, worldY = 129, worldZ = 130;

    public void CommenceCreateWorld()
    {
        GetUserInputs();
        Validation();
        CreateWorld();
        LoadWorld();
    }

    public CreateNewWorldMenu createNewWorldMenu;
    void GetUserInputs()
    {
        if (!createNewWorldMenu)
            return;
        if (createNewWorldMenu.stringInputs.Length < 1)
            return;

        StringParameter worldName = Array.Find(createNewWorldMenu.stringInputs, si => si.optionID == 1);
        StringParameter worldGamemode = Array.Find(createNewWorldMenu.stringInputs, si => si.optionID == 2);
        StringParameter worldDifficulty = Array.Find(createNewWorldMenu.stringInputs, si => si.optionID == 3);
        StringParameter worldType = Array.Find(createNewWorldMenu.stringInputs, si => si.optionID == 4);
        StringParameter worldSize = Array.Find(createNewWorldMenu.stringInputs, si => si.optionID == 5);

        print($"name {worldName} - gamemode {worldGamemode} - difficulty {worldDifficulty} - type {worldType} - size {worldSize}");

        this.worldName = worldName.GetValue();

        switch (worldGamemode.GetValueAsNum())
        {
            case 0:
                worldGameMode = WorldGameMode.Survival;
                break;
            case 1:
                worldGameMode = WorldGameMode.Creative;
                break;
            default:
                worldGameMode = WorldGameMode.Survival;
                break;
        }

        switch (worldDifficulty.GetValueAsNum())
        {
            case 0:
                this.worldDifficulty = WorldDifficulty.Peaceful;
                break;
            case 1:
                this.worldDifficulty = WorldDifficulty.Easy;
                break;
            case 2:
                this.worldDifficulty = WorldDifficulty.Normal;
                break;
            case 3:
                this.worldDifficulty = WorldDifficulty.Hard;
                break;
            default:
                this.worldDifficulty = WorldDifficulty.Peaceful;
                break;
        }

        switch (worldType.GetValueAsNum())
        {
            case 0:
                this.worldType = WorldType.Flat;
                break;
            case 1:
                this.worldType = WorldType.Normal;
                break;
            default:
                this.worldType = WorldType.Flat;
                break;
        }

        switch (worldSize.GetValueAsNum())
        {
            case 0:
                worldX = 128;
                worldY = 256;
                worldZ = 128;
                break;
            case 1:
                worldX = 512;
                worldY = 256;
                worldZ = 512;
                break;
            case 2:
                worldX = 1024;
                worldY = 256;
                worldZ = 1024;
                break;
            case 3:
                worldX = 3072;
                worldY = 256;
                worldZ = 3072;
                break;
            default:
                worldX = 128;
                worldY = 256;
                worldZ = 128;
                break;
        }
    }

    void Validation()
    {
        if (worldName == "")
            worldName = "_";
        if (worldName.Contains("."))
            worldName = "_";

        char[] worldNameArray = worldName.ToCharArray();

        for (int i = 0; i < worldNameArray.Length; i++)
        {
            if (!char.IsLetterOrDigit(worldNameArray[i]))
            {
                worldNameArray[i] = '_';
            }
        }

        if (worldName.Length < 1)
            worldName.PadRight(1);


        if (File.Exists(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worldName, "world.tas")))
        {
            int fileEx = 0;

            while (File.Exists(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worldName, "world.tas")))
            {
                fileEx++;
            }

            worldName += fileEx;
        }
    }

    void CreateWorld()
    {
        print("Create world");

        WorldData newWorld = new WorldData();
        WorldDataMenu newWorldInfo = new WorldDataMenu();
        newWorldInfo.name = worldName;

        if (worldType == WorldType.Flat)
            newWorldInfo.worldType = WorldDataMenu.WorldType.Flat;
        else if (worldType == WorldType.Normal)
            newWorldInfo.worldType = WorldDataMenu.WorldType.Normal;

        if (worldGameMode == WorldGameMode.Survival)
            newWorldInfo.worldGameMode = WorldDataMenu.WorldGameMode.Survival;
        else if (worldGameMode == WorldGameMode.Creative)
        {
            newWorldInfo.worldGameMode = WorldDataMenu.WorldGameMode.Creative;
            newWorldInfo.cheatsEnabled = true;
        }

        newWorldInfo.worldDifficulty = WorldDataMenu.WorldDifficulty.Peaceful;
        newWorldInfo.worldX = worldX;
        newWorldInfo.worldY = worldY;
        newWorldInfo.worldZ = worldZ;
        newWorldInfo.creationDate = DateTime.UtcNow;
        newWorldInfo.lastSaved = DateTime.UtcNow;

        print("Before world gen");

        if (newWorldInfo.worldType == WorldDataMenu.WorldType.Flat)
            newWorld.data = WorldGenerationFlat();
        else if (newWorldInfo.worldType == WorldDataMenu.WorldType.Normal)
            newWorld.data = WorldGenerationNormal();

        newWorld.playerData.Add(CreatePlayerData());
        print("Before save world");
        SaveWorld(newWorld, newWorldInfo);
    }

    int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }

    byte[,,] WorldGenerationFlat()
    {
        byte[,,] data = new byte[worldX, worldY, worldZ];

        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                for (int y = 1; y < worldY; y++)
                {
                    if (y == 1)
                    {
                        data[x, y, z] = 5;
                    }
                    if (y > 1 && y <= 4) //y <= 4, then stone
                    {
                        data[x, y, z] = 3;
                    }
                    if (y <= 7 && y > 4)
                    {
                        data[x, y, z] = 1;
                    }
                    if (y == 8)
                    {
                        data[x, y, z] = 2;
                    }
                }
            }
        }

        return data;
    }

    byte[,,] WorldGenerationNormal()
    {
        byte[,,] data = new byte[worldX, worldY, worldZ];

        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 7, 0) + 1; // Added +1 to make sure minimum grass height is 1

                for (int y = 1; y < worldY; y++)
                {
                    if (y == 1)
                    {
                        data[x, y, z] = 5; // Bedrock
                    }
                    else if (y <= stone)
                    {
                        data[x, y, z] = 3; // Stone
                    }
                    else if (y <= dirt)
                    {
                        data[x, y, z] = 1; // Dirt
                    }
                    else if (y <= dirt + stone)
                    { //Changed this line thanks to a comment
                        data[x, y, z] = 2;
                    }
                }
            }
        }

        return data;
    }

    void SaveWorld(WorldData newWorld, WorldDataMenu newWorldMenu)
    {
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds"));

        string path = Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worldName);

        if (!UnityEngine.Windows.Directory.Exists(path))
            UnityEngine.Windows.Directory.CreateDirectory(path);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(path, "world.tas"));
        bf.Serialize(file, newWorld);
        file.Close();
        FileStream fileInfo = File.Create(Path.Combine(path, "info.tas"));
        bf.Serialize(fileInfo, newWorldMenu);
        fileInfo.Close();
        print("World saved");
        print(Application.persistentDataPath);
    }

    void LoadWorld()
    {
        GameObject.Find("World Loader").GetComponent<WorldLoader>().StartLoad(worldName);
    }

    PlayerData CreatePlayerData()
    {
        bool newPlayer = true;
        string profileID = "01";
        DateTime dateCreated = DateTime.UtcNow;
        string versionCreated = "0.";
        int secondsPlayed = 0;
        int secondsSinceLastDeath = 0;
        float health = 20;
        float foodLevel = 20;
        float foodSaturationLevel = 20;
        float foodExhaustionLevel = 20;
        float sleepLevel = 20;
        float sleepTickTimer = 100;
        bool isCreative = true;
        float exp = 0;
        PlayerData.Vector3Ser originalSpawnLocation = new PlayerData.Vector3Ser(0, 0, 0);
        PlayerData.Vector3Ser currentSpawnLocation = originalSpawnLocation;
        bool useOriginalSpawnLocation = true;
        PlayerData.Vector3Ser currentLocation = currentSpawnLocation;
        bool isDead = false;
        List<PlayerData.Vector3IntSer> previousDeaths = new List<PlayerData.Vector3IntSer>();
        List<InventorySlot> inventorySlots = new List<InventorySlot>(32);
        for (int i = 0; i < inventorySlots.Capacity; i++)
        {
            inventorySlots.Add(new InventorySlot(i + 1, (i + 1) * 2));
        }

        PlayerData playerData = new PlayerData(newPlayer, profileID, dateCreated, versionCreated, secondsPlayed, secondsSinceLastDeath, health, foodLevel, foodSaturationLevel, foodExhaustionLevel, sleepLevel, sleepTickTimer, isCreative, exp, originalSpawnLocation, currentSpawnLocation, useOriginalSpawnLocation, currentLocation, isDead, inventorySlots, previousDeaths);
        return playerData;
    }
   
}


