/* Tasman
 *
 * Created by Liam HALL on 01/03/19.
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
using Newtonsoft.Json;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<Item> items;
    public List<Block> blocks;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(BuildDatabase());
    }

    public Item GetItem(int id)
    {
        foreach(Item item in items)
        {
            if (item.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public Block GetBlock(int id)
    {
        foreach(Block block in blocks)
        {
            if (block.ItemId == id)
            {
                return block;
            }
        }
        return null;
    }

    IEnumerator BuildDatabase()
    {
        var request = UnityEngine.Networking.UnityWebRequest.Get(Application.streamingAssetsPath + "/JSON/Items.json");
        yield return request.SendWebRequest();
        string json = request.downloadHandler.text;

        items = JsonConvert.DeserializeObject<List<Item>>(json);
        //items = JsonConvert.DeserializeObject<List<Item>>(Resources.Load<TextAsset>("JSON/Items").ToString()); LEGACY
        if (items[1].GetName() != "Dirt")
        {
            Debug.LogError("Item Database was unable to validate!");
        }

        request = UnityEngine.Networking.UnityWebRequest.Get(Application.streamingAssetsPath + "/JSON/Blocks.json");
        yield return request.SendWebRequest();
        json = request.downloadHandler.text;

        blocks = JsonConvert.DeserializeObject<List<Block>>(json);
        //blocks = JsonConvert.DeserializeObject<List<Block>>(Resources.Load<TextAsset>("JSON/Blocks").ToString());
        if (blocks[1].TexturePos.Top != new Vector2(0, 17))
        {
            Debug.LogError("Block Database was unable to validate!");
        }
    }
}
