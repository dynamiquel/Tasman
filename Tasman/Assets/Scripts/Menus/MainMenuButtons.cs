/* Tasman
 *
 * Created by Liam HALL on 26/02/19.
 * Copyright © 2019 Liam HALL. All rights reserved.
 *
 * Coding Convention: Microsoft C# Coding Conventions
 * 
 * Contributors:
 *   Liam HALL
 */
using Microsoft.Xbox.Services.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource fxAudioSource;
    public GameObject quitMenu;
    public GameObject mainMenuButtons;
    public Image background;
    public GameObject worldMenu;
    public GameObject mainMenu;
    public bool isController;
    public List<GameObject> userInputButtons;

    public GameObject dialogPrefab;

    void Click(int audioClip)
    {
        if (fxAudioSource)
        {
            if (audioClip < audioClips.Length)
            {
                fxAudioSource.PlayOneShot(audioClips[audioClip]);
            }
        }
    }

    public void ButtonQuitGameClicked(int audioClip)
    {
        QuitGame(audioClip);
    }
    
    void QuitGame(int audioClip)
    {
        Click(audioClip);
        mainMenuButtons.SetActive(false);
        quitMenu.SetActive(true);
    }

    public void ButtonDisplayOptionsClicked(int audioClip)
    {
        DisplayOptions(audioClip);
    }

    void DisplayOptions(int audioClip)
    {
        Click(audioClip);
    }

    public void ButtonStartWorldClicked(int audioClip)
    {
        StartWorld(audioClip);
    }

    void StartWorld(int audioClip)
    {
        Click(audioClip);
        mainMenu.SetActive(false);
        worldMenu.SetActive(true);
    }

    public void ButtonConfirmQuitClicked(int audioClip)
    {
        ConfirmQuit(audioClip);
    }

    void ConfirmQuit(int audioClip)
    {
        Click(audioClip);
        Application.Quit();
    }

    public void ButtonRejectQuitClicked(int audioClip)
    {
        RejectQuit(audioClip);
    }

    void RejectQuit(int audioClip)
    {
        Click(audioClip);
        quitMenu.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    private void LateUpdate()
    {
        ChangeColour();
        CheckIfControllerPluggedIn();
        //UserInput();
    }

    bool colourChange;
    int colourChangeCooldown;

    void ChangeColour()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            colourChange = !colourChange;
        }

        if (colourChange)
        {
            if (colourChangeCooldown >= 30)
            {
                background.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
                colourChangeCooldown = 0;
            }

            colourChangeCooldown++;
        }
    }

    void CheckIfControllerPluggedIn()
    {
        string[] joysticks = Input.GetJoystickNames();

        if (joysticks.Any(x => x.Length != 0))
        {
            isController = true;
            ChangeUserInputButtonsState(true);
        }
        else
        {
            isController = false;
            ChangeUserInputButtonsState(false);
        }
    }

    void ChangeUserInputButtonsState(bool x)
    {
        for (int i = 0; i < userInputButtons.Count; i++)
        {
            userInputButtons[i].GetComponentInChildren<Image>().enabled = x;
        }
    }

    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            XboxProfileButtonClicked();
        }
    }

    public PlayerAuthentication xboxPlayerAuthentication;

    public void XboxProfileButtonClicked()
    {
        if (xboxPlayerAuthentication)
        {
            xboxPlayerAuthentication.SignIn();
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        //StartCoroutine(DisplayDialog()); // Dialog that tells user to accept that this build is a beta
    }

    IEnumerator DisplayDialog()
    {
        GameObject dialogGO = Instantiate(dialogPrefab, gameObject.transform);
        Dialog dialog = dialogGO.GetComponent<Dialog>();
        dialog.SetAllValues("WELCOME", "In order to play this game, you must accept that this is a preview build.", "ACCEPT", "REJECT", 0f);

        yield return new WaitUntil(() => dialog.answer != 0);

        if (dialog.answer == 1)
        {
            Destroy(dialogGO);
        }
        else if (dialog.answer == 2)
        {
            print("App Quit!");
            Application.Quit();
        }
    }
}
