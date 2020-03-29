/* Tasman
 *
 * Created by Liam HALL on 30/04/19.
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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
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

    public void ButtonResumeClicked(GameObject button)
    {
        Resume(button);
    }

    void Resume(GameObject button)
    {
        Click(button.GetComponentInChildren<AudioSource>());
        Players.instance.players[0].gameObject.GetComponent<MenuManager>().PauseMenuToggle();
    }
}
