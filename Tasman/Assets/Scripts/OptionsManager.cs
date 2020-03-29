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
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class OptionsManager : MonoBehaviour
{
    public Options options = new Options();

    private void OnEnable()
    {
        LoadOptions();
    }

    public void LoadOptions()
    {
        if (ValidateOptions())
        {
            // Video Settings
            options.SetWindowed(PlayerPrefs.GetInt("WindowedMode"));
            options.SetShadows(PlayerPrefs.GetInt("Shadows"));
            options.SetAntiAliasing(PlayerPrefs.GetInt("AntiAliasing"));
            options.SetRenderDistance(PlayerPrefs.GetInt("RenderDistance"));
            options.SetResolution(PlayerPrefs.GetInt("ResolutionWidth"), PlayerPrefs.GetInt("ResolutionHeight"), PlayerPrefs.GetInt("ResolutionRefreshRate"));
            options.SetBrightness(PlayerPrefs.GetFloat("Brightness"));
            options.SetFOV(PlayerPrefs.GetInt("FOV"));
            options.SetQualityLevel(PlayerPrefs.GetInt("UnityGraphicsQuality"));

            // Input Settings
            options.SetMouseXSensitivity(PlayerPrefs.GetFloat("MouseXSensitivity"));
            options.SetMouseYSensitivity(PlayerPrefs.GetFloat("MouseYSensitivity"));
            options.SetInvertScroll(PlayerPrefs.GetInt("InvertScroll"));

            // Audio Settings
            options.SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume"));
            options.SetFXVolume(PlayerPrefs.GetFloat("FXVolume"));
            options.SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
            options.SetDialogueVolume(PlayerPrefs.GetFloat("DialogueVolume"));

            // Other Settings
            options.SetBlockObserver(PlayerPrefs.GetInt("BlockObserver"));

            PrintCurrentOptions();
        }
        else
        {
            Debug.LogError("Could not validate player options.");
        }
    }

    public void SaveOptions()
    {
        // Video Settings
        PlayerPrefs.SetInt("WindowedMode", options.GetWindowed());
        PlayerPrefs.SetInt("Shadows", options.GetShadows());
        PlayerPrefs.SetInt("AntiAliasing", options.GetAntiAliasing());
        PlayerPrefs.SetInt("RenderDistance", options.GetRenderDistance());
        var resolution = options.GetResolution();
        PlayerPrefs.SetInt("ResolutionWidth", resolution[0]);
        PlayerPrefs.SetInt("ResolutionHeight", resolution[1]);
        PlayerPrefs.SetInt("ResolutionRefreshRate", resolution[2]);
        PlayerPrefs.SetFloat("Brightness", options.GetBrightness());
        PlayerPrefs.SetInt("FOV", options.GetFOV());
        PlayerPrefs.SetInt("UnityGraphicsQuality", options.GetQualityLevel());

        // Input Settings
        PlayerPrefs.SetFloat("MouseXSensitivity", options.GetMouseXSensitivity());
        PlayerPrefs.SetFloat("MouseYSensitivity", options.GetMouseYSensitivity());
        PlayerPrefs.SetInt("InvertScroll", options.GetInvertScroll());

        // Audio Settings
        PlayerPrefs.SetFloat("MasterVolume", options.GetMasterVolume());
        PlayerPrefs.SetFloat("FXVolume", options.GetFXVolume());
        PlayerPrefs.SetFloat("MusicVolume", options.GetMusicVolume());
        PlayerPrefs.SetFloat("DialogueVolume", options.GetDialogueVolume());

        // Other Settings

        PrintSavedOptions();
        PlayerPrefs.SetInt("BlockObserver", options.GetBlockObserver());
    }

    void PrintCurrentOptions()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("CURRENT OPTIONS");

        // Video Settings
        sb.AppendLine($"WindowedMode - {options.GetWindowed()}");
        sb.AppendLine($"Shadows - {options.GetShadows()}");
        sb.AppendLine($"AntiAliasing - {options.GetAntiAliasing()}");
        sb.AppendLine($"RenderDistance - {options.GetRenderDistance()}");
        var resolution = options.GetResolution();
        sb.AppendLine($"ResolutionWidth - {resolution[0]}");
        sb.AppendLine($"ResolutionHeight - {resolution[1]}");
        sb.AppendLine($"ResolutionRefreshRate - {resolution[2]}");
        sb.AppendLine($"Brightness - {options.GetBrightness()}");
        sb.AppendLine($"FOV - {options.GetFOV()}");
        sb.AppendLine($"UnityGraphicsQuality - {options.GetQualityLevel()}");

        // Input Settings
        sb.AppendLine($"MouseXSensitivity - {options.GetMouseXSensitivity()}");
        sb.AppendLine($"MouseYSensitivity - {options.GetMouseYSensitivity()}");
        sb.AppendLine($"InvertScroll - {options.GetInvertScroll()}");

        // Audio Settings
        sb.AppendLine($"MasterVolume - {options.GetMasterVolume()}");
        sb.AppendLine($"FXVolume - {options.GetFXVolume()}");
        sb.AppendLine($"MusicVolume - {options.GetMusicVolume()}");
        sb.AppendLine($"DialogueVolume - {options.GetDialogueVolume()}");

        // Other Settings
        sb.AppendLine($"BlockObserver - {options.GetBlockObserver()}");

        print(sb);
    }

    void PrintSavedOptions()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("SAVED OPTIONS");

        // Video Settings
        sb.AppendLine($"WindowedMode - {PlayerPrefs.GetInt("WindowedMode")}");
        sb.AppendLine($"Shadows - {PlayerPrefs.GetInt("Shadows")}");
        sb.AppendLine($"AntiAliasing - {PlayerPrefs.GetInt("AntiAliasing")}");
        sb.AppendLine($"RenderDistance - {PlayerPrefs.GetInt("RenderDistance")}");
        sb.AppendLine($"ResolutionWidth - {PlayerPrefs.GetInt("ResolutionWidth")}");
        sb.AppendLine($"ResolutionHeight - {PlayerPrefs.GetInt("ResolutionHeight")}");
        sb.AppendLine($"ResolutionRefreshRate - {PlayerPrefs.GetInt("ResolutionRefreshRate")}");
        sb.AppendLine($"Brightness - {PlayerPrefs.GetFloat("Brightness")}");
        sb.AppendLine($"FOV - {PlayerPrefs.GetInt("FOV")}");
        sb.AppendLine($"UnityGraphicsQuality - {PlayerPrefs.GetInt("QualityLevel")}");

        // Input Settings
        sb.AppendLine($"MouseXSensitivity - {PlayerPrefs.GetFloat("MouseXSensitivity")}");
        sb.AppendLine($"MouseYSensitivity - {PlayerPrefs.GetFloat("MouseYSensitivity")}");
        sb.AppendLine($"InvertScroll - {PlayerPrefs.GetInt("InvertScroll")}");

        // Audio Settings
        sb.AppendLine($"MasterVolume - {PlayerPrefs.GetFloat("MasterVolume")}");
        sb.AppendLine($"FXVolume - {PlayerPrefs.GetFloat("FXVolume")}");
        sb.AppendLine($"MusicVolume - {PlayerPrefs.GetFloat("MusicVolume")}");
        sb.AppendLine($"DialogueVolume - {PlayerPrefs.GetFloat("DialogueVolume")}");

        // Other Settings
        sb.AppendLine($"BlockObserver - {PlayerPrefs.GetInt("BlockObserver")}");

        print(sb);
    }

    bool ValidateOptions()
    {
        List<bool> valid = new List<bool>();

        // Video Settings
        valid.Add(CreateOption("WindowedMode", 0));
        valid.Add(CreateOption("Shadows", 1));
        valid.Add(CreateOption("AntiAliasing", 1));
        valid.Add(CreateOption("RenderDistance", 128));
        valid.Add(CreateOption("ResolutionWidth", 1280));
        valid.Add(CreateOption("ResolutionHeight", 720));
        valid.Add(CreateOption("ResolutionRefreshRate", 60));
        valid.Add(CreateOption("Brightness", 0.5f));
        valid.Add(CreateOption("FOV", 75));
        valid.Add(CreateOption("UnityGraphicsQuality", 3));

        // Input Settings
        valid.Add(CreateOption("MouseXSensitivity", 2f));
        valid.Add(CreateOption("MouseYSensitivity", 2f));
        valid.Add(CreateOption("InvertScroll", 0));

        // Audio Settings
        valid.Add(CreateOption("MasterVolume", 1f));
        valid.Add(CreateOption("FXVolume", 1f));
        valid.Add(CreateOption("MusicVolume", 1f));
        valid.Add(CreateOption("DialogueVolume", 1f));

        // Other Settings
        valid.Add(CreateOption("BlockObserver", 1));

        return valid.All(element => element = true);
    }

    public void DeleteOptions()
    {
        // Video Settings
        PlayerPrefs.DeleteKey("WindowedMode");
        PlayerPrefs.DeleteKey("Shadows");
        PlayerPrefs.DeleteKey("AntiAliasing");
        PlayerPrefs.DeleteKey("RenderDistance");
        PlayerPrefs.DeleteKey("ResolutionWidth");
        PlayerPrefs.DeleteKey("ResolutionHeight");
        PlayerPrefs.DeleteKey("ResolutionRefreshRate");
        PlayerPrefs.DeleteKey("Brightness");
        PlayerPrefs.DeleteKey("FOV");

        // Input Settings
        PlayerPrefs.DeleteKey("MouseXSensitivity");
        PlayerPrefs.DeleteKey("MouseYSensitivity");
        PlayerPrefs.DeleteKey("InvertScroll");

        // Audio Settings
        PlayerPrefs.DeleteKey("MasterVolume");
        PlayerPrefs.DeleteKey("FXVolume");
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("DialogueVolume");

        // Other Settings
        PlayerPrefs.DeleteKey("BlockObserver");

        print("All Options deleted.");
    }

    bool CreateOption(string key, string value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
            return true;
        }
        else
            return true;
    }

    bool CreateOption(string key, int value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
            return true;
        }
        else
            return true;
    }

    bool CreateOption(string key, float value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
            return true;
        }
        else
            return true;
    }
}

