/* Tasman
 *
 * Created by Liam HALL on 12/06/19.
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

public class MyWorldsMenu : Menu
{
    public GameObject mainMenuButtons, worldParent;
    public GameObject worldButtonPrefab;
    public Transform contentViewport;
    List<MenuWorldData> worldDatas = new List<MenuWorldData>();

    private void OnEnable()
    {
        StartCoroutine(GetWorlds());
    }

    private void Update()
    {
        UserInput();
    }

    public void SelectButtonClicked(int buttonID)
    {
        ButtonClick(0);
        if (buttonID < worldDatas.Count)
            GameObject.Find("World Loader").GetComponent<WorldLoader>().StartLoad(worldDatas[buttonID].path);
    }

    public void ModifyButtonClicked(int buttonID)
    {

    }

    public void UserInputButtonClicked(int x)
    {
        switch (x)
        {
            case 1:
                worldParent.SetActive(false);
                mainMenuButtons.SetActive(true);
                break;
        }
    }

    void UserInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            worldParent.SetActive(false);
            mainMenuButtons.SetActive(true);
        }
    }

    IEnumerator GetWorlds()
    {
        if (contentViewport.childCount > 0)
            for (int i = 0; i < contentViewport.childCount; i++)
                Destroy(contentViewport.GetChild(i).gameObject);

        yield return new WaitForSeconds(0.1f);

        GetWorldNames();
    }

    void GetWorldNames()
    {
        CreateDirectory();
        print("Getting world names...");
        if (UnityEngine.Windows.Directory.Exists(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds"))) // If the 'worlds' folder exists...
        {
            string[] files = Directory.GetDirectories(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds")); // Get the name of all of the files in folder
            List<string> worlds = new List<string>();

            for (int i = 0; i < files.Length; i++) // For every file...
                if (UnityEngine.Windows.File.Exists(Path.Combine(files[i], "info.tas")))
                    worlds.Add(files[i]);

            if (worlds.Count > 0) // If there is at least one world, call the 'GetWorldData' method
                GetWorldData(worlds);
        }
    }

    void CreateDirectory()
    {
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData"));
        UnityEngine.Windows.Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds"));
    }

    void GetWorldData(List<string> worlds)
    {
        print("Getting world data...");
        worldDatas = new List<MenuWorldData>();

        for (int i = 0; i < worlds.Count; i++)
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worlds[i], "info.tas")))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(Path.Combine(Application.persistentDataPath, "UserData", GameManager.instance.userID, "UserBinaries", "OfflineData", "Worlds", worlds[i], "info.tas"));
                    Stream stream = new System.IO.MemoryStream(bytes);
                    WorldDataMenu worldData = (WorldDataMenu)bf.Deserialize(stream);

                    bytes = new byte[0];

                    byte gamemode = 0;
                    if (worldData.worldGameMode == WorldDataMenu.WorldGameMode.Survival)
                        gamemode = 0;
                    else if (worldData.worldGameMode == WorldDataMenu.WorldGameMode.Creative)
                        gamemode = 1;

                    MenuWorldData menuWorldData = new MenuWorldData(worlds[i], worldData.name, worldData.cheatsEnabled, worldData.creationDate, worldData.lastSaved, worldData.playtime, gamemode);
                    worldDatas.Add(menuWorldData);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Unable to load world: '{worlds[i]}' - {e}");
                }
            }
        }

        if (worldDatas.Count > 0)
            DisplayWorldData();
    }

    void DisplayWorldData()
    {
        print("Displaying world data...");

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("AVAILABLE WORLDS");

        for (int i = 0; i < worldDatas.Count; i++)
        {
            sb.AppendLine($"{worldDatas[i].name}");
            GameObject worldButton = Instantiate(worldButtonPrefab, contentViewport);
            WorldButton wb = worldButton.GetComponent<WorldButton>();
            wb.SetWorldName(worldDatas[i].name);
            wb.SetWorldDate(worldDatas[i].creationDate.ToShortDateString());
            if (worldDatas[i].gamemode == 0)
                wb.SetWorldGamemode("Survival");
            else if (worldDatas[i].gamemode == 1)
                wb.SetWorldGamemode("Creative");
            wb.buttonNumber = i;
            wb.isNewWorldButton = false;
        }

        print(sb);
    }
}

class MenuWorldData
{
    // Details
    public string path;
    public string name;
    public bool cheatsEnabled;
    public System.DateTime creationDate, lastSaved;
    public int playtime;
    public byte gamemode;

    public MenuWorldData(string path, string name, bool cheatsEnabled, System.DateTime creationDate, System.DateTime lastSaved, int playtime, byte gamemode)
    {
        this.path = path;
        this.name = name;
        this.cheatsEnabled = cheatsEnabled;
        this.creationDate = creationDate;
        this.lastSaved = lastSaved;
        this.playtime = playtime;
        this.gamemode = gamemode;
    }
}
