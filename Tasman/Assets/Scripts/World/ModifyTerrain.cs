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

public class ModifyTerrain : MonoBehaviour
{
    World world;
    GameObject cameraGO;
    bool paused;
    bool canMine;
    bool canPlace;
    public enum InteractState { None, Mining, Placing };
    public InteractState interactState;
    float nextPlaceCooldown;
    bool interacting;
    public int renderDistanceLoad;
    public int renderDistanceUnload;

    float mineCooldown = 0.12f;
    float placeCooldown = 0.16f;
    float mineHoldDelay = 0.2f;
    float placeHoldDelay = 0.3f;
    float currentMineDelay;
    float currentPlaceDelay;
    float currentMineHoldDelay;
    float currentPlaceHoldDelay;
    float mineDelay = 0.1f;
    bool enablePlaceHold;
    bool enableMineHold;
    byte previousLookedAtBlock = 0;

    public bool enableObserverCooldown = true;
    float observerCooldown = 0.05f /* ~20fps*/, currentObserverCooldown;
    
    public GameObject hoverPrefab;
    GameObject hoverInstance;
    Vector3Int previousHoverPosition = new Vector3Int();

    PlayerInventory pi;

    void Start()
    {
        world = World.instance;
        cameraGO = gameObject.GetComponentInChildren<Camera>().gameObject;
        pi = gameObject.GetComponentInParent<PlayerInventory>();
        hoverInstance = Instantiate(hoverPrefab);
    }

    private void Update()
    {
        LoadChunks(GameObject.FindGameObjectWithTag("MainCamera").transform.position, renderDistanceLoad, renderDistanceUnload);
        Modify();
        GetCurrentlyLookedAtBlock();
    }

