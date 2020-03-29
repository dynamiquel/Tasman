/* Tasman
 *
 * Created by Liam HALL on 24/05/19.
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
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldButton : MonoBehaviour
{
    public TextMeshProUGUI worldNameText, worldGamemodeText, worldDateText, worldImageText, modifyText;
    public Image worldImage, modifyImage;
    public GameObject selectButton, modifyButton;
    public bool isNewWorldButton;
    public int buttonNumber;

    private void Start()
    {
        CheckButtonType();
    }

    void CheckButtonType()
    {
        if (isNewWorldButton)
        {
            worldImage.enabled = false;
            worldImageText.enabled = true;
            worldGamemodeText.enabled = false;
            worldDateText.enabled = false;
            worldNameText.text = "Create New World";
            worldNameText.gameObject.transform.position.Set(0, 66, 0);
            modifyButton.SetActive(false);
        }
        else
        {
            worldImage.enabled = true;
            worldImageText.enabled = false;
            worldGamemodeText.enabled = true;
            worldDateText.enabled = true;
            modifyButton.SetActive(true);
        }
    }

    public void SelectButtonClicked()
    {
        gameObject.GetComponentInParent<MyWorldsMenu>().SelectButtonClicked(buttonNumber);
    }

    public void ModifyButtonClicked()
    {
        gameObject.GetComponentInParent<MyWorldsMenu>().ModifyButtonClicked(buttonNumber);
    }

    public void SetWorldName(string t)
    {
        worldNameText.text = t;
    }

    public string GetWorldName()
    {
        return worldNameText.text;
    }

    public void SetWorldGamemode(string t)
    {
        worldGamemodeText.text = t;
    }

    public string GetWorldGamemode()
    {
        return worldGamemodeText.text;
    }

    public void SetWorldDate(string t)
    {
        worldDateText.text = t;
    }

    public string GetWorldDate()
    {
        return worldDateText.text;
    }

    public void SetWorldImage(Sprite s)
    {
        worldImage.sprite = s;
    }

    public Sprite GetWorldImage()
    {
        return worldImage.sprite;
    }
}
