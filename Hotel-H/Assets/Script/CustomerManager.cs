using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform tablePoint;
    public float spawnDelay = 10f;
    public List<Room> rooms;
    private List<CustomerBehaviour> queuedCustomers = new List<CustomerBehaviour>();
    private List<Room> dirtyRooms = new List<Room>();
    private LevelProgress levelProgress;

    void Start()
    {
        levelProgress = FindObjectOfType<LevelProgress>();
        if (levelProgress == null)
        {
            Debug.LogError("LevelProgress not found!");
            return;
        }
        UpdateRoomAvailability();
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            if (queuedCustomers.Count < 5)
            {
                GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
                CustomerBehaviour customer = customerObj.GetComponent<CustomerBehaviour>();
                queuedCustomers.Add(customer);
                Debug.Log($"Spawned customer {customer.gameObject.name}. Queue count: {queuedCustomers.Count}");
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public Vector3 GetEntryQueuePosition(CustomerBehaviour customer)
    {
        int queueIndex = queuedCustomers.IndexOf(customer);
        if (queueIndex == -1)
        {
            queueIndex = queuedCustomers.Count;
        }
        return spawnPoint.position + new Vector3(queueIndex * 2f, 0, 0);
    }

    public bool IsFirstInQueue(CustomerBehaviour customer)
    {
        return queuedCustomers.Count > 0 && queuedCustomers[0] == customer;
    }

    public Room AssignRoom()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        int roomsToUnlock = currentLevel;

        foreach (Room room in rooms)
        {
            int roomIndex = rooms.IndexOf(room) + 1;
            if (roomIndex <= roomsToUnlock && room.IsAvailable())
            {
                room.SetOccupied(true);
                Debug.Log($"Assigned room {roomIndex} at {room.transform.position} to customer");
                return room;
            }
        }
        Debug.LogWarning("No unlocked, empty, and cleaned rooms available!");
        return null;
    }

    public void NotifyRoomDirty(Room room)
    {
        if (!dirtyRooms.Contains(room))
        {
            dirtyRooms.Add(room);
            room.SetClean(false);
            Debug.Log($"Room at {room.transform.position} marked dirty. Dustbin should appear.");

            // We no longer directly tell the player to clean - they'll handle it via dustbin interaction
        }
    }

    public void RoomCleaned(Room room) // New method to clear dirty status
    {
        if (dirtyRooms.Contains(room))
        {
            dirtyRooms.Remove(room);
            Debug.Log($"Room at {room.transform.position} cleaned and removed from dirty list.");
        }
    }

    public void UpdateRoomAvailability()
    {
        int level = PlayerPrefs.GetInt("Level", 1);
        int roomsToUnlock = level;

        for (int i = 0; i < rooms.Count; i++)
        {
            bool shouldUnlock = i < roomsToUnlock;
            rooms[i].SetUnlocked(shouldUnlock);
            if (!shouldUnlock)
            {
                rooms[i].SetOccupied(false);
                rooms[i].SetClean(true);
            }
        }
        Debug.Log($"Level {level}: {roomsToUnlock} room(s) unlocked");
    }

    void Update()
    {
        queuedCustomers.RemoveAll(c => c == null || c.HasPaid);
    }
}