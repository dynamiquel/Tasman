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
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu1 : Menu
{
    public float secondsSinceLastDeath;
    string deathNote;

    private void OnEnable()
    {      
        StartCoroutine(GetDeathNote());
        StringBuilder sb = GetTimeSinceLastDeath();
        subheading.text = $"YOU SURVIVED FOR {sb}    -    {deathNote}";
    }

    IEnumerator GetDeathNote()
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(Application.streamingAssetsPath + "/JSON/DeathNotes.json");
        yield return request.SendWebRequest();
        string json = request.downloadHandler.text;

        List<DeathNote> allDeathNotes = JsonConvert.DeserializeObject<List<DeathNote>>(json);
        //List<DeathNote> allDeathNotes = JsonConvert.DeserializeObject<List<DeathNote>>(Resources.Load<TextAsset>("JSON/DeathNotes").ToString()); LEGACY
        DeathNote desiredDeathNotes = new DeathNote(0, new string[0]);

        for(int i = 0; i < allDeathNotes.Count; i++)
        {
            if (allDeathNotes[i].seconds > secondsSinceLastDeath)
            {
                desiredDeathNotes = allDeathNotes[i - 1];
                break;
            }
        }

        string deathNoteToReturn = desiredDeathNotes.description[UnityEngine.Random.Range(0, desiredDeathNotes.description.Length)];

        deathNote = deathNoteToReturn;
    }

    StringBuilder GetTimeSinceLastDeath()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(secondsSinceLastDeath);

        string[] time = new string[4];

        if (timeSpan.Days == 0)
            time[0] = "";
        else
            time[0] = $"{timeSpan.Days.ToString()}D ";

        if (timeSpan.Hours == 0)
            time[1] = "";
        else
            time[1] = $"{timeSpan.Hours.ToString()}H ";

        if (timeSpan.Minutes == 0)
            time[2] = "";
        else
            time[2] = $"{timeSpan.Minutes.ToString()}M ";

        if (timeSpan.Seconds == 0)
            time[3] = "";
        else
            time[3] = $"{timeSpan.Seconds.ToString()}S";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < time.Length; i++)
            sb.Append(time[i]);

        return sb;
    }

    public void ButtonClicked(int buttonID)
    {
        if (buttonID == 0)
        {
            ButtonClick(0);
            Players.instance.players[0].gameObject.GetComponent<PlayerInventory>().RespawnPlayer();
        }
        else if (buttonID == 1)
        {
            ButtonClick(0);
            StartCoroutine(CreateQuickDialog("EXIT WORLD", "Do you want to return to the main menu?", "YES", "NO", 0, ExitWorld));
        }
    }

    void ExitWorld()
    {
        SceneManager.LoadSceneAsync(0);
    }
}

public class DeathNote
{
    public int seconds { get; set; }
    public string[] description { get; set; }

    [JsonConstructor]
    public DeathNote(int seconds, string[] description)
    {
        this.seconds = seconds;
        this.description = description;
    }
}
