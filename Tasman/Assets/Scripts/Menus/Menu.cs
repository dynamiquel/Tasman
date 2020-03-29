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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource fxAudioSource;
    public bool isController;
    public List<GameObject> userInputButtons;
    public GameObject dialogPrefab;
    public bool dialogActive;
    public TextMeshProUGUI title;
    public TextMeshProUGUI subheading;
    public EventSystem eventSystem;

    private void OnEnable()
    {
        if (eventSystem)
        {
            eventSystem.enabled = true;
        }
    }

    private void LateUpdate()
    {
        UserInput();
        CheckIfControllerPluggedIn();
    }

    public void ButtonClick(int audioClip)
    {
        if (fxAudioSource)
            if (audioClips.Length > 0)
                if (audioClip < audioClips.Length)
                    fxAudioSource.PlayOneShot(audioClips[audioClip]);
    }

    public void UserInputButtonClicked(int x)
    {

    }

    void DialogCountRefresh()
    {
        GameObject[] dialogs = GameObject.FindGameObjectsWithTag("Dialog");
        if (dialogs.Length > 0)
            SetDialogBool(true);
        else
            SetDialogBool(false);
    }

    void SetDialogBool(bool x)
    {
        dialogActive = x;
        EnableUserInputButtons(x);
    }

    void EnableUserInputButtons(bool x)
    {
        if (userInputButtons.Count > 0)
            foreach (GameObject uib in userInputButtons)
                uib.SetActive(x);
    }

    public GameObject CreateDialog(string title, string description, string option1, string option2, float timeout)
    {
        if (eventSystem)
        {
            eventSystem.enabled = false;
        }

        DialogCountRefresh();
        GameObject dialogGO;
        if (dialogPrefab == null)
        {
            dialogGO = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Quad));
        }
        else
        {
            dialogGO = Instantiate(dialogPrefab, gameObject.transform);
            dialogGO.GetComponent<Dialog>().SetAllValues(title, description, option1, option2, timeout);
            dialogGO.GetComponent<Dialog>().timeout = timeout;
        }

        DialogCountRefresh();

        return dialogGO;
    }

    public IEnumerator CreateQuickDialog(string title, string description, string option1, string option2, float timeout, Action Action1, Action Action2 = null)
    {
        GameObject dialogGO = CreateDialog(title, description, option1, option2, timeout);
        Dialog dialog = dialogGO.GetComponent<Dialog>();

        yield return new WaitUntil(() => dialog.answer != 0);

        if (dialog.answer == 1)
            Action1();
        else if (dialog.answer == 2)
            if (Action2 != null)
                Action2();

        Destroy(dialogGO);

        if (eventSystem)
        {
            eventSystem.enabled = true;
        }
    }

    public void UserInput()
    {

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
        foreach (GameObject uib in userInputButtons)
        {
            uib.GetComponentInChildren<Image>().enabled = x;
        }
    }
}
