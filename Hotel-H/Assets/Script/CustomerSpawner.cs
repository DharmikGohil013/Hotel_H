using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab; // Reference to the customer prefab
    public Transform spawnPoint; // Where customers will appear
    public int maxQueueSize = 5; // Max people in line
    private Queue<GameObject> customerQueue = new Queue<GameObject>();

    void Start()
    {
        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        if (customerQueue.Count < maxQueueSize)
        {
            GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            customerQueue.Enqueue(newCustomer);
            newCustomer.GetComponent<Customer>().SetSpawner(this);
        }
    }

    public void RemoveCustomer()
    {
        if (customerQueue.Count > 0)
        {
            customerQueue.Dequeue();
        }

        Invoke("SpawnCustomer", 2f); // Delay before spawning a new one
    }
}
