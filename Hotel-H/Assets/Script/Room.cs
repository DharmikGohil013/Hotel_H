using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isOccupied = false;

    public void AssignCustomer(Customer customer)
    {
        isOccupied = true;
        customer.AssignRoom(this);
    }

    public void CustomerEntered()
    {
        Debug.Log("Customer entered the room!");
    }

    public void CustomerExited()
    {
        Debug.Log("Customer left the room!");
        isOccupied = false;
    }
}
