using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Vector3 moveDirection;
    private Rigidbody rb;
    private CustomerManager customerManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        customerManager = FindObjectOfType<CustomerManager>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void Update()
    {
        HandleTouchInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchEndPos = touch.position;
                Vector2 direction = touchEndPos - touchStartPos;
                direction.Normalize();
                moveDirection = new Vector3(direction.x, 0, direction.y).normalized;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                moveDirection = Vector3.zero;
            }
        }
        else
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(h, 0, v).normalized;
        }
    }

    void MovePlayer()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            Vector3 moveVelocity = moveDirection * moveSpeed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money"))
        {
            Destroy(other.gameObject);
            int currentMoney = PlayerPrefs.GetInt("Money", 0) + 100;
            PlayerPrefs.SetInt("Money", currentMoney);
            PlayerPrefs.Save();
            Debug.Log("💰 Money Collected! Total: " + currentMoney);
        }
        else if (other.CompareTag("Dustbin"))
        {
            Room room = other.GetComponentInParent<Room>();
            if (room != null)
            {
                room.CleanRoom();
                Debug.Log("🧹 Room cleaned via dustbin interaction!");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collided with wall!");
        }
    }
}