public class Options
{
    // Video Settings
    int windowed; // 0 = windowed, 1 = maximised windowed, 2 = fullscreen window, 3 = fullscreen
    int shadows; // 0 = off, 1 = on
    int antiAliasing; // 0 = off, 1 = smaa, 2 = smaa x2, 3 = smaa x4, 4 = taa
    int renderDistance;
    Resolution resolution = new Resolution();
    float brightness;
    int fov;
    int qualityLevel;

    // Input Settings
    float mouseXSensitivity;
    float mouseYSensitivity;
    int invertScroll;

    // Audio Settings
    float masterVolume;
    float fxVolume;
    float musicVolume;
    float dialogueVolume;

    // Other Settings
    int blockObserver;


    public void SetWindowed(int value)
    {
        if (value >= 0 && value <= 2)
        {
            windowed = value;
            Screen.SetResolution(resolution.width, resolution.height, (FullScreenMode)windowed, resolution.refreshRate);
        }
    }

    public int GetWindowed()
    {
        return windowed;
    }

    public void SetShadows(int value)
    {
        if (value >= 0 && value <= 1)
            shadows = value;
    }

    public void ApplyShadows()
    {
        if (SceneManager.GetActiveScene().name == "Sandbox")
        {
            if (!GameManager.instance)
                return;

            var suns = GameObject.FindGameObjectsWithTag("Sun");

            if (suns.Length < 2)
                return;

            Light[] lights = new Light[suns.Length];

            for (int i = 0; i < lights.Length; i++)
                lights[i] = suns[i].GetComponent<Light>();

            switch (shadows)
            {
                case 0:
                    foreach (var light in lights)
                        light.shadows = LightShadows.None;
                    break;
                case 1:
                    foreach (var light in lights)
                        light.shadows = LightShadows.Hard;
                    break;
            }
        }
    }

