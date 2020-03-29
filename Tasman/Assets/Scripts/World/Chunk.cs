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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject worldGO;
    public int chunkSize;
    public int chunkX;
    public int chunkY;
    public int chunkZ;
    public bool update;

    public GameObject lightPrefab;

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();

    double tUnit = 0.0555555556d;

    private Mesh mesh;
    private MeshCollider col;

    private int faceCount;

    private GameObject[,,] lights;

    private World world;

    // Start is called before the first frame update
    void Start()
    {
        world = worldGO.GetComponent<World>();

        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        lights = new GameObject[chunkSize, chunkSize, chunkSize];

        GenerateMesh();
    }

    private void LateUpdate()
    {
        if (update)
        {
            GenerateMesh();
            update = false;
        }
    }

    void CubeTop(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));

        Vector2 texturePos = ItemDatabase.instance.blocks[Block(x, y, z)].TexturePos.Top;

        Cube(texturePos);
    }

    void CubeNorth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = ItemDatabase.instance.blocks[Block(x, y, z)].TexturePos.North;

        Cube(texturePos);
    }

    void CubeEast(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));

        Vector2 texturePos = ItemDatabase.instance.blocks[Block(x, y, z)].TexturePos.East;

        Cube(texturePos);
    }

    void CubeSouth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));

        Vector2 texturePos = ItemDatabase.instance.blocks[Block(x, y, z)].TexturePos.South;

        Cube(texturePos);
    }

    void CubeWest(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        Vector2 texturePos = ItemDatabase.instance.blocks[Block(x, y, z)].TexturePos.West;

        Cube(texturePos);
    }

    void CubeBottom(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = ItemDatabase.instance.blocks[Block(x, y, z)].TexturePos.Bottom;

        Cube(texturePos);
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();

        faceCount = 0; //Fixed: Added this thanks to a bug pointed out by ratnushock!
    }

    void Cube(Vector2 texturePos)
    {
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 1); //2
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4 + 3); //4*/

        newUV.Add(new Vector2((float)(tUnit * texturePos.x + tUnit), (float)tUnit * texturePos.y));
        newUV.Add(new Vector2((float)(tUnit * texturePos.x + tUnit), (float)(tUnit * texturePos.y + tUnit)));
        newUV.Add(new Vector2((float)(tUnit * texturePos.x), (float)(tUnit * texturePos.y + tUnit)));
        newUV.Add(new Vector2((float)(tUnit * texturePos.x), (float)(tUnit * texturePos.y)));

        faceCount++; // Add this line
    }

    public void GenerateMesh()
    {

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    //This code will run for every block in the chunk

                    if (Block(x, y, z) != 0)
                    {
                        //If the block is solid
                        if (Block(x, y + 1, z) == 0)
                        {
                            //Block above is air
                            CubeTop(x, y, z, Block(x, y, z));
                        }

                        if (Block(x, y - 1, z) == 0)
                        {
                            //Block below is air
                            CubeBottom(x, y, z, Block(x, y, z));

                        }

                        if (Block(x + 1, y, z) == 0)
                        {
                            //Block east is air
                            CubeEast(x, y, z, Block(x, y, z));

                        }

                        if (Block(x - 1, y, z) == 0)
                        {
                            //Block west is air
                            CubeWest(x, y, z, Block(x, y, z));

                        }

                        if (Block(x, y, z + 1) == 0)
                        {
                            //Block north is air
                            CubeNorth(x, y, z, Block(x, y, z));

                        }

                        if (Block(x, y, z - 1) == 0)
                        {
                            //Block south is air
                            CubeSouth(x, y, z, Block(x, y, z));

                        }
                    }

                    if (Block(x, y, z) == 9)
                    {
                        if (lights[x, y, z] == null)
                        {
                            GameObject lightGO = Instantiate(lightPrefab, transform.position, transform.rotation);
                            lightGO.transform.parent = transform;

                            lights[x, y, z] = lightGO;
                            lightGO.transform.position = new Vector3(lightGO.transform.position.x + x, lightGO.transform.position.y + y, lightGO.transform.position.z + z);

                        }

                        TLight light = lights[x, y, z].GetComponent<TLight>();

                        if (Block(x, y + 1, z) == 0)
                        {
                            //Block above is air
                            light.SetFace(TLight.Face.Top, true);
                        }
                        else
                            light.SetFace(TLight.Face.Top, false);

                        if (Block(x, y - 1, z) == 0)
                        {
                            //Block below is air
                            light.SetFace(TLight.Face.Bottom, true);
                        }
                        else
                            light.SetFace(TLight.Face.Bottom, false);

                        if (Block(x + 1, y, z) == 0)
                        {
                            //Block east is air
                            light.SetFace(TLight.Face.East, true);
                        }
                        else
                            light.SetFace(TLight.Face.East, false);

                        if (Block(x - 1, y, z) == 0)
                        {
                            //Block west is air
                            light.SetFace(TLight.Face.West, true);
                        }
                        else
                            light.SetFace(TLight.Face.West, false);

                        if (Block(x, y, z + 1) == 0)
                        {
                            //Block north is air
                            light.SetFace(TLight.Face.North, true);
                        }
                        else
                            light.SetFace(TLight.Face.North, false);

                        if (Block(x, y, z - 1) == 0)
                        {
                            //Block south is air
                            light.SetFace(TLight.Face.South, true);
                        }
                        else
                            light.SetFace(TLight.Face.South, false);
                    }
                    else
                    {
                        Destroy(lights[x, y, z]);
                        lights[x, y, z] = null;
                    }
                }
            }
        }

        UpdateMesh();
    }

    byte Block(int x, int y, int z)
    {
        return world.Block(x + chunkX, y + chunkY, z + chunkZ);
    }
}
