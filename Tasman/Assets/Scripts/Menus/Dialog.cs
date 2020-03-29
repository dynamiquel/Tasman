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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dialogTitle, dialogDescription, dialogOption1, dialogOption2;

    public byte answer = 0;

    public float timeout = -1;

    private void Start()
    {
        StartCoroutine(WaitForTimeoutInit());
    }

    IEnumerator WaitForTimeoutInit()
    {
        yield return new WaitUntil(() => timeout >= 0);

        if (timeout > 0)
        {
            StartCoroutine(Timeout());
        }
    }

    private void LateUpdate()
    {
        if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Option2Clicked();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Option1Clicked();
        }
    }

    public void SetTitle(string title)
    {
        dialogTitle.text = title;
    }

    public void SetDescription(string description)
    {
        dialogDescription.text = description;
    }

    public void SetOption1Text(string text)
    {
        dialogOption1.text = text;
    }

    public void SetOption2Text(string text)
    {
        dialogOption2.text = text;
    }

    public void SetTimeout(float timeout)
    {
        this.timeout = timeout;
    }

    public void SetAllValues(string title, string description, string option1Text, string option2Text, float timeout)
    {
        SetTitle(title);
        SetDescription(description);
        SetOption1Text(option1Text);
        SetOption2Text(option2Text);
        SetTimeout(timeout);
    }

    public void Option1Clicked()
    {
        answer = 1;
    }

    public void Option2Clicked()
    {
        answer = 2;
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeout);

        Option2Clicked();
    }
}
