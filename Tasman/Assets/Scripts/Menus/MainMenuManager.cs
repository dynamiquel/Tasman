using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameObject myWorldsMenu;
    public GameObject createWorldMenu;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this);
    }

    public void LoadMenu(int ID)
    {
        switch (ID)
        {
            case 0:
                OpenMyWorldsMenu();
                break;
            case 1:
                OpenCreateWorldMenu();
                break;
        }
    }

    void OpenMyWorldsMenu()
    {
        SetCreateWorldMenu(false);
        SetMyWorldsMenu(true);
    }

    void OpenCreateWorldMenu()
    {
        SetMyWorldsMenu(false);
        SetCreateWorldMenu(true);
    }

    void SetMyWorldsMenu(bool x)
    {
        myWorldsMenu.SetActive(x);
    }

    void SetCreateWorldMenu(bool x)
    {
        createWorldMenu.SetActive(x);
    }
}
