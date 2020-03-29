/* Tasman
 *
 * Created by Liam HALL on 12/04/19.
 * Copyright © 2019 Liam HALL. All rights reserved.
 *
 * Coding Convention: Microsoft C# Coding Conventions
 * 
 * Contributors:
 *   Liam HALL
 */
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public static Players instance;

    public List<Player> players;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetPlayers();
    }

    void GetPlayers()
    {
        players = GetComponentsInChildren<Player>().ToList();
    }
}
