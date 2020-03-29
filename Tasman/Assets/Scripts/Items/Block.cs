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

public class Block: MonoBehaviour
{
    public int ItemId { get; set; }
    public int[] Drops { get; set; }
    /*public bool Physics { get; set; }
    public bool Flammable { get; set; }
    Tool effectiveTool { get; set; }
    public int HarvestLevel { get; set; }
    public double Hardness { get; set; }
    public double Lumiance { get; set; }
    */
    public TexturePos TexturePos { get; set; }

    [Newtonsoft.Json.JsonConstructor]
    public Block(int itemId, int[] drops, /*bool physics, bool flammable, int harvestLevel, double hardness, double lumiance,*/ int[,] texturePos)
    {
        ItemId = itemId;
        Drops = drops;
        /*Physics = physics;
        Flammable = flammable;
        HarvestLevel = harvestLevel;
        Hardness = hardness;
        Lumiance = lumiance;*/
        TexturePos = new TexturePos(texturePos);
    }

    void Harvest()
    {

    }

    void Pop()
    {

    }
}

public class TexturePos
{
    public Vector2 Top { get; set; }
    public Vector2 Bottom { get; set; }
    public Vector2 North { get; set; }
    public Vector2 East { get; set; }
    public Vector2 South { get; set; }
    public Vector2 West { get; set; }

    public TexturePos(int[,] texturePos)
    {
        int length = texturePos.GetLength(0);
        switch (length)
        {
            case 1:
                Top = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                Bottom = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                North = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                East = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                South = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                West = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                break;
            case 2:
                Top = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                Bottom = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                North = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                East = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                South = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                West = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                break;
            case 3:
                Top = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                Bottom = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                North = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                East = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                South = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                West = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                break;
            case 4:
                Top = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                Bottom = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                North = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                East = new Vector2(texturePos[3, 0], texturePos[3, 1]);
                South = new Vector2(texturePos[3, 0], texturePos[3, 1]);
                West = new Vector2(texturePos[3, 0], texturePos[3, 1]);
                break;
            case 5:
                Top = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                Bottom = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                North = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                East = new Vector2(texturePos[3, 0], texturePos[3, 1]);
                South = new Vector2(texturePos[4, 0], texturePos[4, 1]);
                West = new Vector2(texturePos[3, 0], texturePos[3, 1]);
                break;
            case 6:
                Top = new Vector2(texturePos[0, 0], texturePos[0, 1]);
                Bottom = new Vector2(texturePos[1, 0], texturePos[1, 1]);
                North = new Vector2(texturePos[2, 0], texturePos[2, 1]);
                East = new Vector2(texturePos[3, 0], texturePos[3, 1]);
                South = new Vector2(texturePos[4, 0], texturePos[4, 1]);
                West = new Vector2(texturePos[5, 0], texturePos[5, 1]);
                break;
            default:
                Debug.LogError("Block texture missing.");
                break;
        }
    }
}
