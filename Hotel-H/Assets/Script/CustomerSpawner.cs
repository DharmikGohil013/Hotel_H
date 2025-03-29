using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour // Renamed to avoid confusion
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 10f;

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            // Ensure customerPrefab has CustomerBehaviour component
            CustomerBehaviour customer = customerObj.GetComponent<CustomerBehaviour>();
            if (customer == null)
            {
                Debug.LogError("Customer prefab is missing CustomerBehaviour component!");
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}