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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class WorldLoader : MonoBehaviour
{
    WorldData world;
    WorldDataMenu worldInfo;
    public GameObject loadingMenu;
    public TextMeshProUGUI progressText;

    public void StartLoad(string worldPath)
    {
        loadingMenu.SetActive(true);
        progressText.text = "Finding a colossal volume of air";
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadSceneAsync(1);
        StartCoroutine(LoadFile(worldPath));
    }

    public IEnumerator LoadFile(string worldPath)
    {
        string totalPath = System.IO.Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worldPath);
        print("TOTAL PATH" + totalPath);

        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == 1);

        progressText.text = "Planning out the excavation";

        if (File.Exists(System.IO.Path.Combine(totalPath, "world.tas")))
        {
            BinaryFormatter bf = new BinaryFormatter();
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(totalPath, "world.tas"));
            System.IO.Stream stream = new System.IO.MemoryStream(bytes);
            world = (WorldData)bf.Deserialize(stream);

            bytes = new byte[0];

            if (File.Exists(System.IO.Path.Combine(totalPath, "info.tas")))
            {
                bytes = File.ReadAllBytes(System.IO.Path.Combine(totalPath, "info.tas"));
                stream = new System.IO.MemoryStream(bytes);
                worldInfo = (WorldDataMenu)bf.Deserialize(stream);

                StartCoroutine(LoadWorld());
            }
        }
    }

    IEnumerator LoadWorld()
    {
        progressText.text = "Diggy diggy hole";
        yield return new WaitUntil(() => World.instance != null);

        World.instance.name = worldInfo.name;
        World.instance.cheatsEnabled = worldInfo.cheatsEnabled;
        World.instance.worldX = worldInfo.worldX;
        World.instance.worldY = worldInfo.worldY;
        World.instance.worldZ = worldInfo.worldZ;
        // other stuff
        World.instance.worldType = World.WorldType.Flat;
        World.instance.worldDifficulty = World.WorldDifficulty.Peaceful;
        World.instance.creationDate = worldInfo.creationDate;
        World.instance.lastSaved = worldInfo.lastSaved;
        World.instance.data = world.data;
        LoadPlayer();
        progressText.text = "Quality check";
        GameManager.instance.gameObject.AddComponent<WorldSave>();
        World.instance.OnWorldLoad();
        Destroy(gameObject);
    }

    void LoadPlayer() // Temp
    {
        progressText.text = "Scavenging items";
        PlayerInventory pi = Players.instance.players[0].GetComponent<PlayerInventory>();
        Vector3 originalSpawnLocation = new Vector3(world.playerData[0].originalSpawnLocation.x, world.playerData[0].originalSpawnLocation.y, world.playerData[0].originalSpawnLocation.z);
        Vector3 currentSpawnLocation = new Vector3(world.playerData[0].currentSpawnLocation.x, world.playerData[0].currentSpawnLocation.y, world.playerData[0].currentSpawnLocation.z);
        Vector3 currentLocation = new Vector3(world.playerData[0].currentLocation.x, world.playerData[0].currentLocation.y, world.playerData[0].currentLocation.z);
        List<Vector3Int> previousDeaths = new List<Vector3Int>();
        for (int i = 0; i < world.playerData[0].previousDeaths.Count; i++)
            previousDeaths.Add(new Vector3Int(world.playerData[0].previousDeaths[i].x, world.playerData[0].previousDeaths[i].y, world.playerData[0].previousDeaths[i].z));

        pi.SetData(world.playerData[0].newPlayer, world.playerData[0].profileID, world.playerData[0].dateCreated, world.playerData[0].versionCreated, world.playerData[0].secondsPlayed, world.playerData[0].secondsSinceLastDeath, world.playerData[0].health, world.playerData[0].foodLevel, world.playerData[0].foodSaturationLevel, world.playerData[0].foodExhaustionLevel, world.playerData[0].sleepLevel, world.playerData[0].sleepTickTimer, world.playerData[0].isCreative, world.playerData[0].exp, originalSpawnLocation, currentSpawnLocation, world.playerData[0].useOriginalSpawnLocation, currentLocation, world.playerData[0].isDead, world.playerData[0].inventorySlot, previousDeaths);
    }
}
