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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StringParameter : MonoBehaviour
{
    public int optionID;
    public TMP_InputField inputField;

    public string GetValue()
    {
        if (inputField)
        {
            print("Found IF");
            return inputField.text;
        }

        return "";
    }

    public byte GetValueAsNum()
    {
        byte result = 0;

        if (inputField)
        {
            byte.TryParse(inputField.text, out result);
        }

        return result;
    }

    public void SetValueAsNum(byte value)
    {
        inputField.text = value.ToString();
    }
}