    public int GetShadows()
    {
        return shadows;
    }

    public void SetAntiAliasing(int value)
    {
        if (value >= 0 && value <= 5)
        {
            antiAliasing = value;

            ApplyAntiAliasing();
        }
    }

    public void ApplyAntiAliasing()
    {
        if (SceneManager.GetActiveScene().name == "Sandbox")
        {
            if (!Players.instance)
                return;
            var pp = Players.instance.players[0].gameObject.GetComponentInChildren<PostProcessLayer>();

            if (!pp)
                return;

            switch (antiAliasing)
            {
                case 0:
                    pp.antialiasingMode = PostProcessLayer.Antialiasing.None;
                    break;
                case 1:
                    pp.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    pp.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Low;
                    break;
                case 2:
                    pp.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    pp.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Medium;
                    break;
                case 3:
                    pp.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    pp.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.High;
                    break;
                case 4:
                    pp.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                    break;
                default:
                    pp.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                    pp.subpixelMorphologicalAntialiasing.quality = SubpixelMorphologicalAntialiasing.Quality.Low;
                    break;
            }
        }
    }

    public int GetAntiAliasing()
    {
        return antiAliasing;
    }

    public void SetRenderDistance(int value)
    {
        if (value >= 128 && value <= 1024)
        {
            renderDistance = value;

            ApplyRenderDistance();
        }
    }

    public void ApplyRenderDistance()
    {
        if (SceneManager.GetActiveScene().name == "Sandbox")
        {
            if (!Players.instance)
                return;
            var mt = Players.instance.players[0].gameObject.GetComponentInChildren<ModifyTerrain>();
            if (!mt)
                return;
            mt.renderDistanceLoad = renderDistance;
            mt.renderDistanceUnload = renderDistance + 4;
        }
    }

    public int GetRenderDistance()
    {
        return renderDistance;
    }

    public void SetResolution(int width, int height, int refreshRate)
    {
        if (height > 0 && width > 0 && refreshRate > 0)
        {
            resolution.height = height;
            resolution.width = width;
            resolution.refreshRate = refreshRate;
            ApplyResolution();
        }
    }

