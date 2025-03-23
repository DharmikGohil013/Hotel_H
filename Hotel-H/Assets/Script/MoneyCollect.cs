using UnityEngine;

public class MoneyCollect : MonoBehaviour
{
    public int moneyValue = 10; // Amount added when collected

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Check if player touches the money
        {
            // Find the MoneyManager script in the scene
            MoneyManager moneyManager = FindObjectOfType<MoneyManager>();

            if (moneyManager != null)
            {
                moneyManager.AddMoney(moneyValue); // Add money
            }

            Destroy(gameObject); // Remove the money object
        }
    }
}
