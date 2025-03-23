using UnityEngine;

public class Customer : MonoBehaviour
{
    private Room assignedRoom;
    private bool isMoving = false;
    private float speed = 3f;
    private CustomerSpawner spawner;

    public void SetSpawner(CustomerSpawner _spawner)
    {
        spawner = _spawner;
    }

    public void AssignRoom(Room room)
    {
        assignedRoom = room;
        isMoving = true;
    }

    void Update()
    {
        if (isMoving && assignedRoom != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, assignedRoom.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, assignedRoom.transform.position) < 0.1f)
            {
                isMoving = false;
                assignedRoom.CustomerEntered();
                Invoke("LeaveRoom", 3f);
            }
        }
    }

    void LeaveRoom()
    {
        assignedRoom.CustomerExited();
        Destroy(gameObject); // Remove customer after leaving
    }
}
