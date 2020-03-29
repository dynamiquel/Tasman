using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(transform.parent.gameObject);
        DontDestroyOnLoad(transform.parent.gameObject);
    }

    public string userID = "1";
    public string userName = "";

    private void Start()
    {
        StartCoroutine(GetXboxUserID());
    }

    IEnumerator GetXboxUserID()
    {
        while (userID == "1")
        {
            if (SignInManager.Instance.GetCurrentNumberOfPlayers() >= 1)
            {
                XboxLiveUser xboxLiveUser = SignInManager.Instance.GetPlayer(1);
                userID = xboxLiveUser.XboxUserId;
                userName = xboxLiveUser.Gamertag;
            }
            yield return null;
        }

        yield return 0;
    }
}
