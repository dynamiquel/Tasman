/* Tasman
 *
 * Created by Liam HALL on 28/02/19.
 * Copyright © 2019 Liam HALL. All rights reserved.
 *
 * Coding Convention: Microsoft C# Coding Conventions
 * 
 * Contributors:
 *   Liam HALL
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class World : MonoBehaviour
{
    public static World instance;
    public bool worldLoaded;
    public string name;
    public bool cheatsEnabled; 
    public byte[,,] data;
    public int worldX = 16;
    public int worldY = 16;
    public int worldZ = 16;
    public DateTime creationDate, lastSaved;
    public int playtime;

    public enum WorldType
    {
        Flat,
        Normal
    };
    public WorldType worldType = WorldType.Normal;

    public enum WorldDifficulty
    {
        Peaceful,
        Easy,
        Normal,
        Hard
    };
    public WorldDifficulty worldDifficulty;

    public enum WorldGameMode
    {
        Survival,
        Creative
    };
    public WorldGameMode worldGameMode;

    public GameObject chunk;
    public Chunk[,,] chunks;
    public int chunkSize = 16;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    /*void Start()
    {
        season = 2;
        second = 300;

        data = new byte[worldX, worldY, worldZ];

        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 7, 0) + 1; // Added +1 to make sure minimum grass height is 1

                for (int y = 1; y < worldY; y++)
                {
                    if (worldType == WorldType.Normal)
                    {
                        if (y == 1)
                        {
                            data[x, y, z] = 5; // Bedrock
                        }
                        else if (y <= stone)
                        {
                            data[x, y, z] = 3; // Stone
                        }
                        else if (y <= dirt)
                        {
                            data[x, y, z] = 1; // Dirt
                        }
                        else if (y <= dirt + stone)
                        { //Changed this line thanks to a comment
                            data[x, y, z] = 2;
                        }
                    }
                    else
                    {
                        if (y == 1)
                        {
                            data[x, y, z] = 5;
                        }
                        if (y > 1 && y <= 4) //y <= 4, then stone
                        {
                            data[x, y, z] = 3;
                        }
                        if (y <= 7 && y > 4)
                        {
                            data[x, y, z] = 1;
                        }
                        if (y == 8)
                        {
                            data[x, y, z] = 2;
                        }
                    }
                }
            }

            chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize),
            Mathf.FloorToInt(worldY / chunkSize),
            Mathf.FloorToInt(worldZ / chunkSize)];
        }

        CreateBorders();
    }*/

    public void OnWorldLoad()
    {
        season = 2;
        second = 300;

        for (int x = 0; x < worldX; x++)
        {
            chunks = new Chunk[Mathf.FloorToInt(worldX / chunkSize),
            Mathf.FloorToInt(worldY / chunkSize),
            Mathf.FloorToInt(worldZ / chunkSize)];
        }

        CreateBorders();

        worldLoaded = true;
    }

    public byte Block(int x, int y, int z)
    {

        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return (byte) 1;
        }

        return data[x, y, z];
    }

    int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }

    public void GenColumn(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            //Create a temporary Gameobject for the new chunk instead of using chunks[x,y,z]
            GameObject newChunk = Instantiate(chunk, new Vector3(gameObject.transform.position.x + x * chunkSize - 0.5f,
                y * chunkSize + 0.5f, gameObject.transform.position.z + z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
            newChunk.transform.SetParent(gameObject.transform);

            //Now instead of using a temporary variable for the script assign it
            //to chunks[x,y,z] and use it instead of the old \"newChunkScript\" 
            chunks[x, y, z] = newChunk.GetComponent<Chunk>();
            chunks[x, y, z].worldGO = gameObject;
            chunks[x, y, z].chunkSize = chunkSize;
            chunks[x, y, z].chunkX = x * chunkSize;
            chunks[x, y, z].chunkY = y * chunkSize;
            chunks[x, y, z].chunkZ = z * chunkSize;         
        }      
    }

    public void UnloadColumn(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            UnityEngine.Object.Destroy(chunks[x, y, z].gameObject);
        }
    }

    // Sets how long a day and season should be
    int secondsPerDay = 1440;
    int daysPerSeason = 25;
    int[] seasonDaytimeLength = { 480, 720, 960, 600 };

    // Current counters
    float second = 0;
    int day = 0;
    int season = 0;

    // Days since the creation of the world
    int daysSinceCreation = 0;

    // Time multiplier
    public float timeMultiplier = 1;

    double dayPercentage;
    double daytimePercentage;
    double nighttimePercentage;

    public GameObject volumeSettings;
    public GameObject sun;
    public GameObject moon;

    private void FixedUpdate()
    {
        if (!worldLoaded)
            return;

        UpdateTime();
        CheckDay();
        CheckSeason();
        UpdateDaytimePercentages();

        sun.transform.localRotation = Quaternion.Euler((float)(daytimePercentage * 220 - 20), sun.transform.localRotation.y, sun.transform.localRotation.z);
        moon.transform.localRotation = Quaternion.Euler((float)(nighttimePercentage * 220 - 20), moon.transform.localRotation.y, moon.transform.localRotation.z);

        if (daytimePercentage == 1)
        {
            sun.SetActive(false);
        }
        else
        {
            sun.SetActive(true);
        }
        
    }

    void CheckDay()
    {
        if (second >= secondsPerDay)
        {
            second -= secondsPerDay;
            day++;
            daysSinceCreation++;
        }
    }

    void CheckSeason()
    {
        if (day >= daysPerSeason)
        {
            day -= daysPerSeason;

            if (season >= 3)
                season = 0;
            else
                season++;
        }
    }

    void UpdateTime()
    {
        second += Time.deltaTime * timeMultiplier;
    }

    void UpdateDaytimePercentages()
    {
        dayPercentage = second / secondsPerDay;

        if (second <= seasonDaytimeLength[season])
        {
            nighttimePercentage = 1;
            daytimePercentage = second / seasonDaytimeLength[season];
        }
        else
        {
            daytimePercentage = 1;
            nighttimePercentage = (second - seasonDaytimeLength[season]) / (secondsPerDay - seasonDaytimeLength[season]);
        }
    }

    void CreateBorders()
    {
        GameObject[] borderQuads = { GameObject.CreatePrimitive(PrimitiveType.Quad), GameObject.CreatePrimitive(PrimitiveType.Quad), GameObject.CreatePrimitive(PrimitiveType.Quad), GameObject.CreatePrimitive(PrimitiveType.Quad), GameObject.CreatePrimitive(PrimitiveType.Quad) };

        GameObject borderParent = new GameObject("Borders");
        borderParent.transform.SetParent(gameObject.transform.parent);
        
        for (int i = 0; i < borderQuads.Length; i++)
        {
            borderQuads[i].GetComponent<MeshRenderer>().enabled = false;
            borderQuads[i].transform.SetParent(borderParent.transform);

            if (i == 4)
            {
                borderQuads[i].transform.localScale = new Vector3(worldX, worldZ, 0);
            }
            else
            {
                borderQuads[i].transform.localScale = new Vector3(worldX, worldY * 2, 0);
            }
        }

        borderQuads[0].transform.position = new Vector3(worldX / 2 - 0.5f, 0, 10.5f);
        borderQuads[1].transform.position = new Vector3(worldX / 2 - 0.5f, 0, worldZ - 10.5f);
        borderQuads[2].transform.position = new Vector3(10.5f, 0, worldZ / 2 - 0.5f);
        borderQuads[3].transform.position = new Vector3(worldX - 10.5f, 0, worldZ / 2 - 0.5f);

        borderQuads[0].transform.rotation = new Quaternion(0, 1, 0, 0);
        borderQuads[2].transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);
        borderQuads[3].transform.rotation = new Quaternion(0, 0.7f, 0, 0.7f);

        borderQuads[4].transform.position = new Vector3(worldX / 2, worldY, worldZ / 2);
        borderQuads[4].transform.rotation = new Quaternion(-0.7f, 0, 0, 0.7f);  
    }
}
