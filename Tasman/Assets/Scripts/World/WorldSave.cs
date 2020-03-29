/* Tasman
 *
 * Created by Liam HALL on 29/06/19.
 * Copyright © 2019 Liam HALL. All rights reserved.
 *
 * Coding Convention: Microsoft C# Coding Conventions
 * 
 * Contributors:
 *   Liam HALL
 */
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSave : MonoBehaviour
{
    public void SaveWorldClick()
    {
        GenerateWorldData();
    }

    void GenerateWorldData()
    {
        print("Generating world data...");

        WorldData worldData = new WorldData();
        WorldDataMenu worldInfo = new WorldDataMenu();
        worldInfo.name = World.instance.name;

        if (World.instance.worldType == World.WorldType.Flat)
            worldInfo.worldType = WorldDataMenu.WorldType.Flat;
        else if (World.instance.worldType == World.WorldType.Normal)
            worldInfo.worldType = WorldDataMenu.WorldType.Normal;

        if (World.instance.worldGameMode == World.WorldGameMode.Survival)
            worldInfo.worldGameMode = WorldDataMenu.WorldGameMode.Survival;
        else if (World.instance.worldGameMode == World.WorldGameMode.Creative)
        {
            worldInfo.worldGameMode = WorldDataMenu.WorldGameMode.Creative;
            worldInfo.cheatsEnabled = true;
        }

        if (World.instance.worldDifficulty == World.WorldDifficulty.Peaceful)
            worldInfo.worldDifficulty = WorldDataMenu.WorldDifficulty.Peaceful;
        else if (World.instance.worldDifficulty == World.WorldDifficulty.Easy)
            worldInfo.worldDifficulty = WorldDataMenu.WorldDifficulty.Easy;
        else if (World.instance.worldDifficulty == World.WorldDifficulty.Normal)
            worldInfo.worldDifficulty = WorldDataMenu.WorldDifficulty.Normal;
        else if (World.instance.worldDifficulty == World.WorldDifficulty.Hard)
            worldInfo.worldDifficulty = WorldDataMenu.WorldDifficulty.Hard;

        worldInfo.worldX = World.instance.worldX;
        worldInfo.worldY = World.instance.worldY;
        worldInfo.worldZ = World.instance.worldZ;

        worldData.data = World.instance.data;

        worldData.playerData.Add(Players.instance.players[0].GetComponent<PlayerInventory>().GetPlayerData());

        worldInfo.creationDate = World.instance.creationDate;
        worldInfo.lastSaved = DateTime.UtcNow;

        print("World data generated");

        SaveWorld(worldData, worldInfo);
    }

    void SaveWorld(WorldData worldData, WorldDataMenu worldInfo)
    {
        print("Saving world");
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds"));

        string path = Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worldInfo.name);

        UnityEngine.Windows.Directory.CreateDirectory(path + ".incomplete");

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Path.Combine(path + ".incomplete", "world.tas"));
        bf.Serialize(file, worldData);
        file.Close();

        FileStream fileInfo = File.Create(Path.Combine(path + ".incomplete", "info.tas"));
        bf.Serialize(fileInfo, worldInfo);
        fileInfo.Close();

        UnityEngine.Windows.Directory.Delete(path);
        Directory.Move(path + ".incomplete", path);

        print("World saved");
        print(Application.persistentDataPath);
    }
}
