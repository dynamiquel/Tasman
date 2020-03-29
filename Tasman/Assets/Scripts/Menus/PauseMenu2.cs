using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu2 : Menu
{
    public bool dialogOpen;

    private void OnEnable()
    {
        DisplayWorldName();
    }

    void DisplayWorldName()
    {
        if (World.instance)
            subheading.text = World.instance.name;
        else
            subheading.text = "ERROR: CANNOT ACCESS WORLD PROPERTIES";
    }

    public void ButtonClicked(int buttonID)
    {
        if (buttonID == 0)
        {
            ButtonClick(0);
            dialogOpen = false;
            Players.instance.players[0].gameObject.GetComponent<MenuManager>().PauseMenuToggle();
        }
        else if (buttonID == 3)
        {
            ButtonClick(0);
            dialogOpen = true;
            StartCoroutine(CreateQuickDialog("SAVE WORLD", "Do you really want to save this world?", "YES", "NO", 0, SaveWorld));
        }
        else if (buttonID == 4)
        {
            ButtonClick(0);
            dialogOpen = true;
            StartCoroutine(CreateQuickDialog("EXIT WORLD", "Do you want to return to the main menu?", "YES", "NO", 0, ExitWorld));
        }
    }

    void ExitWorld()
    {
        Destroy(GameManager.instance.gameObject.GetComponent<WorldSave>());
        SceneManager.LoadSceneAsync(0);
    }

    void SaveWorld()
    {
        WorldSave worldSave = new WorldSave();
        worldSave.SaveWorldClick();
    }
}
