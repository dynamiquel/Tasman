/* Tasman
 *
 * Created by Liam HALL on 11/05/19.
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
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    void Click(AudioSource audioSource)
    {
        if (audioSource.isPlaying)
        {
            //return;
        }
        audioSource.Play();
    }

    public void ButtonQuitGameClicked(GameObject button)
    {
        QuitGame(button);
    }

    void QuitGame(GameObject button)
    {
        Click(button.GetComponentInChildren<AudioSource>());
        SceneManager.LoadSceneAsync(0);
    }

    public void ButtonDisplayOptionsClicked(GameObject button)
    {
        DisplayOptions(button);
    }

    void DisplayOptions(GameObject button)
    {
        Click(button.GetComponentInChildren<AudioSource>());
    }

    public void ButtonRespawnClicked(GameObject button)
    {
        Respawn(button);
    }

    void Respawn(GameObject button)
    {
        Click(button.GetComponentInChildren<AudioSource>());
        Players.instance.players[0].gameObject.GetComponent<PlayerInventory>().RespawnPlayer();
    }
}
