/* Tasman
 *
 * Created by Liam HALL on 26/03/19.
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
using UnityEngine;
using System.Linq;

[Serializable]
public class InventorySlot
{
    public int ItemID { get; set; }
    public int Quantity { get; set; }

    public InventorySlot(int itemID, int quantity)
    {
        ItemID = itemID;
        Quantity = quantity;
    }
}

public class PlayerInventory : MonoBehaviour
{
    #region Inventory
    // Inventory slots; 0 = ItemID, 1 = Quantity
    //public int[,] inventorySlot = new int[32, 2] { { 1, 1 }, { 2, 1 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, { 6, 1 }, { 7, 1 }, { 8, 1 }, { 9, 1 }, { 10, 1 }, { 11, 1 }, { 12, 1 }, { 13, 1 }, { 14, 1 }, { 15, 1 }, { 16, 1 }, { 17, 1 }, { 18, 1 }, { 19, 1 }, { 20, 1 }, { 21, 1 }, { 22, 1 }, { 23, 1 }, { 24, 1 }, { 25, 1 }, { 26, 1 }, { 27, 1 }, { 28, 1 }, { 29, 1 }, { 30, 1 }, { 31, 1 }, { 32, 1 } };
    public List<InventorySlot> inventorySlot = new List<InventorySlot>(capacity: 32);

    // The inventory slot that the user is currently selecting in the hot bar
    byte selectedSlot = 0;

    public HUDUI hudUI;

    void PrintInventory()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("INVENTORY");

        for (int i = 0; i < inventorySlot.Count; i++)
        {
            sb.AppendLine($"Inventory Slot {i}: {ItemDatabase.instance.GetItem(inventorySlot[i].ItemID).ItemName} x {inventorySlot[i].Quantity}");
        }

        print(sb);

        if (hudUI)
        {
            hudUI.ChangeInventoryBarItems(GetInventoryBarInventoryWithQuantity());
            hudUI.SelectedInventorySlotChanged(selectedSlot, inventorySlot[selectedSlot].ItemID, GetTotalCountOfItem(inventorySlot[selectedSlot].ItemID));
        }
    }

    // Detecs user input
    void UserInput()
    {
        if (isDead)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
            SortInventoryByQuantityThenID();

        if (GameManager.instance.gameObject.GetComponent<OptionsManager>().options.GetInvertScroll() == 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                if (selectedSlot == 8)
                    ChangeSelectedInventorySlot(0);
                else
                    ChangeSelectedInventorySlot((byte)(selectedSlot + 1));
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                if (selectedSlot == 0)
                    ChangeSelectedInventorySlot(8);
                else
                    ChangeSelectedInventorySlot((byte)(selectedSlot - 1));
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                if (selectedSlot == 8)
                    ChangeSelectedInventorySlot(0);
                else
                    ChangeSelectedInventorySlot((byte)(selectedSlot + 1));
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                if (selectedSlot == 0)
                    ChangeSelectedInventorySlot(8);
                else
                    ChangeSelectedInventorySlot((byte)(selectedSlot - 1));
        }

        if (Input.GetButtonDown("Next Item"))
            if (selectedSlot == 8)
                ChangeSelectedInventorySlot(0);
            else
                ChangeSelectedInventorySlot((byte)(selectedSlot + 1));
        else if (Input.GetButtonDown("Previous Item"))
            if (selectedSlot == 0)
                ChangeSelectedInventorySlot(8);
            else
                ChangeSelectedInventorySlot((byte)(selectedSlot - 1));

        // Changes the selected inventory slot
        if (Input.GetButtonDown("Item 1"))
            ChangeSelectedInventorySlot(0);
        else if (Input.GetButtonDown("Item 2"))
            ChangeSelectedInventorySlot(1);
        else if (Input.GetButtonDown("Item 3"))
            ChangeSelectedInventorySlot(2);
        else if (Input.GetButtonDown("Item 4"))
            ChangeSelectedInventorySlot(3);
        else if (Input.GetButtonDown("Item 5"))
            ChangeSelectedInventorySlot(4);
        else if (Input.GetButtonDown("Item 6"))
            ChangeSelectedInventorySlot(5);
        else if (Input.GetButtonDown("Item 7"))
            ChangeSelectedInventorySlot(6);
        else if (Input.GetButtonDown("Item 8"))
            ChangeSelectedInventorySlot(7);
        else if (Input.GetButtonDown("Item 9"))
            ChangeSelectedInventorySlot(8);
    }

    void ChangeSelectedInventorySlot(byte selectedSlot)
    {
        this.selectedSlot = selectedSlot;
        print($"Selected Item: {ItemDatabase.instance.GetItem(inventorySlot[selectedSlot].ItemID).ItemName}");
        hudUI.SelectedInventorySlotChanged(selectedSlot, inventorySlot[selectedSlot].ItemID, GetTotalCountOfItem(inventorySlot[selectedSlot].ItemID));
    }

    public byte GetSelectedSlot()
    {
        return selectedSlot;
    }

    // Returns the ItemID for the selected inventory slot index
    public int GetInventoryItem(int slotIndex)
    {
        return inventorySlot[slotIndex].ItemID;
    }

    // Changes the ItemID for the selected inventory slot index
    public void ChangeInventoryItem(int slotIndex, int itemID)
    {
        inventorySlot[slotIndex].ItemID = itemID;
    }

    // Returns the entire inventory slot of the selected inventory slot index
    public int[,] GetInventoryItemWithQuantity(int slotIndex)
    {
        int[,] invSlot = new int[1, 1];
        invSlot[0, 0] = inventorySlot[slotIndex].ItemID;
        invSlot[0, 1] = inventorySlot[slotIndex].Quantity;
        return invSlot;
    }

    // Returns the entire user's inventory (without quantity)
    public int[] GetEntireInventory()
    {
        int[] inv = new int[inventorySlot.Count];

        for (int i = 0; i < inventorySlot.Count; i++)
        {
            inv[i] = inventorySlot[i].ItemID;
        }

        return inv;
    }

    // Returns the entire user's inventory
    public List<InventorySlot> GetEntireInventoryWithQuantity()
    {
        return inventorySlot;
    }

    public int[,] GetInventoryBarInventoryWithQuantity()
    {
        int[,] inv = new int[9, 2];

        for (int i = 0; i < 9; i++)
        {
            inv[i, 0] = inventorySlot[i].ItemID;
            inv[i, 1] = inventorySlot[i].Quantity;
        }

        return inv;
    }

    public void InventoryChanged()
    {
        CheckForEmptySlots();
        CheckIfOverStackLimit();
        hudUI.ChangeInventoryBarItems(GetInventoryBarInventoryWithQuantity());
        hudUI.ChangeSelectedInventorySlotItemName(inventorySlot[selectedSlot].ItemID);
        hudUI.ChangeSelectedInventorySlotItemQuantity(GetTotalCountOfItem(inventorySlot[selectedSlot].ItemID));
    }

    // Checks for any empty slots in the inventory and changes them to air
    void CheckForEmptySlots()
    {
        for (int i = 0; i < inventorySlot.Count; i++)                // For every inventory slot
        {
            if (inventorySlot[i].Quantity <= 0)                                   // If quantity <= 0, set to air and 0
            {
                inventorySlot[i].ItemID = 0;
                inventorySlot[i].Quantity = 0;
            }
            if (inventorySlot[i].ItemID == 0 && inventorySlot[i].Quantity != 0)       // If its air, set its quantity to 0
            {
                inventorySlot[i].Quantity = 0;
            }
        }
    }

    // Checks if any of the inventory items are over the stack limit; creates a 'leftOver' variable if there is
    void CheckIfOverStackLimit()
    {
        for (int i = 0; i < inventorySlot.Count; i++)                // For every inventory slot
        {
            if (inventorySlot[i].ItemID != 0 && inventorySlot[i].Quantity > 64)       // If it is not air and is over the stack limit, then
            {
                int leftOver = inventorySlot[i].Quantity - 64;                    // Calculate how much is left over
                inventorySlot[i].Quantity = 64;                                   // Set the current slot's quantity to the stack limit

                leftOver = CheckForSimilarSlot(inventorySlot[i].ItemID, leftOver); // Attempts to find another similar slot to push the left over into; returns how much didn't get pushed

                if (leftOver <= 0)                                          // If all left over got pushed, then stop
                    return;

                CheckForEmptySlot(inventorySlot[i].ItemID, leftOver);           // Attempts to find an empty slot to push the left over into; returns how much didn't get pushed

                if (leftOver <= 0)                                          // If all left over got pushes, then stop
                    return;

                print($"Dropped {ItemDatabase.instance.GetItem(inventorySlot[i].ItemID).ItemName} x { leftOver}"); // Announce how much left over got dropped
            }
        }
    }

    // Looks for a similar slot to put left over items into; left over is returned
    int CheckForSimilarSlot(int itemID, int leftOver)
    {
        for (int i = 0; i < inventorySlot.Count; i++)                // For every inventory slot
        {
            if (inventorySlot[i].ItemID == itemID)                              // If the inventory slot has the same item
            {
                leftOver = PushLeftOverIntoSlot(i, leftOver);               // Push as much left over to the slot as possible; return how much didn't get pushed

                if (leftOver <= 0)                                          // If all left over has been pushed, then stop
                {
                    return leftOver;
                }                                                           // Else, look for another similar slot to push the left over into                                                                          
            }
        }

        return leftOver;                                                    // Returns the left over so program can calculate how much left over didn't get pushed
    }

    // Looks for an empty slot to put left over items into; left over is returned
    int CheckForEmptySlot(int itemID, int leftOver)
    {
        for (int i = 0; i < inventorySlot.Count; i++)                // For every inventory slot
        {
            if (inventorySlot[i].ItemID == 0)                                   // If the inventory slot is empty
            {
                ChangeInventoryItem(i, itemID);                             // Change its itemID to the itemID of the left over
                leftOver = PushLeftOverIntoSlot(i, leftOver);               // Push as much left over to the slot as possible; return how much didn't get pushed

                if (leftOver <= 0)                                          // If all left over has been pushed, then stop
                {
                    return leftOver;
                }                                                           // Else, look for another similar slot to push the left over into                                                                          

            }
        }

        return leftOver;                                                    // Returns the left over so program can calculate how much left over didn't get pushed
    }

    int PushLeftOverIntoSlot(int i, int leftOver)
    {
        int maxInput = 64 - inventorySlot[i].Quantity;                        // Calculate how many items the slot can take

        if (maxInput >= leftOver)                                       // If they can take more than the left over needs, then set maxInput to leftOver
        {
            maxInput = leftOver;
            leftOver = 0;
        }
        else                                                            // Else, take away the maxInput from leftOver so it can be returned
        {
            leftOver -= maxInput;
        }

        inventorySlot[i].Quantity += maxInput;                                // Push as much left over to the slot as possible

        return leftOver;                                                // Returns the left over so program can calculate how much left over didn't get pushed
    }

    public void AddToInventory(int itemID, int itemQuantity)
    {
        itemQuantity = CheckForSimilarSlot(itemID, itemQuantity);

        if (itemQuantity <= 0)                                          // If all left over got pushed, then stop
            return;

        itemQuantity = CheckForEmptySlot(itemID, itemQuantity);

        if (itemQuantity <= 0)                                          // If all left over got pushed, then stop
            return;

        print($"Dropped {ItemDatabase.instance.GetItem(itemID).ItemName} x { itemQuantity}"); // Announce how much left over got dropped
    }

    void SortInventoryByQuantityThenID()
    {
        SortInventoryByQuantity();
        SortInventoryByID();
    }

    void SortInventoryByID()
    {
        var hotBar = GetHotbarInventory();
        var notHotBar = GetNotHotbarInventory();
        notHotBar = notHotBar.OrderBy(x => x.ItemID).ToList();
        inventorySlot = CombineInventoryLists(hotBar, notHotBar);
        CompressInventory();
        InventoryChanged();
    }

    void SortInventoryByQuantity()
    {
        var hotBar = GetHotbarInventory();
        var notHotBar = GetNotHotbarInventory();
        notHotBar = notHotBar.OrderByDescending(x => x.Quantity).ToList();
        inventorySlot = CombineInventoryLists(hotBar, notHotBar);
        CompressInventory();
        InventoryChanged();
    }

    void SortInventoryByAlphabet()
    {
        CompressInventory();
        var hotBar = GetHotbarInventory();
        var notHotBar = GetNotHotbarInventory();
        notHotBar = notHotBar.OrderBy(x => ItemDatabase.instance.GetItem(x.ItemID).ItemName).ToList();
        inventorySlot = CombineInventoryLists(hotBar, notHotBar);
        InventoryChanged();
    }

    void CompressInventory()
    {
        var notHotBar = CreateUnqiueList(GetNotHotbarInventory());
        notHotBar = DivideUniqueListIntoRealList(notHotBar);
        notHotBar = FillInventoryListWithEmptySlots(notHotBar);
        inventorySlot = CombineInventoryLists(GetHotbarInventory(), notHotBar);
    }

    List<InventorySlot> CreateUnqiueList(List<InventorySlot> inventorySlot)
    {
        List<InventorySlot> uniqueList = new List<InventorySlot>(capacity: 24);

        for (int i = 0; i < inventorySlot.Count; i++)
        {
            if (inventorySlot[i].ItemID != 0)
            {
                if (uniqueList.FindIndex(x => x.ItemID == inventorySlot[i].ItemID) < 0)
                {
                    uniqueList.Add(inventorySlot[i]);
                    print($"{inventorySlot[i].ItemID} with {inventorySlot[i].Quantity} was added to unique list. {uniqueList}");
                }
                else
                {
                    print($"Adding {inventorySlot[i].ItemID}");
                    uniqueList[uniqueList.FindIndex(x => x.ItemID == inventorySlot[i].ItemID)].Quantity += inventorySlot[i].Quantity;
                    print($"Added {inventorySlot[i].ItemID}");
                }
            }
        }

        return uniqueList;
    }

    List<InventorySlot> DivideUniqueListIntoRealList(List<InventorySlot> uniqueList)
    {
        List<InventorySlot> newList = new List<InventorySlot>(capacity: 24);

        for (int i = 0; i < uniqueList.Count; i++)
        {
            int quantity = uniqueList[i].Quantity;
            int stackLimit = ItemDatabase.instance.GetItem(uniqueList[i].ItemID).GetStackLimit();

            while (quantity > 0)
            {
                if (quantity >= stackLimit)
                {
                    newList.Add(new InventorySlot(uniqueList[i].ItemID, stackLimit));
                    quantity -= stackLimit;
                }
                else
                {
                    newList.Add(new InventorySlot(uniqueList[i].ItemID, quantity));
                    quantity = 0;
                }
            }
        }

        return newList;
    }

    List<InventorySlot> FillInventoryListWithEmptySlots(List<InventorySlot> newList)
    {
        if (newList.Count < 32)
        {
            for (int i = newList.Count - 1; i < 24; i++)
            {
                newList.Add(new InventorySlot(0, 0));
            }
        }

        return newList;
    }

    List<InventorySlot> CombineInventoryLists(List<InventorySlot> hotBar, List<InventorySlot> notHotBar)
    {
        List<InventorySlot> temp = new List<InventorySlot>(capacity: 32);
        temp.AddRange(hotBar);
        temp.AddRange(notHotBar);

        return temp;
    }

    List<InventorySlot> GetHotbarInventory()
    {
        List<InventorySlot> hotbar = new List<InventorySlot>(inventorySlot);
        hotbar.RemoveRange(9, hotbar.Count - 9);

        return hotbar;
    }

    List<InventorySlot> GetNotHotbarInventory()
    {
        List<InventorySlot> newInv = new List<InventorySlot>(inventorySlot);
        newInv.RemoveRange(0, 9);

        return newInv;
    }

    int GetTotalCountOfItem(int itemID)
    {
        int totalCount = 0;

        foreach (var iS in GetEntireInventoryWithQuantity())
            if (iS.ItemID == itemID)
                totalCount += iS.Quantity;

        return totalCount;
    }
    #endregion

    #region General
    string profileID;
    DateTime dateCreated;
    string versionCreated;
    float secondsPlayed;
    float secondsSinceLastDeath;

    float health;
    public float foodLevel;
    float foodSaturationLevel;
    float foodExhaustionLevel;
    float sleepLevel;
    float sleepTickTimer;

    bool isCreative;

    float exp;
    int expLevel;

    Vector3 originalSpawnLocation;
    Vector3 currentSpawnLocation;
    bool useOriginalSpawnLocation;
    Vector3 currentLocation;
    bool isDead;

    float walkSpeed;
    float sprintSpeed;
    float flySpeed;
    bool canFly;
    bool invulnerable;
    bool instaDestroy;

    // Non-saved data
    float maxHealth = 20;
    float maxFoodLevel = 20;
    float maxFoodTimer = 4.0f;
    float maxSleepLevel = 20;
    float maxSleepTimer = 100;
    float foodTickTimer;
    float foodDrainTickTimer;
    float voidTickTimer;

    bool newProfile;
    bool playerLoaded;

    List<Vector3Int> previousDeaths = new List<Vector3Int>();

    public GameObject playerControllerPrefab;

    GameObject playerController;

    private void Awake()
    {
        print("PlayerData Woke");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckForWorldLoad());
    }

    IEnumerator CheckForWorldLoad()
    {
        yield return new WaitUntil(() => World.instance.worldLoaded && playerLoaded);

        if (newProfile)
        {
            CreateSpawnPoint();
            currentSpawnLocation = originalSpawnLocation;
            currentLocation = currentSpawnLocation;
            newProfile = false;
        }
        CreatePlayerData();
        PrintInventory();
        SpawnPlayer(false);
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
    }

    private void LateUpdate()
    {
        LateUpdatePlayerController();
    }

    private void FixedUpdate()
    {
        FixedUpdatePlayerController();
    }

    void LateUpdatePlayerController()
    {
        if (playerController)
        {
            currentLocation = playerController.transform.position; // Saves player's current location
        }

        secondsPlayed += Time.deltaTime;
        secondsSinceLastDeath += Time.deltaTime;
    }

    void FixedUpdatePlayerController()
    {
        if (playerController)
        {
            UpdateVoid();
            UpdateFoodLevel();
        }
    }

    void UpdateVoid()
    {
        if (playerController.transform.position.y < -5) // If player is under map
        {
            voidTickTimer += Time.deltaTime;

            if (voidTickTimer >= 0.2)
            {
                VoidDamagePlayer(1); // Deal damage to player
                voidTickTimer = 0;
            }
        }
    }

    void UpdateFoodLevel()
    {
        if (GetFoodLevel() >= 18 && GetHealth() < 20)
        {
            foodTickTimer += Time.deltaTime;

            if (foodTickTimer >= maxFoodTimer)
            {
                if (GetHealth() < 20)
                {
                    SetHealth(GetHealth() + 1);
                }

                foodTickTimer = 0;
            }
        }
        else if (GetFoodLevel() <= 0 && World.instance.worldDifficulty != World.WorldDifficulty.Peaceful)
        {
            if (World.instance.worldDifficulty == World.WorldDifficulty.Easy)
            {
                if (GetHealth() > 10)
                {
                    foodTickTimer += Time.deltaTime;

                    if (foodTickTimer >= maxFoodTimer)
                    {
                        DamagePlayer(1);
                        foodTickTimer = 0;
                    }
                }
            }
            else if (World.instance.worldDifficulty == World.WorldDifficulty.Normal)
            {
                if (GetHealth() > 1)
                {
                    foodTickTimer += Time.deltaTime;

                    if (foodTickTimer >= maxFoodTimer)
                    {
                        DamagePlayer(1);
                        foodTickTimer = 0;
                    }
                }
            }
            else if (World.instance.worldDifficulty == World.WorldDifficulty.Hard)
            {
                foodTickTimer += Time.deltaTime;

                if (foodTickTimer >= maxFoodTimer)
                {
                    DamagePlayer(1);
                    foodTickTimer = 0;
                }
            }
        }
        else
        {
            foodTickTimer = 0;
        }

        if (foodLevel < 20 && World.instance.worldDifficulty == World.WorldDifficulty.Peaceful)
        {
            foodDrainTickTimer += Time.deltaTime;

            if (foodDrainTickTimer >= maxFoodTimer)
            {
                AddHunger(1);
                foodDrainTickTimer = 0;
            }
        }
        else
        {
            foodDrainTickTimer = 0;
        }
    }

    void AddHunger(int newHunger)
    {
        if (GetFoodLevel() + newHunger < maxFoodLevel)
        {
            SetFoodLevel(GetFoodLevel() +  newHunger);
        }
        else
        {
            SetFoodLevel(newHunger);
        }
    }

    // Initialises the player's data
    void CreatePlayerData()
    {
        /*health = maxHealth;
        foodLevel = maxFoodLevel;
        foodTickTimer = 0;
        sleepLevel = maxSleepLevel;
        sleepTickTimer = maxSleepTimer;
        isCreative = true;
        currentSpawnLocation = originalSpawnLocation;*/
        walkSpeed = 1;
        sprintSpeed = 1;
        flySpeed = 1;
        canFly = false;
        invulnerable = false;
        instaDestroy = false;
        Time.timeScale = 1;
    }

    public void SetHealth(float x)
    {
        health = x;

        if (hudUI)
        {
            hudUI.HealthChanged(GetHealth());
        }

        if (GetHealth() <= 0) // If health is zero...
        {
            health = 0; // Ensures health doesn't go below zero
            OnPlayerDeath();
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetFoodLevel(float x)
    {
        foodLevel = x;

        if (hudUI)
        {
            hudUI.FoodLevelChanged(GetFoodLevel());
        }
    }

    public float GetFoodLevel()
    {
        return foodLevel;
    }

    public void SetFoodTimer(float x)
    {
        foodTickTimer = x;
    }

    public float GetFoodTimer()
    {
        return foodTickTimer;
    }

    public void SetSleepLevel(float x)
    {
        sleepLevel = x;
    }

    public float GetSleepLevel()
    {
        return sleepLevel;
    }

    public void SetSleepTimer(float x)
    {
        sleepTickTimer = x;
    }

    public float GetSleepTimer()
    {
        return sleepTickTimer;
    }

    public void SetCreativeMode(bool x)
    {
        isCreative = x;

        if (hudUI)
        {
            hudUI.GameModeChanged(isCreative);
        }
    }

    public bool GetCreativeMode()
    {
        return isCreative;
    }

    public void SetCurrentSpawnLocation(float x, float y, float z)
    {
        currentSpawnLocation.Set(x, y, z);
    }

    public Vector3 GetCurrentSpawnLocation()
    {
        return currentSpawnLocation;
    }

    public void SetWalkSpeed(float x)
    {
        walkSpeed = x;
    }

    public float GetWalkSpeed()
    {
        return walkSpeed;
    }

    public void SetSprintSpeed(float x)
    {
        sprintSpeed = x;
    }

    public float GetSprintSpeed()
    {
        return sprintSpeed;
    }

    public void SetFlySpeed(float x)
    {
        flySpeed = x;
    }

    public float GetFlySpeed()
    {
        return flySpeed;
    }

    public void SetCanFly(bool x)
    {
        canFly = x;
    }

    public bool GetCanFly()
    {
        return canFly;
    }

    public void SetInvulnerability(bool x)
    {
        invulnerable = x;
    }

    public bool GetInvulnerability()
    {
        return invulnerable;
    }

    public void SetInstaDestroy(bool x)
    {
        instaDestroy = x;
    }

    public bool GetInstaDestroy()
    {
        return instaDestroy;
    }

    public float GetSecondsSinceLastDeath()
    {
        return secondsSinceLastDeath;
    }

    public float GetSecondsPlayed()
    {
        return secondsPlayed;
    }

    // Creates a player character controller
    void SpawnPlayer(bool spawnAtSpawnPoint)
    {
        if (spawnAtSpawnPoint)
            playerController = Instantiate(playerControllerPrefab, currentSpawnLocation, Quaternion.identity); // Spawn the player at spawn point
        else
            playerController = Instantiate(playerControllerPrefab, currentLocation, Quaternion.identity); // Spawn the player at last location

        playerController.transform.SetParent(gameObject.transform); // Set the player's parent to this

        if (hudUI)
        {
            hudUI.GameModeChanged(isCreative);
            hudUI.HealthChanged(health);
            hudUI.FoodLevelChanged(foodLevel);
            hudUI.SleepLevelChanged(sleepLevel);
        }
    }

    // Create the spawn point for the player
    void CreateSpawnPoint()
    {
        Vector3Int worldSize = new Vector3Int(World.instance.worldX, World.instance.worldY, World.instance.worldZ); // Get the dimensions of the World
        int worldCentre = worldSize.x / 2; // Finds the world's centre by halving

        bool spawnFound = false;

        while (!spawnFound) // While no spawn is found, find one
        {
            Vector3Int position = FindSpawnPoint(worldCentre); // Gets a spawn point to try

            for (int y = worldSize.y - 1; y > 1; y--) // For every y block on the x and z position; from top to bottom
            {
                if (World.instance.data[position.x, y, position.z] != 0) // If the block is not air
                {
                    position.y = y + 1; // Calculates the player's y spawn position by adding 1 blocks on top of the non-air position
                    originalSpawnLocation = position; // Sets the player's original spawn location to the new found one
                    spawnFound = true; // Ends while loop
                    return; // Ends for loop
                }
            }
        }


        /* LEGACY SPAWN POINT CREATOR (BOTTOM TO TOP)
        bool spawnFound = false;

        while (!spawnFound) // While no spawn is found, find one
        {
            Vector3Int position = FindSpawnPoint(worldCentre); // Gets a spawn point to try

            for (int y = 0; y < worldSize.y; y++) // For every y block on the x and z position...
            {
                if (World.instance.data[position.x, y, position.z] == 2) // If the block is not air
                {
                    position.y = y + 2; // Calculates the player's y spawn position by adding 2 blocks on top of the grass position.
                    originalSpawnLocation = position; // Sets the player's original spawn location to the new found one
                    spawnFound = true; // Ends while loop
                }
            }
        }*/
    }

    // Finds a random spawn point within a radius of the world centre
    Vector3Int FindSpawnPoint(int worldCentre)
    {
        Vector3Int position = new Vector3Int(UnityEngine.Random.Range(worldCentre - 10, worldCentre + 10), 0, UnityEngine.Random.Range(worldCentre - 10, worldCentre + 10));
        return position;
    }

    // Called when player dies
    void OnPlayerDeath()
    {
        isDead = true;
        gameObject.GetComponent<MenuManager>().SetPlayerDeathState(true);
        AddDeathToList(); // Adds the player's death location to the death list
        PlayerEnabled(false);
    }

    public void RespawnPlayer()
    {
        if (isDead)
        {
            Destroy(playerController); // Destorys the current player controller
            ResetPlayerData(); // Resets the player's values for respawn
            SpawnEligibility(); // Checks if the spawn point is broken and needs to be changed
            isDead = false;
            gameObject.GetComponent<MenuManager>().HideDeathMenu();

            SpawnPlayer(true); // Spawn the player again
        }
    }

    void PlayerEnabled(bool b)
    {
        if (playerController)
        {
            MonoBehaviour[] scripts = playerController.GetComponentsInChildren<MonoBehaviour>();

            foreach (MonoBehaviour sc in scripts)
            {
                sc.enabled = b;
            }
        }
    }

    // Adds the player's most recent death to a list
    void AddDeathToList()
    {
        while (previousDeaths.Count > 9) // Makes sure there are only a max of 10 entries
        {
            previousDeaths.RemoveAt(0); // Removes the left most entry
        }

        previousDeaths.Add(new Vector3Int(Mathf.RoundToInt(currentLocation.x), Mathf.RoundToInt(currentLocation.y), Mathf.RoundToInt(currentLocation.z))); // Adds the death location to the list
    }

    // Checks if the player's current spawn point is broken
    void SpawnEligibility()
    {
        if (PreviousDeathsInSomeLocation()) // If last few deaths are in same place, then...
        {
            CreateSpawnPoint(); // Create a new spawn point
            currentSpawnLocation = originalSpawnLocation; // Updates the player's current spawn location
        }
    }

    // Checks if the player has died in the same place for a few times in a row
    bool PreviousDeathsInSomeLocation()
    {
        int previousDeathsCount = previousDeaths.Count;

        if (previousDeathsCount > 2) // If the player has died more than twice, then....
        { 
            // Gets the three most recent deaths
            Vector2Int[] mostRecentDeaths = { new Vector2Int(previousDeaths[previousDeathsCount - 1].x, previousDeaths[previousDeathsCount - 1].z), new Vector2Int(previousDeaths[previousDeathsCount - 2].x, previousDeaths[previousDeathsCount - 2].z), new Vector2Int(previousDeaths[previousDeathsCount - 3].x, previousDeaths[previousDeathsCount - 3].z) };

            if (mostRecentDeaths[0] == mostRecentDeaths[1] && mostRecentDeaths[0] == mostRecentDeaths[2]) // If the most recent deaths are the same, then...
            {
                return true;
            }
        }

        return false;
    }

    // Resets the player data; ready for respawn
    void ResetPlayerData()
    {
        secondsSinceLastDeath = 0;
        health = maxHealth;
        foodLevel = maxFoodLevel;
        foodTickTimer = 0;
        sleepLevel = maxSleepLevel;
        sleepTickTimer = 0;
        exp = 0;
        expLevel = 0;
        isDead = false;
        Time.timeScale = 1;
    }

    // Damages the player for a certain amount of damage
    public void DamagePlayer(float damage)
    {
        if (!isCreative || !invulnerable)
        {
            if (!isDead)
            {
                SetHealth(GetHealth() - damage);             
            }
        }
    }

    // Damages the player for a certain amount of damage (bypasses creative mode)
    public void VoidDamagePlayer(float damage)
    {
        if (!isDead)
        {
            SetHealth(GetHealth() - damage);
        }
    }
    #endregion

    public void SetData(bool newProfile, string profileID, DateTime dateCreated, string versionCreated, float secondsPlayed, float secondsSinceLastDeath, float health, float foodLevel, float foodSaturationLevel, float foodExhaustionLevel, float sleepLevel, float sleepTickTimer, bool isCreative, float exp, Vector3 originalSpawnLocation, Vector3 currentSpawnLocation, bool useOriginalSpawnLocation, Vector3 currentLocation, bool isDead, List<InventorySlot> inventorySlots, List<Vector3Int> previousDeaths)
    {
        this.newProfile = newProfile;
        this.profileID = profileID;
        this.dateCreated = dateCreated;
        this.versionCreated = versionCreated;
        this.secondsPlayed = secondsPlayed;
        this.secondsSinceLastDeath = secondsSinceLastDeath;
        SetHealth(health);
        SetFoodLevel(foodLevel);
        this.foodSaturationLevel = foodSaturationLevel;
        this.foodExhaustionLevel = foodExhaustionLevel;
        SetSleepLevel(sleepLevel);
        this.sleepTickTimer = sleepTickTimer;
        SetCreativeMode(isCreative);
        this.exp = exp;
        this.originalSpawnLocation = originalSpawnLocation;
        SetCurrentSpawnLocation(currentSpawnLocation.x, currentSpawnLocation.y, currentSpawnLocation.z);
        this.useOriginalSpawnLocation = useOriginalSpawnLocation;
        this.currentLocation = currentLocation;
        this.isDead = isDead;
        this.inventorySlot = inventorySlots;
        this.previousDeaths = previousDeaths;
        playerLoaded = true;
    }

    public PlayerData GetPlayerData()
    {
        PlayerData.Vector3Ser originalSpawnLocation = new PlayerData.Vector3Ser(this.originalSpawnLocation.x, this.originalSpawnLocation.y, this.originalSpawnLocation.z);
        PlayerData.Vector3Ser currentSpawnLocation = new PlayerData.Vector3Ser(this.currentSpawnLocation.x, this.currentSpawnLocation.y, this.currentSpawnLocation.z);
        PlayerData.Vector3Ser currentLocation = new PlayerData.Vector3Ser(this.currentLocation.x, this.currentLocation.y, this.currentLocation.z);
        List<PlayerData.Vector3IntSer> previousDeaths = new List<PlayerData.Vector3IntSer>();
        for (int i = 0; i < this.previousDeaths.Count; i++)
            previousDeaths.Add(new PlayerData.Vector3IntSer(this.previousDeaths[i].x, this.previousDeaths[i].y, this.previousDeaths[i].z));

        PlayerData playerData = new PlayerData(newProfile, profileID, dateCreated, versionCreated, secondsPlayed, secondsSinceLastDeath, health, foodLevel, foodSaturationLevel, foodExhaustionLevel, sleepLevel, sleepTickTimer, isCreative, exp, originalSpawnLocation, currentSpawnLocation, useOriginalSpawnLocation, currentLocation, isDead, inventorySlot, previousDeaths);
        return playerData;
    }
}
