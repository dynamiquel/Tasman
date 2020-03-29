/* Tasman
 *
 * Created by Liam HALL on 21/07/19.
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
using UnityEngine.SceneManagement;

public class CreateNewWorldMenu : Menu
{
    public GameObject mainMenuButtons, worldParent;
    public GameObject worldButtonPrefab;
    public Transform contentViewport;
    public StringParameter[] stringInputs;

    private void OnEnable()
    {
        ResetValues();
    }

    void ResetValues()
    {
        foreach (var si in stringInputs)
            si.SetValueAsNum(0);
    }

    private void Update()
    {
        UserInput();
    }

    public void UserInputButtonClicked(int x)
    {
        switch (x)
        {
            case 1:
                MainMenuManager.instance.LoadMenu(0);
                break;
        }
    }

    void UserInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            MainMenuManager.instance.LoadMenu(0);
        }
    }
}