    void GetCurrentlyLookedAtBlock()
    {
        currentObserverCooldown += Time.deltaTime;

        float range = 5;

        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                Vector3 position = hit.point;
                Vector3Int positionInt = new Vector3Int();
                position += (hit.normal * -0.5f);
                positionInt.x = Mathf.RoundToInt(position.x);
                positionInt.y = Mathf.RoundToInt(position.y);
                positionInt.z = Mathf.RoundToInt(position.z);

                if (previousHoverPosition != positionInt)
                {
                    hoverInstance.transform.position = positionInt;
                    previousHoverPosition = positionInt;

                    if (!enableObserverCooldown || currentObserverCooldown >= observerCooldown)
                    {                   
                        if (GameManager.instance.GetComponent<OptionsManager>().options.GetBlockObserver() > 0)
                        {                      
                            byte currentLookedAtBlock = world.data[positionInt.x, positionInt.y, positionInt.z];

                            if (previousLookedAtBlock != currentLookedAtBlock)
                            {
                                previousLookedAtBlock = currentLookedAtBlock;
                                pi.hudUI.ChangeBlockObserver(currentLookedAtBlock);
                            }
                        }

                        currentObserverCooldown = 0f;
                    }
                }

                
            }
            else
            {
                pi.hudUI.ChangeBlockObserver(0);
                previousLookedAtBlock = 0;
                Vector3 position = hoverInstance.transform.position;            
                hoverInstance.transform.position = new Vector3(position.x, -999, position.z);
            }        
        }
    }

    void Modify()
    {
        if (Input.GetButtonDown("Secondary Action") || Input.GetAxis("Secondary Action2") > 0)
        {
            interactState = InteractState.Placing;
            SecondaryAction();
            currentPlaceDelay = 0;
        }

        else if (Input.GetButton("Secondary Action"))
        {
            if (!enablePlaceHold)
            {
                currentPlaceHoldDelay += Time.deltaTime;

                if (currentPlaceHoldDelay >= placeHoldDelay)
                {
                    enablePlaceHold = true;
                }
            }
            else
            {
                currentPlaceDelay += Time.deltaTime;

                if (currentPlaceDelay >= placeCooldown)
                {
                    interactState = InteractState.Placing;
                    SecondaryAction();
                    currentPlaceDelay = 0;
                }
            }
        }

        else if (Input.GetButtonDown("Primary Action") || Input.GetAxis("Primary Action2") > 0)
        {
            interactState = InteractState.Mining;
            PrimaryAction();
            currentMineDelay = 0;
        }

        else if (Input.GetButton("Primary Action"))
        {
            if (!enableMineHold)
            {
                currentMineHoldDelay += Time.deltaTime;

                if (currentMineHoldDelay >= mineHoldDelay)
                {
                    enableMineHold = true;
                }
            }
            else
            {
                currentMineDelay += Time.deltaTime;

                if (currentMineDelay >= mineCooldown)
                {
                    interactState = InteractState.Mining;
                    currentMineDelay = 0;
                    PrimaryAction();
                }
            }
        }

        if (Input.GetButtonUp("Secondary Action"))
        {
            currentPlaceDelay = 0;
            currentPlaceHoldDelay = 0;
            enablePlaceHold = false;
        }

        if (Input.GetButtonUp("Primary Action"))
        {
            currentMineDelay = 0;
            currentMineHoldDelay = 0;
            enableMineHold = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            pi.SetCreativeMode(!pi.GetCreativeMode());
        }
    }

    void SingleSecondaryAction()
    {

    }

    void PrimaryAction()
    {
        //ReplaceBlockCursor(5, 0);
        ReplaceBlockCenter(5, 0);
        pi.InventoryChanged();
    }

    void SecondaryAction()
    {
        //AddBlockCursor(5, (byte)gameObject.GetComponent<PlayerInventory>().inventorySlot[gameObject.GetComponent<PlayerInventory>().selectedSlot, 0]);
        AddBlockCenter(5, (byte)pi.inventorySlot[pi.GetSelectedSlot()].ItemID);    
    }

    bool CanBlockBePlaced(RaycastHit hit)
    {
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = Mathf.RoundToInt(position.z);

        Vector3 playerPosition = gameObject.transform.position;
        playerPosition.x = Mathf.RoundToInt(playerPosition.x);
        playerPosition.y = Mathf.RoundToInt(playerPosition.y);
        playerPosition.z = Mathf.RoundToInt(playerPosition.z);

        if (position.x == playerPosition.x && position.z == playerPosition.z && (position.y == playerPosition.y || position.y == playerPosition.y + 1))
        {
            return false;
        }

        return true;
    }

    public void ReplaceBlockCenter(float range, byte block)
    {
        //Replaces the block directly in front of the player
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                ReplaceBlockAt(hit, block);
            }
        }
    }

    public void AddBlockCenter(float range, byte block)
    {
        //Adds the block specified directly in front of the player
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5))
        {
            if (CanBlockBePlaced(hit))
            {
                AddBlockAt(hit, block);
                print(hit.distance);
                print(hit.transform.name);

                if (!pi.GetCreativeMode())
                {
                    pi.inventorySlot[pi.GetSelectedSlot()].Quantity--;
                }

                pi.InventoryChanged();               
            }

            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }

    public void ReplaceBlockCursor(float range, byte block)
    {
        //Replaces the block specified where the mouse cursor is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5))
        {
            if (hit.distance < range)
            {
                ReplaceBlockAt(hit, block);
            }
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }

    public void AddBlockCursor(float range, byte block)
    {
        //Adds the block specified where the mouse cursor is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                AddBlockAt(hit, block);
            }
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance),  Color.green, 2);
        }
    }

    public void ReplaceBlockAt(RaycastHit hit, byte block)
    {
        //removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
        Vector3 position = hit.point;
        position += (hit.normal * -0.5f);

        byte currentBlock = world.data[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z)];
        print("current block: " + currentBlock);
        for (int i = 0; i < ItemDatabase.instance.GetBlock(currentBlock).Drops.Length; i++)
        {
            int drop = ItemDatabase.instance.GetBlock(currentBlock).Drops[i];
            print("drops for: " + ItemDatabase.instance.GetItem(currentBlock).ItemName + " are " + ItemDatabase.instance.GetBlock(currentBlock).Drops);
            if (drop != 0)
            {
                print("drop: " + drop);
                Item item = ItemDatabase.instance.GetItem(drop);
                print("dropped item: " + item);
                print($"Dropped {item.ItemName}");

                if (!pi.GetCreativeMode())
                {
                    pi.AddToInventory(item.Id, 1);
                }
            }
        }

        SetBlockAt(position, block);
        hoverInstance.transform.position = new Vector3(0, -999, 0);
    }

    public void AddBlockAt(RaycastHit hit, byte block)
    {
        //adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);

        SetBlockAt(position, block);
    }

    public void SetBlockAt(Vector3 position, byte block)
    {
        //sets the specified block at these coordinates
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        SetBlockAt(x, y, z, block);
    }

    public void SetBlockAt(int x, int y, int z, byte block)
    {
        //adds the specified block at these coordinates
        world.data[x, y, z] = block;
        UpdateChunkAt(x, y, z);
    }

    public void UpdateChunkAt(int x, int y, int z)
    {
        //Updates the chunk containing this block
        int updateX = Mathf.FloorToInt(x / world.chunkSize);
        int updateY = Mathf.FloorToInt(y / world.chunkSize);
        int updateZ = Mathf.FloorToInt(z / world.chunkSize);

        world.chunks[updateX, updateY, updateZ].update = true;

        if (x - (world.chunkSize * updateX) == 0 && updateX != 0)
        {
            world.chunks[updateX - 1, updateY, updateZ].update = true;
        }

        if (x - (world.chunkSize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1)
        {
            world.chunks[updateX + 1, updateY, updateZ].update = true;
        }

        if (y - (world.chunkSize * updateY) == 0 && updateY != 0)
        {
            world.chunks[updateX, updateY - 1, updateZ].update = true;
        }

        if (y - (world.chunkSize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1)
        {
            world.chunks[updateX, updateY + 1, updateZ].update = true;
        }

        if (z - (world.chunkSize * updateZ) == 0 && updateZ != 0)
        {
            world.chunks[updateX, updateY, updateZ - 1].update = true;
        }

        if (z - (world.chunkSize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1)
        {
            world.chunks[updateX, updateY, updateZ + 1].update = true;
        }
    }

    public void LoadChunks(Vector3 playerPos, float distToLoad, float distToUnload)
    {
        for (int x = 0; x < world.chunks.GetLength(0); x++)
        {
            for (int z = 0; z < world.chunks.GetLength(2); z++)
            {
                float dist = Vector2.Distance(new Vector2(x * world.chunkSize,
                z * world.chunkSize), new Vector2(playerPos.x, playerPos.z));

                if (dist < distToLoad)
                {
                    if (world.chunks[x, 0, z] == null)
                    {
                        world.GenColumn(x, z);
                    }
                }
                else if (dist > distToUnload)
                {
                    if (world.chunks[x, 0, z] != null)
                    {

                        world.UnloadColumn(x, z);
                    }
                }
            }
        }
    }
}
