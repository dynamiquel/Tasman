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
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    byte openMenu = 0; // 0 = none; 1 = character menu; 2 = pause menu; 3 = death menu
    public GameObject pauseMenu; // Pause Menu
    public GameObject hudMenu; // HUD
    public GameObject deathMenu; // Death Menu
    public GameObject thisPlayer; // The current player
    bool playerDead;

    private void Update()
    {
        DetectInput();
        CheckIfPlayerDied();
    }

    void CheckIfPlayerDied()
    {
        if (playerDead && (openMenu != 2 || openMenu != 3))
        {
            ShowDeathMenu();
        }
    }

    void ShowDeathMenu()
    {
        openMenu = 3;
        CloseCharacterMenu();
        CloseHUD();
        deathMenu.GetComponent<DeathMenu1>().secondsSinceLastDeath = thisPlayer.GetComponent<PlayerInventory>().GetSecondsSinceLastDeath();
        deathMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideDeathMenu()
    {
        playerDead = false;
        deathMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        OpenHUD();
        openMenu = 0;
    }

    // Detects user input
    void DetectInput()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (openMenu == 3)
            {
                thisPlayer.GetComponent<PlayerInventory>().RespawnPlayer();
            }
            else
            {
                PauseMenuToggle();
            }
        }
    }

    // Toggles the state of the Pause Menu
    public void PauseMenuToggle()
    {
        if (openMenu != 2) // If not Pause Menu, then
            OpenPauseMenu();
        else
        {
            if (!pauseMenu.GetComponent<PauseMenu2>().dialogOpen)
                ClosePauseMenu();
        }
    }

    // Opens the Pause Menu
    void OpenPauseMenu()
    {
        CloseHUD();
        openMenu = 2;

        // Screenshot
        ScreenCapture.CaptureScreenshot("thumbnail.png", 0);
        pauseMenu.SetActive(true);
        thisPlayer.GetComponentInChildren<ModifyTerrain>().enabled = false;
        thisPlayer.GetComponentInChildren<FirstPersonController>().enabled = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator CaptureScreenshot()
    {
        
        yield return null;
    }

    // Closes the Pause Menu
    void ClosePauseMenu()
    {
        OpenHUD();
        openMenu = 0;
        pauseMenu.SetActive(false);
        thisPlayer.GetComponentInChildren<ModifyTerrain>().enabled = true;
        thisPlayer.GetComponentInChildren<FirstPersonController>().enabled = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Toggles the state of the Character Menu
    public void CharacterMenuToggle()
    {
        if (openMenu == 0) // If no other menu is open, then
            OpenCharacterMenu();
        else
            CloseCharacterMenu();
    }

    // Opens the Character Menu
    void OpenCharacterMenu()
    {
        CloseHUD();
    }

    // Closes the Character Menu
    void CloseCharacterMenu()
    {
        OpenHUD();
    }

    void OpenHUD()
    {
        hudMenu.SetActive(true);
    }

    void CloseHUD()
    {
        hudMenu.SetActive(false);
    }

    public void SetPlayerDeathState(bool b)
    {
        playerDead = b;
    }
}
