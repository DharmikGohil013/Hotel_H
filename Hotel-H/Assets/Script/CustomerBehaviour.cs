using UnityEngine;
using System.Collections;

public class CustomerBehaviour : MonoBehaviour
{
    // References (set these in Inspector)
    public Transform entryPoint;
    public Transform tablePoint;
    public Transform exitPoint;
    public GameObject moneyPrefab;
    public Transform moneyPlace;

    // Settings (adjust these in Inspector)
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float tableRange = 5f;
    [SerializeField] private float sleepDuration = 5f;
    [SerializeField] private float waitAtTableDuration = 2f;

    // Public properties
    public bool HasPaid { get; private set; }
    public bool IsAtTable { get; private set; }

    // Private variables
    private Room assignedRoom;
    private CustomerManager customerManager;
    private Animator animator;
    private bool isLeaving = false;

    private void Start()
    {
        InitializeComponents();
        StartCoroutine(CustomerLifecycleRoutine());
    }

    private void InitializeComponents()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        animator = GetComponent<Animator>();

        if (customerManager == null)
            Debug.LogError("CustomerManager not found in scene!");

        if (entryPoint == null || tablePoint == null || exitPoint == null)
            Debug.LogError("Movement points not assigned in Inspector!");
    }

    private IEnumerator CustomerLifecycleRoutine()
    {
        yield return EnterHotel();
        yield return WaitAtTable();
        yield return UseRoom();
        yield return LeaveHotel(); // Changed to ensure exit point visit
    }

    private IEnumerator EnterHotel()
    {
        // Move to queue position
        Vector3 queuePosition = customerManager.GetEntryQueuePosition(this);
        yield return MoveToPosition(queuePosition);

        // Wait turn in queue
        while (!customerManager.IsFirstInQueue(this))
            yield return null;

        // Move to table
        yield return MoveToPosition(tablePoint.position);
        IsAtTable = true;
    }

    private IEnumerator WaitAtTable()
    {
        // Wait for player to approach
        yield return new WaitUntil(IsOwnerInTableRange);
        yield return new WaitForSeconds(waitAtTableDuration);

        // Pay money
        ReleaseMoney();
    }

    private IEnumerator UseRoom()
    {
        // Get room assignment
        assignedRoom = customerManager.AssignRoom();
        if (assignedRoom == null)
        {
            Debug.Log("No available rooms, leaving early");
            yield break;
        }

        // Move to room
        yield return MoveToPosition(assignedRoom.transform.position);

        // Sleep in room
        if (animator != null)
            animator.SetBool("IsSleeping", true);

        yield return new WaitForSeconds(sleepDuration);

        if (animator != null)
            animator.SetBool("IsSleeping", false);

        // Mark room dirty when leaving
        assignedRoom.SetOccupied(false);
        assignedRoom.SetClean(false);
    }

    private IEnumerator LeaveHotel()
    {
        isLeaving = true;

        // First go to shop exit point
        if (exitPoint != null)
        {
            yield return MoveToPosition(exitPoint.position);
            Debug.Log("Customer reached exit point - now destroying");
        }
        else
        {
            Debug.LogWarning("Exit point not assigned!");
        }

        // Then destroy the customer
        Destroy(gameObject);
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        if (animator != null)
            animator.SetBool("IsWalking", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            RotateTowards(targetPosition);
            yield return null;
        }

        if (animator != null)
            animator.SetBool("IsWalking", false);
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    private bool IsOwnerInTableRange()
    {
        Player owner = FindObjectOfType<Player>();
        return owner != null &&
               Vector3.Distance(owner.transform.position, tablePoint.position) < tableRange;
    }

    public void ReleaseMoney()
    {
        if (HasPaid || moneyPrefab == null || moneyPlace == null)
            return;

        Instantiate(moneyPrefab, moneyPlace.position, Quaternion.identity);
        HasPaid = true;
        Debug.Log($"{name} paid money!");
    }

    public void InteractWithOwner()
    {
        if (!HasPaid)
            ReleaseMoney();
    }
}