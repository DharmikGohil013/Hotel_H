using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    public bool IsOccupied { get; private set; }
    public bool IsDirty { get; private set; }
    public bool IsUnlocked { get; private set; }
    public GameObject dustbin;

    private CustomerManager customerManager;

    void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        HideDustbin(); // Start with dustbin hidden
    }

    public void SetOccupied(bool occupied) => IsOccupied = occupied;

    public void SetClean(bool isClean)
    {
        IsDirty = !isClean;
        if (IsDirty) ShowDustbin();
        else HideDustbin();
    }

    public void SetUnlocked(bool unlocked) => IsUnlocked = unlocked;

    public bool IsAvailable() => IsUnlocked && !IsOccupied && !IsDirty;

    public void CleanRoom()
    {
        if (IsDirty)
        {
            IsDirty = false;
            if (dustbin != null)
            {
                dustbin.SetActive(false);
            }
            Debug.Log("Room cleaned via dustbin interaction!");
        }
    }

    private void ShowDustbin()
    {
        if (dustbin != null)
        {
            dustbin.SetActive(true);
            // Make sure dustbin can be interacted with
            dustbin.GetComponent<Collider>().enabled = true;
        }
    }

    private void HideDustbin()
    {
        if (dustbin != null)
        {
            dustbin.SetActive(false);
        }
    }
}