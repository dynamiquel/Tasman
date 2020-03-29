/* Tasman
 *
 * Created by Liam HALL on 01/09/19.
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
using UnityEditor;
using System;

public class OptionsManagerDebug : EditorWindow
{
    int currentTabIndex = 0;
    Vector2 scrollPosition;

    [MenuItem ("Tasman/Windows/Options Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(OptionsManagerDebug));
    }

    void ShowOff()
    {

    }

    void ShowGeneral()
    {
        UpdateGeneral();
    }

    void UpdateGeneral()
    {
        OptionsManager om = GameManager.instance.gameObject.GetComponent<OptionsManager>();

        GUILayout.BeginVertical("Options");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Windowed");
        om.options.SetWindowed(EditorGUILayout.Popup(om.options.GetWindowed(), new string[] { "Windowed", "Maximised Window", "Fullscreen Window", "Fullscreen"}));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Shadows");
        om.options.SetShadows(EditorGUILayout.Popup(om.options.GetShadows(), new string[] { "Off", "Low", "High" }));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Anti-Aliasing");
        om.options.SetAntiAliasing(EditorGUILayout.Popup(om.options.GetAntiAliasing(), new string[] { "Off", "SMAA", "SMAA 2x", "SMAA 4x", "TAA" }));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Render Distance");
        om.options.SetRenderDistance(EditorGUILayout.IntField(om.options.GetRenderDistance()));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Resolution");
        Resolution[] resolutions = Screen.resolutions;
        int currentResolutionIndex = Array.FindIndex(resolutions, r => r.Equals(Screen.currentResolution));
        string[] resolutionsString = new string[resolutions.Length];
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionsString[i] = $"{resolutions[i].width}x{resolutions[i].height} @ {resolutions[i].refreshRate}";
        }
        om.options.SetResolution(resolutions[EditorGUILayout.Popup(currentResolutionIndex, resolutionsString)]);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Brightness");
        om.options.SetBrightness(EditorGUILayout.Slider(om.options.GetBrightness(), 0f, 1f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Field of View");
        om.options.SetFOV(EditorGUILayout.IntSlider(om.options.GetFOV(), 60, 110));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Quality Level");
        om.options.SetQualityLevel(EditorGUILayout.Popup(om.options.GetQualityLevel(), new string[] { "Very Low", "Low", "Medium", "High", "Very High", "Ultra", "Definitive" }));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mouse X Sensitivity");
        om.options.SetMouseXSensitivity(EditorGUILayout.Slider(om.options.GetMouseXSensitivity(), 0.1f, 10f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mouse Y Sensitivity");
        om.options.SetMouseYSensitivity(EditorGUILayout.Slider(om.options.GetMouseYSensitivity(), 0.1f, 10f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Invert Scroll");
        om.options.SetInvertScroll(EditorGUILayout.Popup(om.options.GetInvertScroll(), new string[] { "No", "Yes"}));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Master Volume");
        om.options.SetMasterVolume(EditorGUILayout.Slider(om.options.GetMasterVolume(), 0f, 1f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("FX Volume");
        om.options.SetFXVolume(EditorGUILayout.Slider(om.options.GetFXVolume(), 0f, 1f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Music Volume");
        om.options.SetMusicVolume(EditorGUILayout.Slider(om.options.GetMusicVolume(), 0f, 1f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Dialogue Volume");
        om.options.SetDialogueVolume(EditorGUILayout.Slider(om.options.GetDialogueVolume(), 0f, 1f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Block Observer");
        om.options.SetBlockObserver(EditorGUILayout.Popup(om.options.GetBlockObserver(), new string[] { "Off", "On" }));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
            om.SaveOptions();
        if (GUILayout.Button("Load"))
            om.LoadOptions();
        if (GUILayout.Button("Default"))
        {
            om.DeleteOptions();
            om.LoadOptions();
        }
        GUILayout.EndHorizontal();
    }

    void OnGUI()
    {
        if (!GameManager.instance)
        {
            GUILayout.Label("Game Manager not present", EditorStyles.boldLabel);
            return;
        }
        else if (!GameManager.instance.gameObject.GetComponent<OptionsManager>())
        {
            GUILayout.Label("Options Manager not present", EditorStyles.boldLabel);
            return;
        } 

        currentTabIndex = GUILayout.Toolbar(currentTabIndex, new string[] { "Off", "General" });

        switch (currentTabIndex)
        {
            case 0:
                ShowOff();
                break;
            case 1:
                ShowGeneral();
                break;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.EndScrollView();

    }
}