    public void SetResolution(Resolution value)
    {
        if (value.height > 0 && value.width > 0 && value.refreshRate > 0)
        {
            resolution = value;
            ApplyResolution();
        }
    }

    public void ApplyResolution()
    {
        Screen.SetResolution(resolution.width, resolution.height, (FullScreenMode)windowed, resolution.refreshRate);
    }

    public int[] GetResolution()
    {
        int[] resolution = new int[3];
        resolution[0] = this.resolution.width;
        resolution[1] = this.resolution.height;
        resolution[2] = this.resolution.refreshRate;

        return resolution;
    }

    public void SetBrightness(float value)
    {
        if (value >= 0 && value <= 1)
            brightness = value;
    }

    public float GetBrightness()
    {
        return brightness;
    }

    public void SetMouseXSensitivity(float value)
    {
        if (value >= 0.1 && value <= 10)
        {
            mouseXSensitivity = value;

            ApplyMouseXSensitivity();
        }
    }

    public void ApplyMouseXSensitivity()
    {
        if (SceneManager.GetActiveScene().name == "Sandbox")
        {
            if (!Players.instance)
                return;
            if (!Players.instance.players[0].gameObject.GetComponentInChildren<FirstPersonController>())
                return;
            Players.instance.players[0].gameObject.GetComponentInChildren<FirstPersonController>().m_MouseLook.XSensitivity = mouseXSensitivity;
        }
    }

    public float GetMouseXSensitivity()
    {
        return mouseXSensitivity;
    }

    public void SetMouseYSensitivity(float value)
    {
        if (value >= 0.1 && value <= 10)
        {
            mouseYSensitivity = value;

            ApplyMouseYSensitivity();
        }
    }

    public void ApplyMouseYSensitivity()
    {
        if (SceneManager.GetActiveScene().name == "Sandbox")
        {
            if (!Players.instance)
                return;
            if (!Players.instance.players[0].gameObject.GetComponentInChildren<FirstPersonController>())
                return;
            Players.instance.players[0].gameObject.GetComponentInChildren<FirstPersonController>().m_MouseLook.YSensitivity = mouseYSensitivity;
        }
    }

    public float GetMouseYSensitivity()
    {
        return mouseYSensitivity;
    }

    public void SetMasterVolume(float value)
    {
        if (value >= 0 && value <= 1)
        {
            masterVolume = value;
            ApplyMasterVolume();
        }
    }

    public void ApplyMasterVolume()
    {
        AudioListener.volume = masterVolume;

    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void SetFXVolume(float value)
    {
        if (value >= 0 && value <= 1)
        {
            fxVolume = value;
            ApplyFXVolume();
        }
    }

    public void ApplyFXVolume()
    {
        var audioSourcesGO = GameObject.FindGameObjectsWithTag("FXASource");

        if (audioSourcesGO.Length < 1)
            return;

        AudioSource[] audioSources = new AudioSource[audioSourcesGO.Length];

        for (int i = 0; i < audioSources.Length; i++)
            audioSources[i] = audioSourcesGO[i].GetComponent<AudioSource>();

        foreach (var audioSource in audioSources)
            audioSource.volume = fxVolume;
    }

    public float GetFXVolume()
    {
        return fxVolume;
    }

    public void SetMusicVolume(float value)
    {
        if (value >= 0 && value <= 1)
            musicVolume = value;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetDialogueVolume(float value)
    {
        if (value >= 0 && value <= 1)
            dialogueVolume = value;
    }

    public float GetDialogueVolume()
    {
        return dialogueVolume;
    }

    public void SetFOV(int value)
    {
        if (value >= 60 && value <= 110)
        {
            fov = value;
            ApplyFOV();
        }
    }

    public void ApplyFOV()
    {
        if (SceneManager.GetActiveScene().name == "Sandbox")
        {
            if (!GameObject.FindGameObjectWithTag("MainCamera"))
                return;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = fov;
        }
    }

    public int GetFOV()
    {
        return fov;
    }

    public void SetQualityLevel(int value)
    {
        if (value >= 0 && value <= 6)
        {
            qualityLevel = value;
            ApplyQualityLevel();
        }
    }

    public void ApplyQualityLevel()
    {
        QualitySettings.SetQualityLevel(qualityLevel);
    }

    public int GetQualityLevel()
    {
        return qualityLevel;
    }

    public void SetInvertScroll(int value)
    {
        if (value >= 0 && value <= 1)
        {
            invertScroll = value;
        }
    }

    public int GetInvertScroll()
    {
        return invertScroll;
    }

    public void SetBlockObserver(int value)
    {
        if (value >= 0 && value <= 1)
        {
            blockObserver = value;
        }
    }

    public int GetBlockObserver()
    {
        return blockObserver;
    }
}